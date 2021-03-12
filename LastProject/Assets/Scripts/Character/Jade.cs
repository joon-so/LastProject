using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jade : MonoBehaviour
{
    [SerializeField] GameObject attackRange = default;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float fireCoolTime = 0.5f;

    Vector3 vecTarget;

    public Transform RbulletPos;
    public Transform LbulletPos;
    public GameObject bullet;
    Vector3 moveVec;

    bool onDodge;
    bool onFire;

    float curDodgeCoolTime = 0;
    float fireDelay;

    Animator anim;
    Weapon weapon;

    float distanceWithPlayer;
    public float followDistance = 5.0f;
    NavMeshAgent nav;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();

        vecTarget = transform.position;

        curDodgeCoolTime = dodgeCoolTime;
        onDodge = true;
    }

    void Update()
    {
        if (gameObject.transform.parent.tag == "MainCharacter")
        {
            //계산량 많을듯 -> 애니메이션 이벤트로 변경
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("DodgeForward"))
                Move();
            Dodge();
            Stop();
            Attack();
            AttackRange();
            CoolTime();
        }
        else
        {
            Follow();
        }
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
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("DodgeForward"))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            vecTarget = transform.position;
        }
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
        fireDelay += Time.deltaTime;
        //onFire = weapon.shotSpeed < fireDelay;

        if (Input.GetMouseButtonDown(0))
        {
            if (fireDelay > fireCoolTime)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 nextVec = hit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }

                GameObject instantBullet = Instantiate(bullet, RbulletPos.position, RbulletPos.rotation);
                Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = RbulletPos.forward * 50;

                instantBullet = Instantiate(bullet, LbulletPos.position, LbulletPos.rotation);
                bulletRigid = instantBullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = LbulletPos.forward * 50;


                moveSpeed = 0f;
                anim.SetBool("isRun", false);
                vecTarget = transform.position;

                anim.SetTrigger("doShot");
                fireDelay = 0;
            }
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

    void Follow()
    {
        //distanceWithPlayer = Vector3.Distance(MainCharacter.transform.position, transform.position);

        ////메인 플레이어와의 거리가 범위를 넘어가는 경우
        //if (distanceWithPlayer > followDistance)
        //{
        //    nav.SetDestination(MainCharacter.transform.position);
        //    anim.SetBool("isRun", true);
        //}
        //else
        //{
        //    nav.SetDestination(transform.position);
        //    anim.SetBool("isRun", false);
        //}
    }
}
