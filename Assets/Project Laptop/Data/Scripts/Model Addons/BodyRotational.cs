using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyRotational : MonoBehaviour
{
    //Rotieren des Körpers zur Ausrichtung des Bodens

    [SerializeField] private float speed = 0.1f;

    private Mesh bodyMesh;

    private void Start()
    {
        //Methode zum Finden des Mittelpunktes des Meshes schreiben
        //bodyMesh = GetComponent<Mesh>();
    }
    
    private void BodyRotation()
    {
        RaycastHit hit;
        Ray heightDetection = new Ray(transform.position + Vector3.up *0.5f, Vector3.down);
        

        if(Physics.Raycast(heightDetection, out hit, Mathf.Infinity))
        {
            var targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);

            Debug.DrawRay(heightDetection.origin, heightDetection.direction * 100);
            DrawPlane(heightDetection.origin, heightDetection.direction);
        }

    }


    public void DrawPlane(Vector3 position, Vector3 normal)
    {
        Vector3 v3;
        if (normal.normalized != Vector3.forward)
            v3 = Vector3.Cross(normal, Vector3.forward).normalized * normal.magnitude;
        else
            v3 = Vector3.Cross(normal, Vector3.up).normalized * normal.magnitude;

        var corner0 = position + v3;
        var corner2 = position - v3;
        var q = Quaternion.AngleAxis(90.0f, normal);
        v3 = q * v3;
        var corner1 = position + v3;
        var corner3 = position - v3;
        Debug.DrawRay(corner0, Vector3.down);
        Debug.DrawRay(corner1, Vector3.down);
        Debug.DrawRay(corner2, Vector3.down);
        Debug.DrawRay(corner3, Vector3.down);
        Debug.DrawLine(corner0, corner2, Color.green);
        Debug.DrawLine(corner1, corner3, Color.green);
        Debug.DrawLine(corner0, corner1, Color.green);
        Debug.DrawLine(corner1, corner2, Color.green);
        Debug.DrawLine(corner2, corner3, Color.green);
        Debug.DrawLine(corner3, corner0, Color.green);
        Debug.DrawRay(position, normal, Color.red);
    }

    // Update is called once per frame
    void Update()
    {
        BodyRotation();
    }
}
