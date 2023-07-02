using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    public static Vector3 SetOnGround(Vector3 origin, LayerMask layerMask, float yOffset, float rayLength, float sphereCastRadius)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin + Vector3.up * yOffset, Vector3.down, out hit, rayLength, layerMask))
        {
            return hit.point;
        }
        else if (Physics.SphereCast(origin + Vector3.up * yOffset, sphereCastRadius, Vector3.down, out hit, rayLength, layerMask))
        {
            return hit.point;
        }
        else
        {
            return origin;
        }
    }

    public static bool IsValidPoint(Vector3 origin, LayerMask layerMask, float yOffset, float rayLength, float sphereCastRadius)
    {
        RaycastHit hit;

        if(Physics.SphereCast(origin + Vector3.up * yOffset, sphereCastRadius, Vector3.down, out hit, rayLength, layerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool IsPointInsideCollider(Vector3 point)
    {
        //yOffset so setzen, damit es nicht zufällig auf der Kante stehen kann
        Vector3 yOffset = new Vector3(0, 0.01f, 0);

        Collider[] hitColliderList = Physics.OverlapSphere(point + yOffset, 0f);
        bool isUnderCollider = Physics.Raycast(point, Vector3.up, 1);
        if(hitColliderList.Length > 0 || isUnderCollider)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
