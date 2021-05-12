using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubAI : MonoBehaviour
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
    public float traceDistance = 2.0f;
    public float attackDistance = 1.0f;

    public void FindEnemys()
    {
        targets = GameObject.Find("Enemys").GetComponent<EnemyList>().Enemys;
    }

    public void MainCharacterTrace()
    {
        currentState = characterState.trace;
        if (distance > traceDistance)
        {
            nav.SetDestination(tagCharacter.transform.position);
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
            //�ֺ��� �������Ѵٸ� ����, �ƴϸ� ���
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
        if (distance <= traceDistance)
        {
            //�ֺ��� �������Ѵٸ� ����, �ƴϸ� ���
            if (FindEnemy())
            {
                currentState = characterState.attack;
            }
            else
            {
                target = null;
            }
        }
        else
        {
            currentState = characterState.trace;
            target = null;
        }
        nav.SetDestination(transform.position);
    }

    bool FindEnemy()
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
        if (target != null)
            return true;
        return false;
    }
}
