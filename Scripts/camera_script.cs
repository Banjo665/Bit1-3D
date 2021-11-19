using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    public float minAngle = -50.0f;
    public float maxAngle = 50.0f;

    Vector3 offset;
    Vector3 objectEulerAngles;
    Vector3 cameraEulerAngles;
    GameObject Player;
    
    void Start(){
        Player = GameObject.FindWithTag("Player");
        offset = transform.position - Player.transform.position;
    }

    void Update(){
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        offset = Vector3.Scale(offset, new Vector3(transform.forward.x, 1, transform.forward.z));

        float rotationX = Mathf.Clamp(mouseY + cameraEulerAngles.x, -50f, 50f);
        objectEulerAngles += new Vector3(0, mouseX, 0);
        cameraEulerAngles = new Vector3(rotationX, objectEulerAngles.y, 0);
        
        transform.position = Player.transform.position + offset;
        transform.eulerAngles = cameraEulerAngles;
        Player.transform.eulerAngles = objectEulerAngles;
    }
}
