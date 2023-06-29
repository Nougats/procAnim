using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationLock : MonoBehaviour
{
    public float lockedXRotation = 60f;
    public float lockedZRotation = 60f;
    public float lockedYRotation = 210f;

    public bool xLock = false;
    public bool zLock = false;
    public bool yLock = false;

    private Quaternion targetRotation;

    private void Start()
    {
   
        LockRotation();
    }

    private void Update()
    {
        transform.rotation = targetRotation;
    }

    private void LockRotation()
    {
        Quaternion currentRotation = transform.rotation;
        Vector3 eulerRotation = currentRotation.eulerAngles;

        //XZ Lock
        if(xLock == true)
        eulerRotation.x = lockedXRotation;
        if(zLock == true)
        eulerRotation.z = lockedZRotation;

        //Y Lock
        if(yLock == true)
        eulerRotation.y = lockedYRotation;

        targetRotation = Quaternion.Euler(eulerRotation);
    }
}
