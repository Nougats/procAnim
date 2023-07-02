using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class ChainTail : MonoBehaviour
{
    public Transform target;
    public Transform tipJoint;
    public Transform[] tailJoints;
    public ChainIKConstraint tailChainIK;


    public LayerMask whatIsTarget;
    public float sightRange;
    public bool targetInSightRange;
    private State state;

    private enum State
    {
        Normal,
        TargetFound
    }

    private void Awake()
    {
        state = State.Normal;
    }

    private void Update()
    {
        targetInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsTarget);

        switch (state)
        {
            default:
            case State.Normal:
                TailNormal();
                break;
            case State.TargetFound:
                TailTargetFound();
                break;
        }

    }

        void TailNormal()
        {
        Vector3 normalPosition = tipJoint.position;
        if (!targetInSightRange)
        {
            //Update the positions of the tail joints
            for (int i = 0; i < tailJoints.Length; i++)
            {

                //Calculate the weight for each tail joint based on its distance from the target
                float weight = (i / tailJoints.Length);

                //Set the position and weight of the tail joint
                foreach (Transform tailJoint in tailJoints)
                {
                    tailChainIK.data.target.position = Vector3.Lerp(tailJoints[i].position, normalPosition, weight);
                    tailChainIK.data.chainRotationWeight = weight;
                }
            }
            //state = State.Normal;
        }
        }

    void TailTargetFound()
    {

        //Update the targets position baed on your desired logic
        //For Example, you can make the tail follow the player's location
        Vector3 targetPosition = target.position;

        if (targetInSightRange)
        {
            //Update the positions of the tail joints
            for (int i = 0; i < tailJoints.Length; i++)
            {

                //Calculate the weight for each tail joint based on its distance from the target
                float weight = (i / tailJoints.Length);

                //Set the position and weight of the tail joint
                foreach (Transform tailJoint in tailJoints)
                {
                    tailChainIK.data.target.position = Vector3.Lerp(tailJoints[i].position, targetPosition, weight);
                    tailChainIK.data.chainRotationWeight = weight;
                }
            }
            //state = State.TargetFound;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(tipJoint.position, sightRange);
    }
}
