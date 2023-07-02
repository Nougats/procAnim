using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Insect_Controller : MonoBehaviour
{
    public float _speed = 3f;
    public float smoothness = 5f;
    public int raysNb = 8;
    public float raysEccentricity = 0.2f;
    public float outerRaysOffset = 2f;
    public float innerRaysOffset = 25f;

    private Vector3 velocity;
    private Vector3 lastVelocity;
    private Vector3 lastPosition;
    private Vector3 forward;
    private Vector3 upward;
    private Quaternion lastRot;
    private Vector3[] pn;

    static Vector3[] GetClosestPoint(Vector3 point, Vector3 forward, Vector3 up, float halfRange, float eccentricity, float offset1, float offset2, int rayAmount)
    {
        // Initialize the result array with the input point and up vector
        Vector3[] res = new Vector3[2] { point, up };

        // Calculate the right vector using the cross product of up and forward vectors
        Vector3 right = Vector3.Cross(up, forward);

        // Initialize the normal and position amounts
        float normalAmount = 1f;
        float positionAmount = 1f;

        // Create an array to store the direction vectors
        Vector3[] dirs = new Vector3[rayAmount];

        // Calculate the angular step for each ray
        float angularStep = 2f * Mathf.PI / (float)rayAmount;
        float currentAngle = angularStep / 2f;

        // Calculate the direction vectors for each ray
        for (int i = 0; i < rayAmount; ++i)
        {
            dirs[i] = -up + (right * Mathf.Cos(currentAngle) + forward * Mathf.Sin(currentAngle)) * eccentricity;
            currentAngle += angularStep;
        }

        // Iterate over each direction vector
        foreach (Vector3 dir in dirs)
        {
            RaycastHit hit;
            Vector3 largener = Vector3.ProjectOnPlane(dir, up);

            // Create a ray starting from the point with an offset and direction
            Ray ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset1 / 100f, dir);

            // Perform a spherecast and check for collisions
            //Debug.DrawRay(ray.origin, ray.direction);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                res[0] += hit.point;
                res[1] += hit.normal;
                normalAmount += 1;
                positionAmount += 1;
            }

            // Create another ray with a different offset
            ray = new Ray(point - (dir + largener) * halfRange + largener.normalized * offset2 / 100f, dir);

            // Perform another spherecast and check for collisions
            //Debug.DrawRay(ray.origin, ray.direction, Color.green);
            if (Physics.SphereCast(ray, 0.01f, out hit, 2f * halfRange))
            {
                res[0] += hit.point;
                res[1] += hit.normal;
                normalAmount += 1;
                positionAmount += 1;
            }
        }

        // Calculate the average position and normal
        res[0] /= positionAmount;
        res[1] /= normalAmount;

        return res;
    }


    // Start is called before the first frame update
    void Start()
    {
        // Initialize variables
        velocity = new Vector3();
        forward = transform.forward;
        upward = transform.up;
        lastRot = transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Calculate velocity based on the position change
        velocity = (smoothness * velocity + (transform.position - lastPosition)) / (1f + smoothness);

        // If the velocity is very small, use the last saved velocity
        if (velocity.magnitude < 0.00025f)
            velocity = lastVelocity;

        // Update last position and velocity
        lastPosition = transform.position;
        lastVelocity = velocity;

        // Set the movement multiplier based on whether the left shift key is pressed
        float multiplier = 1f;
        if (Input.GetKey(KeyCode.LeftShift))
            multiplier = 2f;

        // Get input values for vertical and horizontal movement
        float valueY = Input.GetAxis("Vertical");
        float valueX = Input.GetAxis("Horizontal");

        // Move the transform based on the input values
        if (valueY != 0)
            transform.position += transform.forward * valueY * _speed * multiplier * Time.fixedDeltaTime;
        if (valueX != 0)
            transform.position += Vector3.Cross(transform.up, transform.forward) * valueX * _speed * multiplier * Time.fixedDeltaTime;

        // If there is any movement input
        if (valueX != 0 || valueY != 0)
        {
            // Get the closest point and updated upward vector
            pn = GetClosestPoint(transform.position, transform.forward, transform.up, 0.5f, 0.1f, 30, -30, 4);
            upward = pn[1];

            // Get closest point again and update transform position smoothly
            Vector3[] pos = GetClosestPoint(transform.position, transform.forward, transform.up, 0.5f, raysEccentricity, innerRaysOffset, outerRaysOffset, raysNb);
            transform.position = Vector3.Lerp(lastPosition, pos[0], 1f / (1f + smoothness));

            // Update forward vector and smoothly rotate the transform
            forward = velocity.normalized;
            Quaternion q = Quaternion.LookRotation(forward, upward);
            transform.rotation = Quaternion.Lerp(lastRot, q, 1f / (1f + smoothness));
        }

        // Update last rotation
        lastRot = transform.rotation;
    }
}