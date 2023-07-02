using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public NavMeshAgent agent;
    [SerializeField] private LayerMask groundMask;

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Idle()
    {
        if (!walkPointSet)
        {
            SearchWalkPoint();
        }

        if(walkPointSet)
        {
            agent.SetDestination(walkPoint);
        }
        Vector3 distanceToWalkpoint = transform.position - walkPoint;

        if(distanceToWalkpoint.magnitude < 1f)
        {
            walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        //Calculate Random Point in Range
        float randZ;
        float randX;

        randZ = Random.Range(-walkPointRange, walkPointRange);
        randX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z + randZ);

        if(Physics.Raycast(walkPoint, -transform.up,10f,groundMask))
        { 
            walkPointSet = true;
        }
    }
    private void Update()
    {
        Idle();

        //Ray lookDirection = new Ray(transform.position,Vector3.forward);
        //Debug.DrawRay(lookDirection.origin, lookDirection.direction * 10f);
    }
}
