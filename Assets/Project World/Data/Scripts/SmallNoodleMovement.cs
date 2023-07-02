using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmallNoodleMovement : MonoBehaviour
{
    public GameObject noodlePointer;
    public float noodleRotationSpeed;
    float timeCounter = 0;
    
    private float randomX = 0f;
    private float randomZ = 0f;
    private float randomY = 0f;

    void Update()
    {
        var noodlePosition = noodlePointer.transform.position;
        timeCounter += Time.deltaTime;
        if(randomX == 0)
        {
            randomX = Random.Range(-5f, 5f);
        }
        if (randomZ == 0)
        {
            randomZ = Random.Range(-5f, 5f);
        }
        if (randomY == 0)
        {
            randomY = Random.Range(-5f, 5f);
        }
        float x = Mathf.Sin(timeCounter);
        float y = Mathf.Cos(timeCounter);
        
        //transform.position = new Vector3(noodlePosition.x + x + randomX, noodlePosition.y + y + randomY, noodlePosition.z + randomZ);
        transform.RotateAround(noodlePosition, Vector3.right, noodleRotationSpeed * Time.deltaTime);
        
    }

    
}
