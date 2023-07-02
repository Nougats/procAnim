using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Poking : MonoBehaviour
{

    public Rig chain_rig;
    public float sphereRadius;
    public GameObject tailTip;
    public LayerMask hitMask;
    public Transform targetHit;
    public float weight_speed = 0.2f;

    int attack_ID;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B)) attack_ID = 1;
        else if (Input.GetKeyDown(KeyCode.N)) attack_ID = 2;

        if (attack_ID == 1)
        {
            chain_rig.weight = Mathf.MoveTowards(chain_rig.weight, 1, weight_speed * Time.deltaTime);
            if (chain_rig.weight == 1) attack_ID = 0;
        }

        else if (attack_ID == 2)
        {
            chain_rig.weight = Mathf.MoveTowards(chain_rig.weight, 0, weight_speed * Time.deltaTime);
            if (chain_rig.weight == 0) attack_ID = 0;
        }
        //RadiusChecker();
        //MoveAround();
    }

    //void RadiusChecker()
    //{
    //    if (Physics.CheckSphere(tailTip.transform.position, sphereRadius))
    //    {
    //        Mathf.MoveTowards(chain_rig.weight, 1, weight_speed * Time.deltaTime);
    //    }
    //    else
    //        Mathf.MoveTowards(chain_rig.weight, 0, weight_speed * Time.deltaTime);
    //}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(tailTip.transform.localPosition, sphereRadius);
    }

    //void MoveAround()
    //{
    //    //chain_rig.weight = Mathf.MoveTowards(chain_rig.weight, 1, weight_speed * Time.deltaTime);
    //    if(Physics.CheckSphere(targetHit.transform.position, sphereRadius,hitMask))
    //    {
    //        Vector3.MoveTowards(tailTip.transform.position, targetHit.transform.position, weight_speed * Time.deltaTime);
    //    }
    //}
}
