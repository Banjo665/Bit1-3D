using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup : MonoBehaviour
{
    public GameObject myHands; //reference to your hands/the position where you want your object to go
    GameObject ObjectIwantToPickUp; // the gameobject onwhich you collided with
    bool hasItem; // a bool to see if you have an item in your hand
    GameObject MainCamera;
    int layerMask;
    public GameObject prefab;
    public Camera Camera;

    void Start()
    {
        hasItem = false;
        MainCamera = GameObject.FindWithTag("MainCamera");
        layerMask = 1 << 8; // 8th layer = item
        Transform cam = MainCamera.transform;
        Ray ray = new Ray(cam.position, cam.forward);
        

    }

    bool objectInFront(){
        RaycastHit hit;
        if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, 2f, layerMask)){
            ObjectIwantToPickUp = hit.transform.gameObject;
            return true;
        }
        else{
            return false;
        }
    }

    void Update()
    {
        if (objectInFront()) // if you enter thecollider of the objecct
        {
            if (Input.GetKeyDown("z") && hasItem == false) // can be e or any key
            {
                ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;   //makes the rigidbody not be acted upon by forces
                ObjectIwantToPickUp.GetComponent<MeshCollider>().isTrigger = true;
                ObjectIwantToPickUp.transform.eulerAngles = myHands.transform.eulerAngles;
                ObjectIwantToPickUp.transform.position = myHands.transform.position; // sets the position of the object to your hand position
                ObjectIwantToPickUp.transform.parent = myHands.transform; //makes the object become a child of the parent so that it moves with the hands
                hasItem = true;
            }
        }
        if (Input.GetKeyDown("g") && hasItem == true) // if you have an item and get the key to remove the object, again can be any key
        {
            ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again'
            ObjectIwantToPickUp.GetComponent<MeshCollider>().isTrigger = false;
            ObjectIwantToPickUp.transform.parent = null; // make the object no be a child of the hands
            hasItem = false;
        }

        if (Input.GetKeyDown("v") && hasItem == true)
        {
            // Create a ray from the camera going through the middle of your screen
            Ray ray = Camera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            // Check whether your are pointing to something so as to adjust the direction
            Vector3 targetPoint;
            if (Physics.Raycast(ray, out hit))
                targetPoint = hit.point;
            else
                targetPoint = ray.GetPoint(1000); // You may need to change this value according to your needs
                                                  // Create the bullet and give it a velocity according to the target point computed before
            


        ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again'
            ObjectIwantToPickUp.GetComponent<MeshCollider>().isTrigger = false;
            ObjectIwantToPickUp.GetComponent<Rigidbody>().velocity = (targetPoint - myHands.transform.position).normalized * 10;
            ObjectIwantToPickUp.transform.parent = null; // make the object no be a child of the hands
            hasItem = false;
        }


    }
}