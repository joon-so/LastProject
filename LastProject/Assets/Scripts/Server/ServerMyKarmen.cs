using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ServerMyKarmen : ServerSubAIManager
{
    [SerializeField] GameObject leftStaffEffect;
    [SerializeField] GameObject rightStaffEffect;

    [SerializeField] GameObject qSkill;
    public Transform qSkillPos;

    [SerializeField] GameObject wLeftEffect = null;
    [SerializeField] GameObject wRightEffect = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float followDistance = 5.0f;
    public float attackDelay = 0.5f;

    public static float qSkillCoolTime = 5.0f;
    public static float wSkillCoolTime = 5.0f;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;

    float curAttackDelay;
    float subAttackDelay = 1.5f;

    bool canMove;
    bool canDodge;
    bool canAttack;
    bool canSkill;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;

    Vector3 vecTarget;
    Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {

        vecTarget = transform.position;
        curDodgeCoolTime = dodgeCoolTime;
        curQSkillCoolTime = qSkillCoolTime;
        curWSkillCoolTime = wSkillCoolTime;

        canMove = false;
        canDodge = false;
        canAttack = false;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;

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
            if (canSkill)
            {
                Q_Skill();
                W_Skill();
            }
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
            canSkill = false;

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
                canSkill = false;

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
        if (Input.GetMouseButtonUp(0))
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }
    void CoolTime()
    {
        if (curDodgeCoolTime < dodgeCoolTime)
            curDodgeCoolTime += Time.deltaTime;
        else
            onDodge = true;
        if (curQSkillCoolTime < qSkillCoolTime)
            curQSkillCoolTime += Time.deltaTime;
        else
            onQSkill = true;
        if (curWSkillCoolTime < wSkillCoolTime)
            curWSkillCoolTime += Time.deltaTime;
        else
            onWSkill = true;
    }
    void Tag()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            vecTarget = transform.position;
        }
    }
    void Q_Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && onQSkill)
        {
            onQSkill = false;
            curQSkillCoolTime = 0;
            myAnimator.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            StartCoroutine(BigAttack());
            ServerLoginManager.playerList[0].mainCharacterBehavior = 4; // WSkill
        }
    }

    void W_Skill()
    {
        if (Input.GetKeyDown(KeyCode.W) && onWSkill)
        {
            onWSkill = false;
            curWSkillCoolTime = 0;
            myAnimator.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            StartCoroutine(StraightAttack());
            ServerLoginManager.playerList[0].mainCharacterBehavior = 5; // WSkill
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
        canSkill = true;
    }
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.6f);
        canMove = true;
        canDodge = true;
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0; // Idle
    }
    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
        canMove = true;
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0; // Idle
    }
    IEnumerator BigAttack()
    {
        curQSkillCoolTime = 0.0f;

        leftStaffEffect.SetActive(false);
        rightStaffEffect.SetActive(false);

        myAnimator.SetTrigger("QSkill");
        myAnimator.SetFloat("Speed", 0.2f);
        yield return new WaitForSeconds(0.5f);
        Instantiate(qSkill, qSkillPos.position, qSkillPos.rotation);
        myAnimator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(1.0f);
        myAnimator.SetFloat("Speed", 1.0f);
        yield return new WaitForSeconds(1.0f);

        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        vecTarget = transform.position;
        myAnimator.SetBool("Run", false);

        ServerLoginManager.playerList[0].mainCharacterBehavior = 0; // Idle

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }

    IEnumerator StraightAttack()
    {
        curWSkillCoolTime = 0.0f;

        leftStaffEffect.SetActive(false);
        rightStaffEffect.SetActive(false);

        myAnimator.SetTrigger("WSkill");
        wLeftEffect.SetActive(true);
        wRightEffect.SetActive(true);
        yield return new WaitForSeconds(2.8f);

        wLeftEffect.SetActive(false);
        wRightEffect.SetActive(false);

        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        vecTarget = transform.position;
        myAnimator.SetBool("Run", false);

        ServerLoginManager.playerList[0].mainCharacterBehavior = 0; // Idle

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }
}
