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

    void Start()
    {
        hasItem = false;
        MainCamera = GameObject.FindWithTag("MainCamera");
        layerMask = 1 << 8; // 8th layer = item
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
            if (Input.GetKeyDown("e") && hasItem == false) // can be e or any key
            {
                ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = true;   //makes the rigidbody not be acted upon by forces
                ObjectIwantToPickUp.GetComponent<MeshCollider>().isTrigger = true;
                ObjectIwantToPickUp.transform.eulerAngles = myHands.transform.eulerAngles;
                ObjectIwantToPickUp.transform.position = myHands.transform.position; // sets the position of the object to your hand position
                ObjectIwantToPickUp.transform.parent = myHands.transform; //makes the object become a child of the parent so that it moves with the hands
                hasItem = true;
            }
        }
        if (Input.GetKeyDown("q") && hasItem == true) // if you have an item and get the key to remove the object, again can be any key
        {
            ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again'
            ObjectIwantToPickUp.GetComponent<MeshCollider>().isTrigger = false;
            ObjectIwantToPickUp.transform.parent = null; // make the object no be a child of the hands
            hasItem = false;
        }
    }
}