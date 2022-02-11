using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class distest : MonoBehaviour
{
    
    public GameObject AI;

    public GameObject _item;
    public AudioClip _itemAudio;

    private NavMeshAgent _navAgent;

    private Animator _anim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitinfo))
            {
                if (!(AI is null))
                    AI.transform.GetComponent<AI_test>().GetDistracted(hitinfo.point);
            }
        }
        
    }

    private void ThrowItem(Vector3 pos)
    {
        Instantiate(_item, pos, Quaternion.identity);
        AudioSource.PlayClipAtPoint(_itemAudio, pos);
    }

    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        if (_navAgent is null)
            Debug.LogError("NavMeshAgent is NULL");

        _anim = GetComponentInChildren<Animator>();
        if (_anim is null)
            Debug.LogError("Animator in children is NULL");
    }

   
}
