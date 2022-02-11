using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_patrol : MonoBehaviour
{
    public float speed;
    public float startWaitTime;
    public Transform[] moveSpots;

    private int randomSpot;
    private float waitTime;





    private void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
        transform.LookAt(moveSpots[randomSpot]);




    }

    private void Update()
    {



        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);

        // Check if the distance between this and the movepoint is less than 0.2f (tollerange range)
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (waitTime <= 0)
            {
                randomSpot = Random.Range(0, moveSpots.Length);
                transform.LookAt(moveSpots[randomSpot]);
                waitTime = startWaitTime;
            }
            else
            {
                waitTime = -Time.deltaTime;
            }
        }
    }
}
