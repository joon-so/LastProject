using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField]
    GameObject attackRange = default;

    public float moveSpeed = 5.0f;
    Vector3 vecTarget;

    void Start()
    {
        vecTarget = transform.position;
    }

    void Update()
    {
        Move();
        Stop();
        AttackRange();
    }

    void Move()
    {
        if (Input.GetMouseButton(1))
        {
            moveSpeed = 5.0f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                vecTarget = hit.point;
                vecTarget.y = transform.position.y;

                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, vecTarget, moveSpeed * Time.deltaTime);
    }

    void AttackRange()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            attackRange.transform.position = transform.position;
            attackRange.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            attackRange.SetActive(false);
        }
    }

    void Stop()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            moveSpeed = 0f;
        }
    }
}