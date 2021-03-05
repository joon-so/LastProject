using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] [Range(10f, 20f)] float detectDistance;
    [SerializeField] [Range(5f, 10f)] float shootDistance = 10f;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;
    Animator anim;
    Vector3 startPoint;

    [SerializeField] float angle = 0f;
    [SerializeField] float distance = 0f;
    [SerializeField] LayerMask layerMask = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        InvokeRepeating("Find", 0f, 0.5f);
        startPoint = transform.position;
    }

    void Update()
    {
        Find();
    }

    void Sight()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, layerMask);

        if (colliders.Length > 0)
        {
            Transform player = colliders[0].transform;

            Vector3 direction = (player.position - transform.position).normalized;
        }
    }

    void Find()
    {
        mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter");

        if (mainCharacter == null)
        {
            return;
        }
        else
        {
            playerDistance = Vector3.Distance(mainCharacter.transform.position, transform.position);

            if(playerDistance < shootDistance)
            {
                nav.SetDestination(transform.position);
                anim.SetBool("isAttack", true);
            }
            else if (playerDistance < detectDistance)
            {
                nav.SetDestination(mainCharacter.transform.position);
                anim.SetBool("isAttack", false);
                anim.SetBool("isRun", true);
            }
            else
            {
                if(Vector3.Distance(startPoint, transform.position) < 0.1f)
                {
                    anim.SetBool("isRun", false);
                }
                else
                {
                 anim.SetBool("isRun", true);
                }
                nav.SetDestination(startPoint);
            }
        }
    }
}