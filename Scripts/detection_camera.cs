using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detection_camera : MonoBehaviour
{
    bool isInTrigger;
    Light cctvLight;

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if(isInTrigger){
            print("Alert!!");
        }
    }

    void Start()
    {
        cctvLight = transform.parent.GetComponent<Light>();
    }
    
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player"){
            cctvLight.color = Color.red;
            isInTrigger = true;
            StartCoroutine(ExecuteAfterTime(1));
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player"){
            cctvLight.color = Color.green;
            isInTrigger = false;
        }
    }
}
