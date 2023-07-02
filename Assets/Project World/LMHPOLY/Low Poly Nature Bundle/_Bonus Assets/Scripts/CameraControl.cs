using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    ////Range for min/max values of variable
    //[Range(-25f, 25f)]
    //public float cameraMoveSpeed_x, cameraMoveSpeed_y, cameraMoveSpeed_z;

    public float speed = 5f;
    public float sensitivity = 5f;
    //// Camera Movement

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        //gameObject.transform.Translate(cameraMoveSpeed_x * Time.deltaTime, cameraMoveSpeed_y * Time.deltaTime, cameraMoveSpeed_z * Time.deltaTime);


        transform.position += transform.forward * Input.GetAxis("Vertical") * speed * Time.deltaTime;
        transform.position += transform.right * Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        transform.eulerAngles += new Vector3(-mouseY * sensitivity, mouseX * sensitivity, 0);
    }


}
