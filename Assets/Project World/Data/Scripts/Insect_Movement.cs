using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Insect_Movement : MonoBehaviour
{
    public Transform[] legTargets;
    public float stepSize = 0.15f;
    public int smoothness = 8;
    public float stepHeight = 0.15f;
    public float sphereCastRadius = 0.125f;
    public bool bodyOrientation = true;

    public float raycastRange = 1.5f;
    private Vector3[] defaultLegPositions;
    private Vector3[] lastLegPositions;
    private Vector3 lastBodyUp;
    private bool[] legMoving;
    private int nbLegs;

    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastBodyPos;

    private float velocityMultiplier = 15f;

    Vector3[] MatchToSurfaceFromAbove(Vector3 point, float halfRange, Vector3 up)
    {
        // Initialize the result array
        Vector3[] res = new Vector3[2];

        // Set the second element of the result array to zero vector
        res[1] = Vector3.zero;

        RaycastHit hit;
        Ray ray = new Ray(point + halfRange * up / 2f, -up);

        // Perform a spherecast from above to find a surface below
        if (Physics.SphereCast(ray, sphereCastRadius, out hit, 2f * halfRange))
        {
            // Store the hit point and normal in the result array
            res[0] = hit.point;
            res[1] = hit.normal;
        }
        else
        {
            // If no surface is found, set the result position to the input point
            res[0] = point;
        }

        return res;
    }
    void Start()
    {
        // Store the initial upward direction of the body
        lastBodyUp = transform.up;

        // Initialize variables related to legs
        nbLegs = legTargets.Length;
        defaultLegPositions = new Vector3[nbLegs];
        lastLegPositions = new Vector3[nbLegs];
        legMoving = new bool[nbLegs];

        // Iterate over each leg
        for (int i = 0; i < nbLegs; ++i)
        {
            // Store the default position of the leg
            defaultLegPositions[i] = legTargets[i].localPosition;

            // Store the initial position of the leg
            lastLegPositions[i] = legTargets[i].position;

            // Set the leg's moving state to false initially
            legMoving[i] = false;
        }

        // Store the initial position of the body
        lastBodyPos = transform.position;
    }

    IEnumerator PerformStep(int index, Vector3 targetPoint)
    {
        // Store the starting position of the leg
        Vector3 startPos = lastLegPositions[index];

        // Perform the step over multiple frames using Lerp
        for (int i = 1; i <= smoothness; ++i)
        {
            // Calculate the interpolation factor based on the current frame and smoothness
            float t = i / (float)(smoothness + 1f);

            // Calculate the intermediate position using Lerp
            legTargets[index].position = Vector3.Lerp(startPos, targetPoint, t);

            // Apply vertical displacement to simulate step height using the sine function

            // Calculate the vertical offset based on the interpolation factor and PI
            // The value of Mathf.Sin ranges from -1 to 1, producing an oscillating effect
            float verticalOffset = Mathf.Sin(t * Mathf.PI) * stepHeight;

            // Apply the vertical offset to the leg position by moving it along the transform's up direction
            legTargets[index].position += transform.up * verticalOffset;

            // Wait for the next fixed update frame
            yield return new WaitForFixedUpdate();
        }

        // Set the final position of the leg to the target point
        legTargets[index].position = targetPoint;

        // Update the last position of the leg
        lastLegPositions[index] = legTargets[index].position;

        // Set the leg moving state to false
        legMoving[0] = false;
    }


    void FixedUpdate()
    {
        // Calculate the velocity of the body
        velocity = transform.position - lastBodyPos;
        velocity = (velocity + smoothness * lastVelocity) / (smoothness + 1f);

        // Check if the velocity is below a threshold and update the velocity accordingly
        if (velocity.magnitude < 0.000025f)
            velocity = lastVelocity;
        else
            lastVelocity = velocity;

        // Determine the desired positions of the legs
        Vector3[] desiredPositions = new Vector3[nbLegs];
        int indexToMove = -1;
        float maxDistance = stepSize;
        for (int i = 0; i < nbLegs; ++i)
        {
            desiredPositions[i] = transform.TransformPoint(defaultLegPositions[i]);

            // Calculate the distance between the desired position and the current leg position projected onto the horizontal plane
            float distance = Vector3.ProjectOnPlane(desiredPositions[i] + velocity * velocityMultiplier - lastLegPositions[i], transform.up).magnitude;

            // Find the leg with the maximum distance and set it as the leg to move
            if (distance > maxDistance)
            {
                maxDistance = distance;
                indexToMove = i;
            }
        }

        // Move all legs except the one being moved
        for (int i = 0; i < nbLegs; ++i)
        {
            if (i != indexToMove)
                legTargets[i].position = lastLegPositions[i];
        }

        // Check if there is a leg to move and it is not already in motion
        if (indexToMove != -1 && !legMoving[0])
        {
            // Calculate the target point for the leg movement
            Vector3 targetPoint = desiredPositions[indexToMove] + Mathf.Clamp(velocity.magnitude * velocityMultiplier, 0.0f, 1.5f) * (desiredPositions[indexToMove] - legTargets[indexToMove].position) + velocity * velocityMultiplier;

            // Match the target point to the surface by performing sphere casts
            Vector3[] positionAndNormalFwd = MatchToSurfaceFromAbove(targetPoint + velocity * velocityMultiplier, raycastRange, (transform.parent.up - velocity * 100).normalized);
            Vector3[] positionAndNormalBwd = MatchToSurfaceFromAbove(targetPoint + velocity * velocityMultiplier, raycastRange * (1f + velocity.magnitude), (transform.parent.up + velocity * 75).normalized);

            // Set the leg moving state to true
            legMoving[0] = true;

            // Start the coroutine to perform the leg movement based on the matched surface positions
            if (positionAndNormalFwd[1] == Vector3.zero)
            {
                StartCoroutine(PerformStep(indexToMove, positionAndNormalBwd[0]));
            }
            else
            {
                StartCoroutine(PerformStep(indexToMove, positionAndNormalFwd[0]));
            }
        }

        // Update the last position of the body
        lastBodyPos = transform.position;

        // Perform body orientation if there are more than 3 legs and body orientation is enabled
        if (nbLegs > 3 && bodyOrientation)
        {
            // Calculate the normal vector of the body based on the positions of legs
            Vector3 v1 = legTargets[0].position - legTargets[1].position;
            Vector3 v2 = legTargets[2].position - legTargets[3].position;
            Vector3 normal = Vector3.Cross(v1, v2).normalized;

            // Interpolate the upward direction of the body
            Vector3 up = Vector3.Lerp(lastBodyUp, normal, 1f / (float)(smoothness + 1));

            // Set the body's up vector and rotation based on the interpolated up direction
            transform.up = up;
            transform.rotation = Quaternion.LookRotation(transform.parent.forward, up);

            // Update the last body up vector
            lastBodyUp = transform.up;
        }
    }


    private void OnDrawGizmosSelected()
    {
        for (int i = 0; i < nbLegs; ++i)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(legTargets[i].position, 0.05f);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.TransformPoint(defaultLegPositions[i]), stepSize);
        }
    }
}
