using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class EnemyChecker : MonoBehaviour
{
    //Detection
    public Transform targetEnemy;
    public LayerMask whatIsEnemy;
    public GameObject headBone;

    public float sightRange;
    public float attackRange;

    public bool enemyInSightRange;
    public bool enemyInAttackRange;

    //ChainIK
    public Rig chainIKRig;
    public Transform chainTip;
    public bool isHooked;
    public float rigSpeed = 0.2f;


    private void Awake()
    {
        targetEnemy = GameObject.FindGameObjectWithTag("Enemy").transform;
        isHooked = false;
    }

    private void Update()
    {
        enemyInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
        enemyInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);

        if(!enemyInSightRange && !enemyInAttackRange)
        {
            Debug.Log("Nothing in Range");
            //Do nothing, chill relax, maybe play a default animation IDK
        }
        if(enemyInSightRange && !enemyInAttackRange)
        {
            Debug.Log("Enemy in Sight Range");
            headBone.transform.LookAt(targetEnemy); //this gotta be written in its own function
            chainIKRig.weight = Mathf.MoveTowards(chainIKRig.weight, 0, rigSpeed * Time.deltaTime);
            //Debug.Log(chainIKRig.weight);
            isHooked = false;
            Debug.Log(isHooked);
        }
        if(enemyInSightRange && enemyInAttackRange)
        {
            Debug.Log("Enemy in Attack Range");
            chainIKRig.weight = Mathf.MoveTowards(chainIKRig.weight, 1, rigSpeed * Time.deltaTime);
            //Debug.Log(chainIKRig.weight);
            Debug.Log("Moving in to Kill");
            isHooked = true;
            Debug.Log(isHooked);
        }

    }

    private void ToHookOrNot()
    {
        if (!isHooked && enemyInAttackRange)
        {
            chainIKRig.weight = Mathf.MoveTowards(chainIKRig.weight, 1, rigSpeed * Time.deltaTime);
            isHooked = true;
        }
        if(isHooked && !enemyInAttackRange)
        {
            chainIKRig.weight = Mathf.MoveTowards(chainIKRig.weight, 0, rigSpeed * Time.deltaTime);
            isHooked = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, sightRange);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    //private void Start()
    //{
    //    targetEnemy = GameObject.FindGameObjectWithTag("Enemy").transform;
    //}

    //private void Update()
    //{
    //    transform.LookAt(targetEnemy);
    //}

}
