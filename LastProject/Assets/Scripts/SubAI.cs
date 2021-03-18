using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubAI : MonoBehaviour
{
    public enum characterState { idle, trace, attack }
    public characterState currentState = characterState.idle;

    private NavMeshAgent navMeshAgent;
    //private Transform 
    private float traceDistance = 10.0f;
    private float attackDistance = 5.0f;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void CheckEnemy()
    {
        
    }
}
