using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Chain : MonoBehaviour
{
    public Rig chainRig;
    public Transform chainTip;
    public Transform targetHit;


    int grapple = 0;
    public float rigSpeed = 3f;
    public float sphereRadius;

    void ChainChecker()
    {
        if (Input.GetKeyDown(KeyCode.B)) grapple = 1;
        else if (Input.GetKeyDown(KeyCode.N)) grapple = 2;

        if(grapple == 2)
        {
            chainRig.weight = Mathf.MoveTowards(chainRig.weight, 0, rigSpeed * Time.deltaTime);
            if (chainRig.weight == 0) grapple = 0;
        }
        if(grapple == 1)
        {
            chainRig.weight = Mathf.MoveTowards(chainRig.weight, 0.8f, rigSpeed * Time.deltaTime);
            if (chainRig.weight == 0) grapple = 0;
        }
    }

    private void Update()
    {
        ChainChecker();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(chainTip.transform.position, sphereRadius);
    }
}
