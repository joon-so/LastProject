using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ServerSubAIManager : MonoBehaviour
{
    public enum characterState { idle, trace, attack }
    public characterState currentState = characterState.idle;

    public NavMeshAgent nav;
    public Rigidbody rigidbody;
    public GameObject tagCharacter;

    protected List<GameObject> targets;
    protected GameObject target = null;
    protected float distance;
    //private Transform 
    public float traceDistance = 5.0f;
    public float attackDistance = 6.0f;
    protected float spinSpeed = 1000f;

    public void FindPlayers()
    {
        targets = GameObject.Find("OtherPlayers").GetComponent<ServerOtherPlayersList>().OtherPlayers;
    }

    public void MainCharacterTrace(Vector3 movePos)
    {
        currentState = characterState.trace;
        if (distance > traceDistance)
        {
            //nav.SetDestination(movePos);
            StartCoroutine(SetDes(movePos));
        }
        else
        {
            currentState = characterState.idle;
        }
        target = null;
    }

    public void SubAttack()
    {
        if (distance <= traceDistance)
        {
            //주변에 적존재한다면 공격, 아니면 대기
            //FindEnemy();
        }
        else
        {
            target = null;
            currentState = characterState.trace;
        }
    }

    public void Idle()
    {
        FindPlayer();
        if (distance <= traceDistance)
        {
            //주변에 적존재한다면 공격, 아니면 대기
            if (target != null)
            {
                currentState = characterState.attack;
            }
            else if (target == null)
            {
                target = null;
            }
        }
        else
        {
            currentState = characterState.trace;
            target = null;
        }
        StartCoroutine(SetDes(transform.position));
//        nav.SetDestination(transform.position);
    }

    public void FindPlayer()
    {
        target = null;
        float targetDistance = attackDistance;
        for (int i = 0; i < targets.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, targets[i].transform.position);
            if (targetDistance > distance)
            {
                targetDistance = distance;
                target = targets[i];
            }
        }
    }

    IEnumerator SetDes(Vector3 target)
    {
        nav.SetDestination(target);
        yield return null;
    }
}
