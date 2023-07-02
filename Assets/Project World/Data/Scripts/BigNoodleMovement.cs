using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigNoodleMovement : MonoBehaviour
{

    
    float timeCounter = 0;

    void Update()
    {
        var parentPosition = transform.parent.parent.localPosition;
        timeCounter += Time.deltaTime;
        float x = Mathf.Cos(timeCounter * 2);
        float y = Mathf.Sin(timeCounter * 2);
        float z = 0f;
        transform.position = new Vector3(parentPosition.x + x, parentPosition.y + y, parentPosition.z + z);
    }
}
