using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTInventory
{
    /// <summary>
    /// Choose interaction type of pickup behavior
    /// raycastFromCamera - FPS style. When you look at the object you can pickup it with use key press
    /// clickToPickup - Pickup an item with mouse click on it
    /// triggerPickup - Item will be picked up if player close to item and pickup button was pressed
    /// </summary>
    public enum InteractionType { raycastFromCamera, clickToPickup, triggerPickup }

    public class PickupItem : MonoBehaviour
    {

        

        public GameObject myHands; //reference to your hands/the position where you want your object to go
        GameObject ObjectIwantToPickUp; // the gameobject onwhich you collided with
        bool hasItem; // a bool to see if you have an item in your hand
        GameObject MainCamera;
        int layerMask;
        public GameObject prefab;
        public Camera Camera;

        public InteractionType interactionType;
        public KeyCode pickupKey = KeyCode.F;
        public Transform playerCamera;

        public float raycastPickupDistance = 3f;

        [HideInInspector]
        public DTInventory inventory;

        public Text itemNameTooltip;

        public GameObject AI;

        private void Start()
        {
            if(itemNameTooltip!=null)
                itemNameTooltip.text = string.Empty;

            if (playerCamera == null && Camera.main != null)
                playerCamera = Camera.main.transform;

            if (inventory == null)
                inventory = FindObjectOfType<DTInventory>();

            hasItem = false;
            MainCamera = GameObject.FindWithTag("MainCamera");
            layerMask = 1 << 8; // 8th layer = item
            Transform cam = MainCamera.transform;
            Ray ray = new Ray(cam.position, cam.forward);
            

            
        }

        private void Update()
        {
            switch (interactionType)
            {
                case InteractionType.clickToPickup:
                    //Do nothing. Behavior presented by Unity OnPonterClick interface. No need for realization special method
                    break;
                case InteractionType.raycastFromCamera:
                    PickupWithRaycast();
                    break;
                case InteractionType.triggerPickup:
                    //Do nothing. Behavior presented by OnTriggerEnter method
                    break;
            }

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
                { if (!(AI is null))
                        AI.transform.GetComponent<AI_test>().GetDistracted(hit.point);
                
                targetPoint = hit.point;
                    }
                else
                    targetPoint = ray.GetPoint(1000); // You may need to change this value according to your needs
                                                      // Create the bullet and give it a velocity according to the target point computed before


                if (Physics.Raycast(ray, out RaycastHit hitinfo))
                {
                    if (!(AI is null))
                        AI.transform.GetComponent<AI_test>().GetDistracted(hitinfo.point);
                }


                ObjectIwantToPickUp.GetComponent<Rigidbody>().isKinematic = false; // make the rigidbody work again'
                ObjectIwantToPickUp.GetComponent<MeshCollider>().isTrigger = false;
                ObjectIwantToPickUp.GetComponent<Rigidbody>().velocity = (targetPoint - myHands.transform.position).normalized * 10;
                ObjectIwantToPickUp.transform.parent = null; // make the object no be a child of the hands
                hasItem = false;
            }
        }

      
        bool objectInFront()
        {
            RaycastHit hit;
            if (Physics.Raycast(MainCamera.transform.position, MainCamera.transform.forward, out hit, 2f, layerMask))
            {
                ObjectIwantToPickUp = hit.transform.gameObject;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void PickupWithRaycast()
        {
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, raycastPickupDistance))
            {
                if(itemNameTooltip)
                    itemNameTooltip.text = string.Empty;

                if (hit.collider.CompareTag("Item") && hit.collider.GetComponent<Item>() != null)
                {
                    var item = hit.collider.GetComponent<Item>();

                    if (itemNameTooltip)
                    {
                        if(item.stackable)
                            itemNameTooltip.text = string.Format("{0}x{1}", item.title, item.stackSize);
                        else
                            itemNameTooltip.text = string.Format("{0}", item.title);
                    }

                    if (Input.GetKeyDown(pickupKey) && hasItem == false)
                        
                    inventory.AddItem(hit.collider.GetComponent<Item>());
                    



                }

            }

        }

        

       private void OnTriggerStay(Collider other)
        {
            //print(other.name);

            if (interactionType != InteractionType.triggerPickup)
            {
                
                //print("Return");
                return;
            }

            if (objectInFront())

               
            if (Input.GetKeyDown(pickupKey))
            {
                    

                    if (!other.GetComponent<Item>()) print("Other item equal null");
                if (other.GetComponent<Item>()) print("Other item has item component");

                

                if (other.CompareTag("Item") && hasItem == false && other.GetComponent<Item>() != null)
                {
                    inventory.AddItem(other.GetComponent<Item>());
                }
            }
        }

        

    }
}