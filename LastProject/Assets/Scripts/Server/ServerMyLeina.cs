using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMyLeina : SubAI
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPos;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float fireDelay = 1.0f;
    public float subFireDelay = 1.5f;
    public float followDistance = 5.0f;

    public static int attackDamage = 20;

    float curDodgeCoolTime;

    float curFireDelay;

    bool canAttack;
    bool canDodge;
    bool canMove;

    bool onDodge;

    Vector3 vecTarget;
    Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        vecTarget = transform.position;
        curDodgeCoolTime = dodgeCoolTime;

        canMove = true;
        canDodge = true;
        canAttack = true;

        onDodge = true;

        curFireDelay = fireDelay;
    }
    void Update()
    {
        curFireDelay += Time.deltaTime;
        if (gameObject.transform.tag == "MainCharacter")
        {
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
            //    anim.SetBool("Run", true);
            //    curFireDelay = 1f;
            //}
            //else if (currentState == characterState.attack)
            //{
            //    SubAttack();

            //    if (target)
            //    {
            //        Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            //        Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            //        transform.rotation = Quaternion.Euler(0, euler.y, 0);
            //    }
            //    if (curFireDelay > subFireDelay && target != null)
            //    {
            //        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
            //        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
            //        arrowRigid.velocity = arrowPos.forward;

            //        moveSpeed = 0f;
            //        anim.SetBool("Run", false);
            //        vecTarget = transform.position;

            //        anim.SetTrigger("Attack");
            //        curFireDelay = 0;

            //        StartCoroutine(AttackDelay());
            //    }
            //}
            //else if (currentState == characterState.idle)
            //{
            //    Idle();
            //    anim.SetBool("Run", false);
            //    curFireDelay = 1f;
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

        ServerLoginManager.playerList[0].mainCharacterBehavior = 1;
        if (vecTarget == transform.position)
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }
    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            myAnimator.SetBool("Run", false);
            vecTarget = transform.position;
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
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
            ServerLoginManager.playerList[0].mainCharacterBehavior = 2;

            StartCoroutine(DodgeDelay());
        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
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
            canMove = false;
            canDodge = false;

            if (curFireDelay > fireDelay)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 nextVec = hit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }

                GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
                Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
                arrowRigid.velocity = arrowPos.forward;

                myAnimator.SetBool("Run", false);
                vecTarget = transform.position;

                myAnimator.SetTrigger("Attack");
                ServerLoginManager.playerList[0].mainCharacterBehavior = 3;
                curFireDelay = 0;

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
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canDodge = true;
    }
    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
        canMove = true;
    }
}
