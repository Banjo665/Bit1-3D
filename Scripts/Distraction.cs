using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distraction : MonoBehaviour
{
    public GameObject Detection;
    bool coroutineIsFinished = true;

    bool detection;

    public GameObject AI;

    public GameObject item;
    // Start is called before the first frame update
    void Start()
    {

        
        Detection.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {

        if (detection) // boolean

        {

            if (coroutineIsFinished)

                StartCoroutine("ActiveAndDeactivate");

            else

            {

                //If the coroutine is still going on we stop it

                StopCoroutine("ActiveAndDeactivate");

                //Then restart it

                StartCoroutine("ActiveAndDeactivate");

            }


        }
    }

    IEnumerator ActiveAndDeactivate()

    {

        coroutineIsFinished = false;



        Detection.SetActive(true);



        yield return new WaitForSeconds(5.0f); //will wait 5seconds before continuing

        Detection.SetActive(false);



        coroutineIsFinished = true;

    }

    void OnTriggerEnter(Collider other)
    {
        

        if (other.gameObject.tag == "Enemy")
        {
            AI.GetComponent<AI_test>().SetTarget(item.transform.position);
        }

    }



    void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "RightHand")
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Stop();
        }

    }

}


