using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Throwing : MonoBehaviour
{

    public GameObject prefab;
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray r = cam.ScreenPointToRay(Input.mousePosition);

            Vector3 dir = r.GetPoint(1) - r.GetPoint(0);

            // position of spanwed object could be 'GetPoint(0).. 1.. 2' half random choice ;)
            GameObject bullet = Instantiate(prefab, r.GetPoint(2), Quaternion.LookRotation(dir));

            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 20;
            Destroy(bullet, 3);
        }

    }
}



