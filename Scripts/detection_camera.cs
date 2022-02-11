using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detection_camera : MonoBehaviour
{
    bool isInTrigger;
    Light cctvLight;

    public GameObject player;
    public GameObject AI;

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        if(isInTrigger){
            print("Alert!!");
            AI.GetComponent<AI_test>().SetTarget(player.transform.position);
        }
    }

    void Start()
    {
        cctvLight = transform.parent.GetComponent<Light>();
    }
    void Update()
    {
    }
    
    void OnTriggerEnter(Collider other)
    {
        RaycastHit hit;
        if(other.tag == "Player" && !Physics.Linecast(transform.parent.position, player.transform.position, ~((1 << 7) + (1 << 10)))){
            cctvLight.color = Color.red;
            isInTrigger = true;
            StartCoroutine(ExecuteAfterTime(0.3f));
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
