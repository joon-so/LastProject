using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMyEva : SubAI
{
    [SerializeField] GameObject qSkill;
    [SerializeField] GameObject wSkillEffect;
    [SerializeField] GameObject wSkillShockEffect;
    public Transform wSkillPos = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float followDistance = 5.0f;
    public float attackDelay = 1.0f;

    public static float qSkillCoolTime = 10.0f;
    public static float wSkillCoolTime = 5.0f;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;
    float curAttackDelay;

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
    }
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

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

        qSkill.SetActive(false);
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
            //    SubAttack();
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

        ServerLoginManager.playerList[0].mainCharacterBehavior = 1;  
        if (vecTarget == transform.position)
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0; 
    }
    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            vecTarget = transform.position;
            myAnimator.SetBool("Run", false);
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
            myAnimator.SetTrigger("Dodge");
            ServerLoginManager.playerList[0].mainCharacterBehavior = 2; 
            StartCoroutine(DodgeDelay());

        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            transform.Translate(Vector3.forward * 2 * Time.deltaTime);
            vecTarget = transform.position;
            myAnimator.SetBool("Run", false);
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;

            if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
                && myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
            {
                moveSpeed = 100f;
            }
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
                ServerLoginManager.playerList[0].mainCharacterBehavior = 3;

                curAttackDelay = 0;

                StartCoroutine(AttackDelay());
            }
        }
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

    void Q_Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && onQSkill)
        {
            onQSkill = false;
            curQSkillCoolTime = 0;
            myAnimator.SetBool("Run", false);

            canAttack = false;
            canDodge = false;
            canSkill = false;

            StartCoroutine(FireGun());
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

            StartCoroutine(ShockWave());
        }
    }

    void Tag()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            vecTarget = transform.position;
        }
    }

    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
        canMove = true;
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1.5f);
        canMove = true;
        canDodge = true;
    }

    IEnumerator StartMotion()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetTrigger("StartMotion");
        yield return new WaitForSeconds(1.8f);
        canMove = true;
        canAttack = true;
        canDodge = true;
    }

    IEnumerator FireGun()
    {
        qSkill.SetActive(true);

        ServerLoginManager.playerList[0].mainCharacterBehavior = 4;

        myAnimator.SetTrigger("QSkill");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }

        yield return new WaitForSeconds(5.0f);
        qSkill.SetActive(false);

        vecTarget = transform.position;
        myAnimator.SetBool("Run", false);
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;

        curQSkillCoolTime = 0.0f;

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }

    IEnumerator ShockWave()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }

        ServerLoginManager.playerList[0].mainCharacterBehavior = 5;

        myAnimator.SetTrigger("WSkill");
        Instantiate(wSkillEffect, wSkillPos.position, wSkillPos.rotation);
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetFloat("Speed", 1.0f);


        yield return new WaitForSeconds(0.3f);
        // »ý¼º
        wSkillShockEffect.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 1.5f, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        wSkillShockEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 3f + transform.right, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 3f + -transform.right, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        wSkillShockEffect.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 5f + transform.right * 2.0f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 5f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 5f + -transform.right * 2.0f, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        wSkillShockEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 7f - transform.right * 3f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 7f - transform.right * 1.2f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 7f + transform.right * 1.2f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 7f + transform.right * 3f, transform.rotation);


        yield return new WaitForSeconds(0.5f);
        myAnimator.SetFloat("Speed", 1.0f);

        vecTarget = transform.position;

        curWSkillCoolTime = 0.0f;

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }
}