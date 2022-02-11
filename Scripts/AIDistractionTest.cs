using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class AIDistractionTest : MonoBehaviour
{

    private bool _distracted;

    public NavMeshAgent _navAgent;

    public Animator _anim;

    // Start is called before the first frame update
    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (_distracted)
            return;
    }

    private void GetDistracted(Vector3 pos)
    {
        if (Vector3.Distance(transform.position, pos) <= 15f)
        {
            StopAllCoroutines();
            _distracted = true;
            _navAgent.SetDestination(pos);
            _anim.SetBool("Walk", true);
            StartCoroutine(FollowDistraction(pos));
        }
    }

    private IEnumerator FollowDistraction(Vector3 pos)
    {
        while (Vector3.Distance(transform.position, pos) > 2f)
            yield return null;

        _anim.SetBool("Walk", false);
        _navAgent.SetDestination(transform.position);
    }
}
