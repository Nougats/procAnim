using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCaster : MonoBehaviour
{
    [SerializeField] float checkRange = 1f;
    [SerializeField] float orientationSpeed = 50f;
    [SerializeField] LayerMask groundLayer;

    private Vector3 lastUp;
    private float t;

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;

        RaycastHit hit;
        Physics.Raycast(transform.position , Vector3.up * 1f, out hit, checkRange, groundLayer);

        Vector3 newUp = Vector3.Lerp(lastUp, hit.normal, t * orientationSpeed);
        var targetRotation = GetTargetRotation(transform.forward, newUp);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, orientationSpeed * Time.deltaTime);

        lastUp = transform.up;
    }

    Quaternion GetTargetRotation(Vector3 approximateForward, Vector3 exactUp)
    {
        Quaternion zToUp = Quaternion.LookRotation(exactUp, -approximateForward);
        Quaternion yToZ = Quaternion.Euler(90, 0, 0);
        return zToUp * yToZ;
    }
}
