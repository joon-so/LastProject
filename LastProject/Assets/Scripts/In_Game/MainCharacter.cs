using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    [SerializeField] GameObject attackRange = default;

    private Rigidbody rigid;

    public float moveSpeed = 5.0f;
    Vector3 vecTarget;

    public float jumpPower = 3.0f;
    bool isJump;

    void Start()
    {
        vecTarget = transform.position;
        rigid = GetComponent<Rigidbody>();
        isJump = false;
    }

    void Update()
    {
        Move();
        Jump();
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

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isJump)
            {
                isJump = true;
                rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            }
            else
                return;
        }
    }

    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveSpeed = 0f;
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isJump = false;
        }
    }
}