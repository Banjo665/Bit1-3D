using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_script : MonoBehaviour
{

    public float minAngle = -50.0f;
    public float maxAngle = 50.0f;

    float offset;
    float oldOffset = 0;
    Vector3 objectEulerAngles;
    Vector3 cameraEulerAngles;
    Vector3 dirHoriz;
    GameObject Player;

    float t;
    bool rotated = false;
    bool rotating;
    float rotateTo;
    public float rotatingSpeed = 1f;
    
    Vector3 nextPos;

    void Start(){
        Player = GameObject.FindWithTag("Player");
    }

    void Update(){
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");
        float useInput = Input.GetAxis("Use");

        Vector3 forw = transform.forward;
        dirHoriz = Quaternion.Euler(0, 90, 0) * forw;

        float rotationX = Mathf.Clamp(mouseY + cameraEulerAngles.x, minAngle, maxAngle);
        objectEulerAngles += new Vector3(0, mouseX, 0);
        cameraEulerAngles = new Vector3(rotationX, objectEulerAngles.y, 0);
        
        transform.eulerAngles = cameraEulerAngles;
        Player.transform.eulerAngles = objectEulerAngles;

        if(Input.GetMouseButton(1)){
            RaycastHit hit;
            
            Vector3 dirLeft = Quaternion.Euler(0, -30, 0) * forw;
            Vector3 dirRight = Quaternion.Euler(0, 30, 0) * forw;
            
            bool rayLeft = Physics.Raycast(transform.position, dirLeft, 1f);
            bool rayRight = Physics.Raycast(transform.position, dirRight, 1f);
            if (Physics.Raycast(transform.position, forw, 1f) && (rayLeft != rayRight) && !rotated && !rotating){
                rotating = true;
                if(rayLeft){
                    rotateTo = 0.3f;
                }
                else{
                    rotateTo = -0.3f;
                }
            }
        }
        if(rotated && (Input.GetMouseButtonUp(1) || !Physics.Raycast(Player.transform.position, forw, 2f))){
            rotating = true;
            rotateTo = 0;
        }
        

        if(Input.GetKeyDown("e")){
            int layerMask = 1 << 9; // button
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, layerMask)){
                Transform obj = hit.transform.parent;

                foreach(Transform tr in obj)
                {
                    if(tr.tag == "door")
                    {
                        if(tr.GetComponent<Door_script>().open){
                            tr.GetComponent<Door_script>().closeDoor();
                        }
                        else{
                            tr.GetComponent<Door_script>().openDoor();
                        }
                    }
                }
            }
        }
    }
    void FixedUpdate(){
        if(rotating){
            t += Time.deltaTime * rotatingSpeed;
            bool check;

            if(check = Mathf.Abs(rotateTo - offset) < 0.01){
                rotated = !rotated;
                rotating = false;
                t = 0;
            }
            else{
                offset = Mathf.Lerp(offset, rotateTo, t);
                transform.position += dirHoriz * (offset - oldOffset);
                oldOffset = offset;
            }
        }
    }
}