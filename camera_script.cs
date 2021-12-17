using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{
    int layerMask;

    public float minAngle = -50.0f;
    public float maxAngle = 50.0f;

    float offset;
    Vector3 objectEulerAngles;
    Vector3 cameraEulerAngles;
    GameObject Player;

    bool moving;
    bool rotated;
    
    Vector3 nextPos;

    void Start(){
        Player = GameObject.FindWithTag("Player");

        layerMask = 1 << 9;
    }

    void Update(){
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        float useInput = Input.GetAxis("Use");

        Vector3 forw = transform.forward;
        Vector3 dirHoriz = Quaternion.Euler(0, 90, 0) * forw;
        
        if(!rotated){
            transform.position -= dirHoriz * offset;
            offset = 0;
        }

        float rotationX = Mathf.Clamp(mouseY + cameraEulerAngles.x, minAngle, maxAngle);
        objectEulerAngles += new Vector3(0, mouseX, 0);
        cameraEulerAngles = new Vector3(rotationX, objectEulerAngles.y, 0);
        
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
                    offset = 0.5f;
                }
                else{
                    offset = -0.5f;
                }
                transform.position += dirHoriz * offset;
            }
        }
        if(Input.GetMouseButtonUp(1) || !Physics.Raycast(Player.transform.position, forw, 1f)){
            rotated = false;
        }

        if(Input.GetKeyDown("e")){
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, layerMask)){
                GameObject door1 = hit.transform.GetChild(0).gameObject;
                GameObject door2 = hit.transform.GetChild(1).gameObject;
                if(door1.GetComponent<Door_script>().open){
                    door1.GetComponent<Door_script>().closeDoor();
                    door2.GetComponent<Door_script>().closeDoor();
                }
                else{
                    door1.GetComponent<Door_script>().openDoor();
                    door2.GetComponent<Door_script>().openDoor();
                }
            }
        }
    }
}