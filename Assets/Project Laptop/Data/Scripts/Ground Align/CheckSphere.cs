using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSphere : MonoBehaviour
{
    public GameObject topCast;
    public LayerMask groundLayer;
    public GameObject targetSphere;
    public float orientationRotation = 50f;

    private Vector3 downCast;
    private float t;

    Quaternion GetTargetRotation(Vector3 fwd, Vector3 up)
    {
        Quaternion zUp = Quaternion.LookRotation(up, -fwd);
        Quaternion yZ = Quaternion.Euler(90, 0, 0);
        return zUp * yZ;
    }
    void StayGrounded()
    {
        t += Time.deltaTime;

        RaycastHit hit;
        Physics.Raycast(topCast.transform.position, Vector3.down, out hit, groundLayer);

        Vector3 recentCast = Vector3.Lerp(downCast, hit.normal, t * orientationRotation);
        var targetRotation = GetTargetRotation(targetSphere.transform.forward, recentCast);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, orientationRotation * Time.deltaTime);
        downCast = topCast.transform.up;
    }
    // Update is called once per frame
    void Update()
    {
        StayGrounded();
    }
}
