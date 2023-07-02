using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastBot : MonoBehaviour
{
    public GameObject topCast;
    //public GameObject bottomCast;

    public GameObject targetSphere;
    
     public LayerMask groundMask;
    // Start is called before the first frame update
    private void Awake()
    {
        targetSphere.transform.position = this.transform.position;
    }
    void RayCastIntersection() { 
    
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 5f, groundMask)) ;
            //&& Physics.Raycast(transform.position, Vector3.up, out hit, 100.0f))
        {
            Debug.Log("Collision with Ground");
            Debug.DrawRay(topCast.transform.position, -Vector3.up, Color.red);
            if(hit.point != null)
            {
                targetSphere.transform.localPosition = hit.point;
            }
            //targetSphere.transform.position = transform.position;

            //Debug.DrawRay(bottomCast.transform.position, Vector3.up, Color.blue);
            //        targetSphere.transform.position = 0;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        RayCastIntersection();
    }
}
