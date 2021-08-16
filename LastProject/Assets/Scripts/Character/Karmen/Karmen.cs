using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Karmen : SubAI
{
    [SerializeField] GameObject attackRange = null;

    [SerializeField] GameObject leftStaffEffect = null;
    [SerializeField] GameObject rightStaffEffect = null;

    [SerializeField] GameObject qSkill = null;
    public Transform qSkillPos = null;

    [SerializeField] CapsuleCollider leftStaff;
    [SerializeField] CapsuleCollider rightStaff;
    [SerializeField] BoxCollider throwingStaff;

    [SerializeField] GameObject wLeftEffect = null;
    [SerializeField] GameObject wRightEffect = null;
    public GameObject EvaKarmenSynergeWeapon = null;
    [SerializeField] GameObject EvaKarmenSynergeEffect = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float followDistance = 5.0f;

    public static float qSkillCoolTime = 5.0f;
    public static float wSkillCoolTime = 5.0f;
    public static float eSkillCoolTime = 5.0f;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;
    float curESkillCoolTime;

    float curAttackDelay;
    float subAttackDelay = 1.5f;
    public float attackDelay = 0.8f;

    bool canMove;
    bool canDodge;
    bool canAttack;
    bool canSkill;
    bool falling;
    bool dead;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;
    bool onESkill;

    bool doingAttack;
    bool motionEndCheck;
    bool comboContinue;

    int characterIndex;

    Vector3 vecTarget;

    Animator animator;
    Rigidbody rigidbody;
    ClientCollisionManager collisionManager;
    ClientSkillEpManager skillEpManager;
    CapsuleCollider capsuleCollider;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        collisionManager = GameObject.Find("GameManager").GetComponent<ClientCollisionManager>();
        skillEpManager = GameObject.Find("GameManager").GetComponent<ClientSkillEpManager>();
    }

    void Start()
    {
        curDodgeCoolTime = dodgeCoolTime;
        curQSkillCoolTime = qSkillCoolTime;
        curWSkillCoolTime = wSkillCoolTime;
        curESkillCoolTime = eSkillCoolTime;

        if (gameObject.transform.CompareTag("MainCharacter"))
        {
            tagCharacter = GameManager.instance.character2;

            characterIndex = 1;
            PlayerManager.instance.c1DodgeCoolTime = dodgeCoolTime;
            PlayerManager.instance.c1QSkillCoolTime = qSkillCoolTime;
            PlayerManager.instance.c1WSkillCoolTime = wSkillCoolTime;
            PlayerManager.instance.c1ESkillCoolTime = eSkillCoolTime;

            PlayerManager.instance.curC1DodgeCoolTime = curDodgeCoolTime;
            PlayerManager.instance.curC1QSkillCoolTime = curQSkillCoolTime;
            PlayerManager.instance.curC1WSkillCoolTime = curWSkillCoolTime;
            PlayerManager.instance.curC1ESkillCoolTime = curESkillCoolTime;
            navMesh.enabled = false;
        }
        else if (gameObject.transform.CompareTag("SubCharacter"))
        {
            tagCharacter = GameManager.instance.character1;

            characterIndex = 2;

            PlayerManager.instance.c2DodgeCoolTime = dodgeCoolTime;
            PlayerManager.instance.c2QSkillCoolTime = qSkillCoolTime;
            PlayerManager.instance.c2WSkillCoolTime = wSkillCoolTime;
            PlayerManager.instance.c2ESkillCoolTime = eSkillCoolTime;

            PlayerManager.instance.curC2DodgeCoolTime = curDodgeCoolTime;
            PlayerManager.instance.curC2QSkillCoolTime = curQSkillCoolTime;
            PlayerManager.instance.curC2WSkillCoolTime = curWSkillCoolTime;
            PlayerManager.instance.curC2ESkillCoolTime = curESkillCoolTime;
            navMesh.enabled = true;
        }

        FindEnemys();
        rigidbody.freezeRotation = true;
        vecTarget = transform.position;

        canMove = false;
        canDodge = false;
        canAttack = false;
        canSkill = false;
        falling = false;
        dead = false;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;
        onESkill = true;

        doingAttack = false;
        motionEndCheck = true;
        comboContinue = true;
        throwingStaff.enabled = false;

        curAttackDelay = attackDelay;

        attackDistance = 3.5f;

        EvaKarmenSynergeWeapon.SetActive(false);


        StartCoroutine(StartMotion());
    }
    void Update()
    {
        CoolTime();
        Tag();
        if (gameObject.transform.CompareTag("MainCharacter"))
        {
            curAttackDelay += Time.deltaTime;
            if (!falling)
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
                }
                Stop();
                AttackRange();
            }
        }
        else if (gameObject.transform.CompareTag("SubCharacter") && !falling)
        {
            curAttackDelay += Time.deltaTime;
            distance = Vector3.Distance(tagCharacter.transform.position, transform.position);

            if (currentState == characterState.trace)
            {
                MainCharacterTrace(tagCharacter.transform.position);
                animator.SetBool("Run", true);
                curAttackDelay = 1f;
            }
            else if (currentState == characterState.attack)
            {
                SubAttack();

                if (target)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
                    transform.rotation = Quaternion.Euler(0, euler.y, 0);
                }
                if (curAttackDelay > subAttackDelay && target != null)
                {
                    moveSpeed = 0f;
                    animator.SetBool("Run", false);
                    animator.SetTrigger("Throwing");

                    vecTarget = transform.position;
                    StartCoroutine(SubAttackHitBoxTime());

                    curAttackDelay = 0;
                }
            }
            else if (currentState == characterState.idle)
            {
                Idle();
                animator.SetBool("Run", false);
                curAttackDelay = 1f;
            }
        }
        if (canSkill)
        {
            E_Skill();
        }

        if (PlayerManager.instance.initTargetVec == true)
        {
            moveSpeed = 0f;
            animator.SetBool("Run", false);
            vecTarget = transform.position;
            PlayerManager.instance.initTargetVec = false;
        }
    }
    void FixedUpdate()
    {
        if (GameManager.instance.clientPlayer.character1Hp <= 0 || GameManager.instance.clientPlayer.character2Hp <= 0)
        {
            if (!dead)
            {
                capsuleCollider.isTrigger = true;
                canAttack = false;
                canDodge = false;
                canMove = false;
                canSkill = false;
                animator.SetTrigger("Dead");
                dead = true;
            }
        }
        FindEnemys();
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
        animator.SetBool("Run", vecTarget != transform.position);

        if (doingAttack)
        {
            animator.SetBool("Run", false);
            vecTarget = transform.position;
        }
    }
    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveSpeed = 0f;
            animator.SetBool("Run", false);
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
            animator.SetTrigger("Dodge");

            StartCoroutine(DodgeDelay());
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("salto2SS"))
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            vecTarget = transform.position;
            animator.SetBool("Run", false);
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

                animator.SetBool("Run", false);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 nextVec = hit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }

                vecTarget = transform.position;
                animator.SetTrigger("Attack");
                leftStaff.enabled = true;
                rightStaff.enabled = true;
                ServerLoginManager.playerList[0].mainCharacterBehavior = 3; // Attack

                curAttackDelay = 0;

                StartCoroutine(AttackDelay());
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
            if (characterIndex == 1)
                PlayerManager.instance.curC1DodgeCoolTime = curDodgeCoolTime;
            else if (characterIndex == 2)
                PlayerManager.instance.curC2DodgeCoolTime = curDodgeCoolTime;
        }
        else
            onDodge = true;

        if (curQSkillCoolTime < qSkillCoolTime)
        {
            curQSkillCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                PlayerManager.instance.curC1QSkillCoolTime = curQSkillCoolTime;
            else if (characterIndex == 2)
                PlayerManager.instance.curC2QSkillCoolTime = curQSkillCoolTime;
        }
        else
            onQSkill = true;

        if (curWSkillCoolTime < wSkillCoolTime)
        {
            curWSkillCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                PlayerManager.instance.curC1WSkillCoolTime = curWSkillCoolTime;
            else if (characterIndex == 2)
                PlayerManager.instance.curC2WSkillCoolTime = curWSkillCoolTime;
        }
        else
            onWSkill = true;

        if (curESkillCoolTime < eSkillCoolTime)
        {
            curESkillCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                PlayerManager.instance.curC1ESkillCoolTime = curESkillCoolTime;
            else if (characterIndex == 2)
                PlayerManager.instance.curC2ESkillCoolTime = curESkillCoolTime;
        }
        else
            onESkill = true;
    }
    void Q_Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && onQSkill)
        {
            onQSkill = false;
            curQSkillCoolTime = 0;
            animator.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            if (GameManager.instance.clientPlayer.curMainCharacter == 1)
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.KarmenQSkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.KarmenQSkill();

            StartCoroutine(BigAttack());
        }
    }
    void W_Skill()
    {
        if (Input.GetKeyDown(KeyCode.W) && onWSkill)
        {
            onWSkill = false;
            curWSkillCoolTime = 0;
            animator.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            if (GameManager.instance.clientPlayer.curMainCharacter == 1)
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.KarmenWSkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.KarmenWSkill();

            StartCoroutine(StraightAttack());
        }
    }
    void E_Skill()
    {
        if (Input.GetKeyDown(KeyCode.E) && onESkill && gameObject.transform.CompareTag("MainCharacter"))
        {
            curESkillCoolTime = 0.0f;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            Vector3 frontVec = transform.position;
            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                frontVec = rayHit.point - transform.position;
                frontVec.y = 0;
                transform.LookAt(transform.position + frontVec);
            }

            onESkill = false; 
            moveSpeed = 0f;
            animator.SetBool("Run", false);
            vecTarget = transform.position;

            if (GameManager.instance.clientPlayer.curMainCharacter == 1)
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.KarmenESkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.KarmenESkill();

            StartCoroutine(KarmenEvaSynerge());

            //if (tagCharacter.name == "Jade")
            //{
            //    moveSpeed = 0f;
            //    anim.SetBool("Run", false);
            //    vecTarget = transform.position;

            //}
            //else if (tagCharacter.name == "Eva")
            //{
            //    moveSpeed = 0f;
            //    anim.SetBool("Run", false);
            //    vecTarget = transform.position;

            //    StartCoroutine(KarmenEvaSynerge());
            //}
            //else if (tagCharacter.name == "Leina")
            //{

            //}
        }
        else if (Input.GetKeyDown(KeyCode.E) && onESkill && gameObject.transform.CompareTag("SubCharacter"))
        {
            curESkillCoolTime = 0.0f; 
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            Vector3 frontVec = transform.position;
            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                frontVec = rayHit.point - transform.position;
                frontVec.y = 0;
                transform.LookAt(transform.position + frontVec);
            }

            onESkill = false;
            moveSpeed = 0f;
            animator.SetBool("Run", false);
            vecTarget = transform.position;

            StartCoroutine(KarmenEvaSynerge());

            //if (tagCharacter.name == "Jade")
            //{
            //}
            //else if (tagCharacter.name == "Eva")
            //{
            //    StartCoroutine(KarmenEvaSynerge());
            //}
            //else if (tagCharacter.name == "Leina")
            //{

            //}
        }
    }
    void Tag()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            vecTarget = transform.position;
        }
    }
    IEnumerator SubAttackHitBoxTime()
    {
        yield return new WaitForSeconds(0.3f);
        throwingStaff.enabled = true;
        yield return new WaitForSeconds(0.05f);
        throwingStaff.enabled = false;
    }
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.8f);
        canMove = true;
        canDodge = true;
        canSkill = true;
        leftStaff.enabled = false;
        rightStaff.enabled = false;
    }
    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.5f);
        canAttack = true;
        canMove = true;
        canSkill = true;
    }
    IEnumerator StartMotion()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("StartMotion");
        yield return new WaitForSeconds(1.5f);
        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);
        canMove = true;
        canAttack = true;
        canDodge = true;
        canSkill = true;

    }
    IEnumerator BigAttack()
    {
        curQSkillCoolTime = 0.0f;

        leftStaffEffect.SetActive(false);
        rightStaffEffect.SetActive(false);

        animator.SetTrigger("QSkill");
        animator.SetFloat("Speed", 0.2f);
        yield return new WaitForSeconds(0.5f);
        Instantiate(qSkill, qSkillPos.position, qSkillPos.rotation);
        animator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(1.0f);
        animator.SetFloat("Speed", 1.0f);
        yield return new WaitForSeconds(1.0f);

        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        vecTarget = transform.position;
        animator.SetBool("Run", false);

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

        animator.SetTrigger("WSkill");
        wLeftEffect.SetActive(true);
        wRightEffect.SetActive(true);
        yield return new WaitForSeconds(2.8f);

        wLeftEffect.SetActive(false);
        wRightEffect.SetActive(false);

        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        vecTarget = transform.position;
        animator.SetBool("Run", false);

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }
    IEnumerator SynergeCharacterMove(Vector3 frontVec, Vector3 pos)
    {
        canAttack = false;
        canMove = false;
        canDodge = false;
        canSkill = false;
        curESkillCoolTime = 0;

        //Vector3 target = transform.position + pos * 1.5f;

        //Vector3 nextTagVec = target - tagCharacter.transform.position;
        //nextTagVec.y = 0;

        //while (true)
        //{
        //    tagCharacter.transform.position = Vector3.MoveTowards(tagCharacter.transform.position,
        //                target, 8f * Time.deltaTime);

        //    tagCharacter.transform.LookAt(tagCharacter.transform.position + nextTagVec);

        //    if(Mathf.Abs(target.x - tagCharacter.transform.position.x) < 0.0002 &&
        //        Mathf.Abs(target.z - tagCharacter.transform.position.z) < 0.0002)
        //    {
        //        tagCharacter.transform.LookAt(tagCharacter.transform.position + frontVec);

        //        canAttack = true;
        //        canMove = true;
        //        canDodge = true;
        //        canSkill = true;
        //        Debug.Log("finish");
        //        yield break;
        //    }

        //    yield return null;
        //}

        NavMeshAgent subNav = tagCharacter.GetComponent<NavMeshAgent>();
        subNav.speed = 12;

        Vector3 target = transform.position + pos * 1.5f;

        while (true)
        {
            subNav.SetDestination(target);

            if (Mathf.Abs(target.x - tagCharacter.transform.position.x) < 0.0003 &&
                Mathf.Abs(target.z - tagCharacter.transform.position.z) < 0.0003)
            {
                tagCharacter.transform.LookAt(tagCharacter.transform.position + frontVec);
                subNav.speed = 3.5f;
                canAttack = true;
                canMove = true;
                canDodge = true;
                canSkill = true;
                //StartCoroutine(JadeEvaSynergeShoot());
                yield break;
            }

            yield return null;
        }
    }
    IEnumerator KarmenEvaSynerge()
    {
        EvaKarmenSynergeWeapon.SetActive(true);
        animator.SetTrigger("KarmenEvaSynerge");

        yield return new WaitForSeconds(3.45f);
        Instantiate(EvaKarmenSynergeEffect, transform.position + transform.forward * 4.5f, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        EvaKarmenSynergeWeapon.SetActive(false);
    }
    IEnumerator FallDown()
    {
        falling = true;
        animator.SetTrigger("Attacked");
        float hitTime = 0.8f;
        while (hitTime > 0)
        {
            hitTime -= Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position - transform.forward * 2f, 5.0f * Time.deltaTime);
            yield return null;
        }
        if (gameObject.tag == "MainCharacter")
        {
            vecTarget = transform.position;
        }
        else
        {
            navMesh.SetDestination(transform.position);
        }
        yield return new WaitForSeconds(2.6f);
        falling = false;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (dead)
            return;
       
        if (collision.gameObject.tag == "Boss" && !falling)
        {
            GameObject boss = collision.gameObject;
            Vector3 pos = boss.transform.position - boss.transform.forward * 2f;
            pos.y = transform.position.y;
            transform.LookAt(pos);
            StartCoroutine(FallDown());
        }
        if (gameObject.CompareTag("MainCharacter"))
        {
            if (collision.gameObject.CompareTag("Enemy1Attack"))
                collisionManager.Enemy1Attack();
            if (collision.gameObject.CompareTag("Enemy2Attack"))
                collisionManager.Enemy2Attack();
            if (collision.gameObject.CompareTag("Enemy3Attack"))
                collisionManager.Enemy3Attack();
            if (collision.gameObject.CompareTag("Enemy6Attack"))
                collisionManager.Enemy6Attack();
            if (collision.gameObject.CompareTag("Enemy7Attack"))
            {
                GameObject enemy = collision.gameObject;
                Vector3 pos = enemy.transform.position - enemy.transform.forward * 2f;
                pos.y = transform.position.y;
                transform.LookAt(pos);
                StartCoroutine(FallDown());
                collisionManager.Enemy7Attack();
            }
            if (collision.gameObject.CompareTag("BossAttack1"))
                collisionManager.BossAttack1();
            if (collision.gameObject.CompareTag("BossAttack2"))
                collisionManager.BossAttack2();
            if (collision.gameObject.CompareTag("BossAttack3"))
                collisionManager.BossAttack3();
            if (collision.gameObject.CompareTag("BossAttack4"))
                collisionManager.BossAttack4();
            if (collision.gameObject.CompareTag("BossAttack5"))
                collisionManager.BossAttack5();
        }
    }
}