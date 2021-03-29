using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Karmen : MonoBehaviour
{
    [SerializeField] GameObject attackRange = default;

    public float moveSpeed = 30.0f;
    public float dodgeCoolTime = 5.0f;
    bool doAttack = false;
    bool motionEndCheck = true;
    bool comboContinue = true;
    bool isRun = false;
    //float comboDelay = 0f;

    Vector3 vecTarget;

    Vector3 moveVec;

    bool onDodge;
    bool onFire;

    float curDodgeCoolTime = 0;
    float fireDelay;

    Animator anim;
    Weapon weapon;

    float distanceWithPlayer;
    public float followDistance = 20.0f;
    NavMeshAgent nav;
    GameObject tagCharacter;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();

        if (gameObject.tag == "MainCharacter")
            tagCharacter = GameObject.FindWithTag("SubCharacter");
        else
            tagCharacter = GameObject.FindWithTag("MainCharacter");

        vecTarget = transform.position;

        curDodgeCoolTime = dodgeCoolTime;
        onDodge = true;
    }

    void Update()
    { 
        if (gameObject.transform.tag == "MainCharacter")
        {
            //계산량 많을듯 -> 애니메이션 이벤트로 변경
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("salto2SS"))
            {
                Move();
                Stop();
                Attack();
            }
            Dodge();
            AttackRange();
            CoolTime();
        }
        else
        {
           // Follow();
        }
    }

    void Move()
    {
        if (Input.GetMouseButton(1))
        {
            moveSpeed = 30.0f;
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

        isRun = vecTarget != transform.position;
        anim.SetBool("isRun", isRun);

        if(doAttack)
        {
            isRun = false;
            anim.SetBool("isRun", isRun);
            vecTarget = transform.position;
        }
    }

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && curDodgeCoolTime >= dodgeCoolTime && onDodge)
        {
            curDodgeCoolTime = 0.0f;

            anim.SetTrigger("doDodge");

            onDodge = false;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("salto2SS"))
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
                && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.6f)
            {
                moveSpeed = 18.0f;
            }
            else
            {
                moveSpeed = 60.0f;
            }

            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            vecTarget = transform.position;
        }
    }

    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveSpeed = 0f;
            isRun = false;
            anim.SetBool("isRun", isRun);
            vecTarget = transform.position;
        }
    }

    void Attack()
    {
        if (doAttack)
        {
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f 
                && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
            {
                if (Input.GetMouseButtonDown(0))
                    if (comboContinue)
                        comboContinue = false;
                motionEndCheck = false;
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && !motionEndCheck)
            {
                if (!comboContinue)
                {
                    anim.SetTrigger("nextCombo");
                    comboContinue = true;
                }
                else if (comboContinue)
                {
                    doAttack = false;
                    anim.SetBool("doAttack", doAttack);

                }
                motionEndCheck = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            isRun = false;
            anim.SetBool("isRun", isRun);

            if ((doAttack && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
                 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle1SS")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("runSS"))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 nextVec = hit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }
                vecTarget = transform.position;
            }

            moveSpeed = 0f;
            doAttack = true;
            anim.SetBool("doAttack", doAttack);
        }

        if (doAttack && Input.GetMouseButtonDown(1))
        {
            doAttack = false;
            anim.SetBool("doAttack", doAttack);
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
        distanceWithPlayer = Vector3.Distance(tagCharacter.transform.position, transform.position);

        if (distanceWithPlayer > followDistance)
        {
            nav.SetDestination(tagCharacter.transform.position);
            isRun = true;
            anim.SetBool("isRun", isRun);
        }
        else
        {
            isRun = false;
            nav.SetDestination(transform.position);
            anim.SetBool("isRun", isRun);
        }
    }
} 
