using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SubCharacter : MonoBehaviour
{
    [SerializeField] GameObject MainCharacter = null;

    NavMeshAgent nav;
    Animator anim;
    public Transform target;
    public float distance = 5.0f;
    float distanceWithPlayer;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        distanceWithPlayer = Vector3.Distance(MainCharacter.transform.position, transform.position);
    }

    void LateUpdate()
    {
       Follow();
    }

    void Follow()
    {
        //메인 플레이어와의 거리가 범위를 넘어가는 경우
        if (distanceWithPlayer > distance)
        {
            nav.SetDestination(MainCharacter.transform.position);
            anim.SetBool("isRun", true);
        }
        else
        {
            nav.SetDestination(transform.position);
            anim.SetBool("isRun", false);
        }
    }
}