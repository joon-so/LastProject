using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] [Range(3f, 8f)] float detectDistance;

    float playerDistance;
    GameObject mainCharacter;

    [SerializeField] float angle = 0f;
    [SerializeField] float distance = 0f;
    [SerializeField] LayerMask layerMask = 0;

    void Start()
    {
                
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
        }
    }
}