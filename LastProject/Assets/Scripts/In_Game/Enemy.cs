using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] [Range(3f, 8f)] float detectDistance;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;

    [SerializeField] float angle = 0f;
    [SerializeField] float distance = 0f;
    [SerializeField] LayerMask layerMask = 0;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        InvokeRepeating("Find", 0f, 0.5f);
    }

    void Update()
    {
        Find();
    }

    void Sight()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, layerMask);

        if (colliders.Length >0 )
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
            Debug.Log(playerDistance);

            if(playerDistance < detectDistance)
            {
                Debug.Log("°¨Áö");
                nav.SetDestination(mainCharacter.transform.position);
            }
            else
            {
                nav.SetDestination(transform.position);
            }
        }
    }
}