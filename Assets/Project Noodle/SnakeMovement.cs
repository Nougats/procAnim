using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SnakeMovement : MonoBehaviour
{

    public Transform[] spineJoints;
    public MultiParentConstraint spineMultiParent;
    public TwoBoneIKConstraint[] ikConstraints;
    public Transform target;
    public float slitherSpeed = 1f;
    public float maxRotationAngle = 30f;

    private float timeOffset;

    // Start is called before the first frame update
    void Start()
    {
        //Set animation variation time offset
        timeOffset = Random.Range(0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        //Calculate the normalized time base on slitherSpeed
        float time = Time.time * slitherSpeed + timeOffset;

        //Update the position and rotations of the spine joints
        for(int i = 0; i < spineJoints.Length; i++)
        {

            //Calculate the rotation angle base on time and index
            float rotationAngle = Mathf.Sin(time + i) * maxRotationAngle;

            //Apply rotation tot he spine joint
            spineJoints[i].localRotation = Quaternion.Euler(rotationAngle, 0f, 0f);
        }

        //Update the target position based on your desired logic
        //For Example, you can make the snake follow a path or target the player's position

        Vector3 targetPosition = target.transform.position;

        //Update the positions and weights of the IK constraints
        foreach(TwoBoneIKConstraint ikConstraint in ikConstraints)
        {
            //ikConstraint.data.target = target.transform.position;
        }
    }
}
