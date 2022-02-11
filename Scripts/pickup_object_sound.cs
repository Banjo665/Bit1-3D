using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickup_object_sound : MonoBehaviour
{
    public AudioClip picked;
    public AudioClip impact;
    public AudioClip inv;
    public KeyCode pickupKey = KeyCode.F;

    AudioSource audiosource;


   
   

    

    

    // Start is called before the first frame update
    void Start()
    {

        audiosource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    private void Update()
    {

        
    }

       

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag != "Player")
                if (other.gameObject.tag != "RightHand")
                {
                    audiosource.PlayOneShot(impact);
                


            }

       

            if (other.gameObject.tag == "RightHand")
            {
                audiosource.PlayOneShot(picked);
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
