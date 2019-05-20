using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movecamera : MonoBehaviour
{
    public float cameraMoveSpeed = 5f;
    public float cameraRotSpeed = 30f;

    public Rect area = new Rect(10, 10, 600, 300);

    //private float trans_z = 0;
    private float eulerAngles_x;
    private float eulerAngles_y;
   // float thedistance = 1f;
   // float dis = 0.2f;
    CharacterController ctrlor;
      
    void Start()
    {

        Vector3 eulerAngles = this.transform.eulerAngles;

        this.eulerAngles_x = eulerAngles.y;

        this.eulerAngles_y = eulerAngles.x;
        ctrlor = GetComponent<CharacterController>();
    }


    void Update()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        if (Input.GetMouseButton(1))
        {

            this.eulerAngles_x += (Input.GetAxis("Mouse X") * this.cameraRotSpeed) * Time.deltaTime;

            this.eulerAngles_y -= (Input.GetAxis("Mouse Y") * this.cameraRotSpeed) * Time.deltaTime;

            Quaternion quaternion = Quaternion.Euler(this.eulerAngles_y, this.eulerAngles_x, (float)0);

            this.transform.rotation = quaternion;
        }

                if (Input.GetKey(KeyCode.A))
                {
                    Vector3 left = transform.TransformDirection(Vector3.left);
                    ctrlor.Move(left * cameraMoveSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    Vector3 right = transform.TransformDirection(Vector3.right);
                    ctrlor.Move(right * cameraMoveSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.W))
                {
                    ctrlor.Move(forward * cameraMoveSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    Vector3 back = transform.TransformDirection(Vector3.back);
                    ctrlor.Move(back * cameraMoveSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.Q))
                {
                    Vector3 up = transform.TransformDirection(Vector3.up);
                    ctrlor.Move(up * cameraMoveSpeed * Time.deltaTime);
                }
                if (Input.GetKey(KeyCode.E))
                {
                    Vector3 down = transform.TransformDirection(Vector3.down);
                    ctrlor.Move(down * cameraMoveSpeed * Time.deltaTime);
                }
                    
        //this.trans_z = (Input.GetAxis("Mouse ScrollWheel") * this.cameraMoveSpeed * 2) * Time.deltaTime;
       
        //ctrlor.Move(forward * this.trans_z);
    }
}


