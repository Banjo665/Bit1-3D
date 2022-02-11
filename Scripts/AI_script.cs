using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_script : MonoBehaviour
{

    
    public GameObject player;
    Vector3 targetPosition;
    NavMeshAgent navmesh;
    int layerMask = ~((1 << 7) + (1 << 10)); // not player and AI

    public GameObject pathObject;
    List<Vector3> patrolPath = new List<Vector3>();
    int nextPosIndex = 0;
    bool distracted = false;
    bool targeted = false;

    // Start is called before the first frame update
    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        targetPosition = transform.position;
        foreach(Transform child in pathObject.transform)
        {
            patrolPath.Add(child.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!Physics.Linecast(transform.position, player.transform.position, layerMask)){
            targetPosition = player.transform.position;
            distracted = true;
            targeted = true;
        }
        else{
            if(Vector3.Distance(targetPosition, transform.position) < 0.5f) // first go to target
                targeted = false;
            if(!targeted) {
                if(distracted){
                    distracted = false;
                    nextPosIndex = FindNearestPosition(patrolPath, transform.position);
                }
                if(Vector3.Distance(patrolPath[nextPosIndex], transform.position) < 0.5f){
                    if(nextPosIndex == patrolPath.Count - 1){
                        nextPosIndex = 0;
                    }else{
                        nextPosIndex++;
                    }
                }
                targetPosition = patrolPath[nextPosIndex];
            }
        }
        navmesh.destination = targetPosition;
    }

    int FindNearestPosition(List<Vector3> positionArray, Vector3 position){
        int indexWithShortestDistance = 0;
        float shortestDistance = Vector3.Distance(positionArray[0], position);
        for(int i = 0; i < positionArray.Count; i++){
            float distance = Vector3.Distance(positionArray[i], position);
            if(distance < shortestDistance){
                indexWithShortestDistance = i;
                shortestDistance = distance;
            }
        }
        return indexWithShortestDistance;
    }

    public void SetTarget(Vector3 position){
        targetPosition = position;
        targeted = true;
    }
}
