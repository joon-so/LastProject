using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMyKarmen : SubAI
{
    [SerializeField] GameObject leftStaffEffect;
    [SerializeField] GameObject rightStaffEffect;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float followDistance = 5.0f;
    public float attackDelay = 1.0f;

    float curDodgeCoolTime;
    float curAttackDelay;

    bool canMove;
    bool canDodge;
    bool canAttack;

    bool onDodge;

    Vector3 vecTarget;
    Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    void Start()
    {
        //if (gameObject.transform.tag == "MainCharacter")
        //{
        //    tagCharacter = ServerIngamePlayerManager.instance.character2;
        //}
        //else if (gameObject.transform.tag == "SubCharacter")
        //{
        //    tagCharacter = ServerIngamePlayerManager.instance.character1;
        //}
        rigidbody = GetComponent<Rigidbody>();

        vecTarget = transform.position;
        curDodgeCoolTime = dodgeCoolTime;

        canMove = false;
        canDodge = false;
        canAttack = false;
        onDodge = true;

        curAttackDelay = attackDelay;

        StartCoroutine(StartMotion());
    }

    void Update()
    {
        if (gameObject.transform.tag == "MainCharacter")
        {
            curAttackDelay += Time.deltaTime;
            if (canMove)
                Move();
            if (canAttack)
                Attack();
            if (canDodge)
                Dodge();
            Stop();
            CoolTime();
        }
        else if (gameObject.transform.tag == "SubCharacter")
        {
            //distance = Vector3.Distance(tagCharacter.transform.position, transform.position);

            //if (currentState == characterState.trace)
            //{
            //    MainCharacterTrace();
            //}
            //else if (currentState == characterState.attack)
            //{
            //   // SubAttack();
            //}
            //else if (currentState == characterState.idle)
            //{
            //    Idle();
            //}
        }
        Tag();
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
        myAnimator.SetBool("Run", vecTarget != transform.position);

        ServerLoginManager.playerList[0].mainCharacterBehavior = 1; // Run
        if (vecTarget == transform.position)
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0; // Idle
    }
    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            vecTarget = transform.position;
            myAnimator.SetBool("Run", false);
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0; // Idle
        }
    }
    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onDodge)
        {
            onDodge = false;
            
            canAttack = false;
            canMove = false;

            curDodgeCoolTime = 0.0f;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
            moveSpeed *= 2;
            myAnimator.SetTrigger("Dodge");
            ServerLoginManager.playerList[0].mainCharacterBehavior = 2; // Dodge

            StartCoroutine(DodgeDelay());
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("salto2SS"))
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            vecTarget = transform.position;
            myAnimator.SetBool("Run", false);

        }
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (curAttackDelay > attackDelay)
            {
                canMove = false;
                canDodge = false;
                myAnimator.SetBool("Run", false);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 nextVec = hit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }

                vecTarget = transform.position;
                myAnimator.SetTrigger("Attack");
                ServerLoginManager.playerList[0].mainCharacterBehavior = 3; // Attack

                curAttackDelay = 0;

                StartCoroutine(AttackDelay());
            }
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
    void Tag()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            vecTarget = transform.position;
        }
    }
    IEnumerator StartMotion()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetTrigger("StartMotion");
        yield return new WaitForSeconds(1.5f);
        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);
        canMove = true;
        canAttack = true;
        canDodge = true;
    }
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.8f);
        canMove = true;
        canDodge = true;
    }
    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
        canMove = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0; // Idle
    }
}
