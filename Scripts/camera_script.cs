using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    public float minAngle = -50.0f;
    public float maxAngle = 50.0f;

    Vector3 initOffset;
    Vector3 offset;
    Vector3 objectEulerAngles;
    Vector3 cameraEulerAngles;
    GameObject Player;

    bool rotated;
    
    void Start(){
        Player = GameObject.FindWithTag("Player");
        initOffset = transform.position - Player.transform.position;
    }

    void Update(){
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        Vector3 forw = transform.forward;
        Vector3 dirHoriz = Quaternion.Euler(0, 90, 0) * forw;
        
        if(!rotated){
            offset = initOffset;
        }

        float rotationX = Mathf.Clamp(mouseY + cameraEulerAngles.x, -50f, 50f);
        objectEulerAngles += new Vector3(0, mouseX, 0);
        cameraEulerAngles = new Vector3(rotationX, objectEulerAngles.y, 0);
        
        transform.position = Player.transform.position + offset;
        transform.eulerAngles = cameraEulerAngles;
        Player.transform.eulerAngles = objectEulerAngles;

        if(Input.GetMouseButton(1)){
            RaycastHit hit;
            
            Vector3 dirLeft = Quaternion.Euler(0, -45, 0) * forw;
            Vector3 dirRight = Quaternion.Euler(0, 45, 0) * forw;
            
            bool rayLeft = Physics.Raycast(transform.position, dirLeft, 1f);
            bool rayRight = Physics.Raycast(transform.position, dirRight, 1f);
            if (Physics.Raycast(transform.position, forw, 1f) && (rayLeft != rayRight) && !rotated){
                rotated = true;
                if(rayLeft){
                    offset += dirHoriz * 0.5f;
                }
                else{
                    offset += -dirHoriz * 0.5f;
                }
            }
        }
        if(Input.GetMouseButtonUp(1) || !Physics.Raycast(Player.transform.position, forw, 1f)){
            rotated = false;
        }
    }
}
