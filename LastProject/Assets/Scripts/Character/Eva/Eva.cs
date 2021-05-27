using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Eva : SubAI
{
    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject qSkill = null;
    [SerializeField] GameObject wSkillEffect = null;
    [SerializeField] GameObject wSkillShockEffect = null;
    public Transform wSkillPos = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float followDistance = 5.0f;

    public static float qSkillCoolTime = 10.0f;
    public static float wSkillCoolTime = 5.0f;
    public static float eSkillCoolTime = 5.0f;
    public static int attackDamage = 20;
    public static int qSkillDamage = 20;
    public static int wSkillDamage = 60;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;

    bool canMove;
    bool canDodge;
    bool canAttack;
    bool canSkill;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;

    bool doingAttack;
    bool motionEndCheck;
    bool comboContinue;
    
    float distanceWithPlayer;

    Vector3 vecTarget;

    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        if (GameManager.instance.isMainEva)
        {
            nav.enabled = false;
            tagCharacter = GameManager.instance.character2;
        }
        else if (GameManager.instance.isSubEva)
        {
            tagCharacter = GameManager.instance.character1;
            nav.enabled = true;
        }

        FindEnemys();

        nav = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();

        vecTarget = transform.position;

        curDodgeCoolTime = 0;
        curQSkillCoolTime = 0;
        curWSkillCoolTime = 0;

        canMove = true;
        canDodge = true;
        canAttack = true;
        canSkill = true;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;

        doingAttack = false;
        motionEndCheck = true;
        comboContinue = true;

        qSkill.SetActive(false);
    }
    void Update()
    {
        if (gameObject.transform.tag == "MainCharacter")
        {
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
                E_Skill();
            }
            Stop();
            AttackRange();
            CoolTime();
        }
        else if (gameObject.transform.tag == "SubCharacter")
        {
            distance = Vector3.Distance(tagCharacter.transform.position, transform.position);

            if (currentState == characterState.trace)
            {
                MainCharacterTrace();
            }
            else if (currentState == characterState.attack)
            {
                SubAttack();
            }
            else if (currentState == characterState.idle)
            {
                Idle();
            }
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
        anim.SetBool("Run", vecTarget != transform.position);
    }
    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveSpeed = 0f;
            anim.SetBool("Run", false);
            vecTarget = transform.position;
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

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
           // moveSpeed *= 2;
            anim.SetTrigger("Dodge");
            StartCoroutine(DodgeDelay());
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("02_Jump"))
        {
            transform.Translate(Vector3.forward * 2 * Time.deltaTime);
            vecTarget = transform.position;
            anim.SetBool("Run", false);
            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
                && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.4f)
            {
                moveSpeed = 100f;
                Debug.Log(moveSpeed);
            }
            //{
            //    moveSpeed = 1.0f;
            //}
            //else
            //{
            //    moveSpeed = 2.0f;
            //}
        }
    }

    void Attack()
    {
        if (doingAttack)
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
                    doingAttack = false;
                    anim.SetBool("Attack", doingAttack);

                }
                motionEndCheck = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            canMove = false;
            anim.SetBool("Run", canMove);

            if ((doingAttack && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
                 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_01")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
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
            doingAttack = true;
            anim.SetBool("Attack", doingAttack);
        }

        if (doingAttack && Input.GetMouseButtonDown(1))
        {
            doingAttack = false;
            anim.SetBool("Attack", doingAttack);
            canMove = true;
            anim.SetBool("Run", canMove);
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

    void Q_Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && onQSkill)
        {
            onQSkill = false;
            curQSkillCoolTime = 0;
            anim.SetBool("Run", false);

            canAttack = false;
            //canMove = false;
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
            anim.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            StartCoroutine(ShockWave());
        }
    }
    void E_Skill()
    {

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
        if (curQSkillCoolTime < qSkillCoolTime)
        {
            curQSkillCoolTime += Time.deltaTime;
        }
        else
        {
            onQSkill = true;
        }
        if (curWSkillCoolTime < wSkillCoolTime)
        {
            curWSkillCoolTime += Time.deltaTime;
        }
        else
        {
            onWSkill = true;
        }
    }

    void Follow()
    {
        //distanceWithPlayer = Vector3.Distance(tagCharacter.transform.position, transform.position);

        //if (distanceWithPlayer > followDistance)
        //{
        //    nav.SetDestination(tagCharacter.transform.position);
        //    isRun = true;
        //    anim.SetBool("isRun", isRun);
        //}
        //else
        //{
        //    isRun = false;
        //    nav.SetDestination(transform.position);
        //    anim.SetBool("isRun", isRun);
        //}
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
        canSkill = true;
    }

    IEnumerator FireGun()
    {
        curQSkillCoolTime = 0.0f;
        qSkill.SetActive(true);
        anim.SetTrigger("QSkill");
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
        anim.SetBool("Run", false);

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }

    IEnumerator ShockWave()
    {
        curWSkillCoolTime = 0.0f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }

        anim.SetTrigger("WSkill");
        Instantiate(wSkillEffect, wSkillPos.position, wSkillPos.rotation);
        yield return new WaitForSeconds(0.5f);
        anim.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(0.5f);
        anim.SetFloat("Speed", 1.0f);


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
        anim.SetFloat("Speed", 1.0f);


        vecTarget = transform.position;

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy1Attack")
        {
            if (GameManager.instance.mainPlayerHp > 0)
                GameManager.instance.mainPlayerHp -= Enemy1.damage;
        }
        if (collision.gameObject.tag == "Enemy2Attack")
        {
            if (GameManager.instance.mainPlayerHp > 0)
                GameManager.instance.mainPlayerHp -= Enemy2.damage;
        }
    }
}