using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubAI : MonoBehaviour
{
    public enum characterState { idle, trace, attack }
    public characterState currentState;

    NavMeshAgent navMeshAgent;
    Rigidbody rigidbody;
    GameObject mainCharacter;
    private float distance;
    //private Transform 
    private float traceDistance = 10.0f;
    private float attackDistance = 5.0f;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();

        currentState = characterState.idle;
        traceDistance = 10.0f;
        attackDistance = 5.0f;
    }

    void Update()
    {
        mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter");
        distance = Vector3.Distance(mainCharacter.transform.position, transform.position);

        if(currentState == characterState.trace)
        {
            MainCharacterTrace();
        }
        else if (currentState == characterState.attack)
        {
            Attack();
        }
        else
        {
            Idle();
        }
    }

    void MainCharacterTrace()
    {

    }

    void Attack()
    {

    }

    void Idle()
    {

    }

    void CheckEnemy()
    {
        
    }
}
