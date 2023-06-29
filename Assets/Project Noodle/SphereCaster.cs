using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereCaster : MonoBehaviour
{
    public float outerSphereRadius = 50f;
    public float innerSphereRadius = 40f;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, outerSphereRadius);
        Gizmos.DrawSphere(transform.position, innerSphereRadius);
    }
}
