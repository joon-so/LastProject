using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] GameObject attackRange = default;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;

    Vector3 vecTarget;

    bool onDodge;
    float curDodgeCoolTime = 0;

    Animator anim;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        vecTarget = transform.position;

        curDodgeCoolTime = dodgeCoolTime;
        onDodge = true;
    }

    void Update()
    {
        Move();
        Dodge();
        Stop();
        Attack();
        AttackRange();
        CoolTime();
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
        
        anim.SetBool("isRun", vecTarget != transform.position);
    }

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && curDodgeCoolTime >= dodgeCoolTime && onDodge)
        {
            curDodgeCoolTime = 0.0f;

            moveSpeed = 10.0f;
            anim.SetTrigger("doDodge");

            onDodge = false;

            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        moveSpeed = 5.0f;
    }

    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveSpeed = 0f;
            anim.SetBool("isRun", false);
            vecTarget = transform.position;
        }
    }

    void Attack()
    {

    }

    void AttackRange()
    {
        attackRange.transform.position = transform.position;
        if (Input.GetKeyDown(KeyCode.A))
        {
            attackRange.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            attackRange.SetActive(false);
        }
    }

    void CoolTime()
    {
        if (curDodgeCoolTime < dodgeCoolTime)
        {
            curDodgeCoolTime += Time.deltaTime;
        }
        else
        {
            onDodge = true;
        }
    }
}