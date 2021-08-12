using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Leina : SubAI
{
    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject arrow = null;
    [SerializeField] Transform arrowPos = null;

    [SerializeField] GameObject posionArrow = null;
    [SerializeField] GameObject SynergeArrow = null;
    [SerializeField] GameObject SynergeEffect = null;
    [SerializeField] Transform posionArrowPos = null;

    [SerializeField] GameObject rainArrow = null;
    [SerializeField] Transform rainArrowPos = null;

    public AudioClip attackClip;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float fireDelay = 1.0f;
    public float subFireDelay = 1.5f;
    public float followDistance = 5.0f;

    public static float qSkillCoolTime = 5.0f;
    public static float wSkillCoolTime = 5.0f;
    public static float eSkillCoolTime = 5.0f;
    public static int attackDamage = 20;
    public static int qSkillDamage = 70;
    public static int wSkillDamage = 30;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;
    float curESkillCoolTime;

    float curFireDelay;

    bool canAttack;
    bool canDodge;
    bool canMove;
    bool canSkill;
    bool falling;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;
    bool onESkill;
    int characterIndex;

    Vector3 vecTarget;

    Animator animator;
    Rigidbody rigidbody;
    ClientCollisionManager collisionManager;
    ClientSkillEpManager skillEpManager;

    void Awake()
    {
        animator = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
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

        canMove = true;
        canDodge = true;
        canAttack = true;
        canSkill = true;
        falling = false;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;
        onESkill = true;

        curFireDelay = fireDelay;
    }
    void Update()
    {
        curFireDelay += Time.deltaTime;
        if (gameObject.transform.CompareTag("MainCharacter"))
        {
            if(!falling)
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
            }
        }
        else if (gameObject.transform.CompareTag("SubCharacter") && !falling)
        {
            distance = Vector3.Distance(tagCharacter.transform.position, transform.position);
            if (currentState == characterState.trace)
            {
                MainCharacterTrace(tagCharacter.transform.position);
                animator.SetBool("Run", true);
                curFireDelay = 1f;
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
                if (curFireDelay > subFireDelay && target != null)
                {
                    GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
                    Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
                    arrowRigid.velocity = arrowPos.forward;

                    moveSpeed = 0f;
                    animator.SetBool("Run", false);
                    vecTarget = transform.position;

                    animator.SetTrigger("Attack");
                    curFireDelay = 0;

                    StartCoroutine(AttackDelay());
                }
            }
            else if (currentState == characterState.idle)
            {
                Idle();
                animator.SetBool("Run", false);
                curFireDelay = 1f;
            }
        }
        if (canSkill)
        {
            E_Skill();
        }
        CoolTime();
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
        animator.SetBool("Run", vecTarget != transform.position);
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
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
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
            if (curFireDelay > fireDelay)
            {
                canMove = false;
                canDodge = false;
                canSkill = false;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 nextVec = hit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }
                SoundManager.instance.SFXPlay("Attack", attackClip);

                GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
                Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
                arrowRigid.velocity = arrowPos.forward;

                moveSpeed = 0f;
                animator.SetBool("Run", false);
                vecTarget = transform.position;

                animator.SetTrigger("Attack");
                curFireDelay = 0;

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
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.LeinaQSkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.LeinaQSkill();

            StartCoroutine(ChargingShot());
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
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.LeinaWSkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.LeinaWSkill();

            StartCoroutine(WideShot());
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

            StartCoroutine(SynergeSkill());

            //if (tagCharacter.name == "Eva")
            //{
            //    //StartCoroutine(SynergeSkill());
            //}
            //else if (tagCharacter.name == "Karmen")
            //{

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

            StartCoroutine(SynergeSkill());
            //if (tagCharacter.name == "Eva")
            //{
            //    //StartCoroutine(JadeEvaSynerge());
            //}
            //else if (tagCharacter.name == "Karmen")
            //{

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
            if (PlayerManager.instance.onTag)
                vecTarget = transform.position;
        }
    }
    IEnumerator AttackDelay()
    {
        //CharacterState.attackCheck = true;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canDodge = true;
        canSkill = true;
        //CharacterState.attackCheck = false;
    }
    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
        canMove = true;
        canSkill = true;
    }

    IEnumerator ChargingShot()
    {
        vecTarget = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
            transform.Rotate(0, transform.rotation.y + 90, 0);
        }
        animator.SetTrigger("QSkill");
        // Â÷Â¡
        yield return new WaitForSeconds(1.4f);
        GameObject instantArrow = Instantiate(posionArrow, posionArrowPos.position, posionArrowPos.rotation);
        LeinaPosionArrow.speed = 0;
        // ¼¦
        yield return new WaitForSeconds(1f);

        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = posionArrowPos.forward;
        LeinaPosionArrow.speed = 40;
        yield return new WaitForSeconds(1.0f);

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }
    IEnumerator WideShot()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }
        SoundManager.instance.SFXPlay("Attack", attackClip);

        animator.SetBool("Run", false);
        vecTarget = transform.position;

        animator.SetTrigger("Attack");
        // ¼¦
        Vector3 pos = arrowPos.position;
        GameObject instantArrow = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -25f, 0));
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward;

        GameObject instantArrow2 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -15f, 0));
        Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
        arrowRigid2.velocity = arrowPos.forward;

        GameObject instantArrow3 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -5f, 0));
        Rigidbody arrowRigid3 = instantArrow3.GetComponent<Rigidbody>();
        arrowRigid3.velocity = arrowPos.forward;

        GameObject instantArrow4 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 5f, 0));
        Rigidbody arrowRigid4 = instantArrow4.GetComponent<Rigidbody>();
        arrowRigid4.velocity = arrowPos.forward;

        GameObject instantArrow5 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 15f, 0));
        Rigidbody arrowRigid5 = instantArrow5.GetComponent<Rigidbody>();
        arrowRigid5.velocity = arrowPos.forward;

        GameObject instantArrow6 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 25f, 0));
        Rigidbody arrowRigid6 = instantArrow6.GetComponent<Rigidbody>();
        arrowRigid6.velocity = arrowPos.forward;

        yield return new WaitForSeconds(0.5f);

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }
    IEnumerator SynergeSkill()
    {
        vecTarget = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
            transform.Rotate(0, transform.rotation.y + 90, 0);
        }
        animator.SetTrigger("QSkill");
        // Â÷Â¡
        LeinaSynergeSkill.speed = 0;
        //SynergeFirstArrow.SetActive(true);

        yield return new WaitForSeconds(1.3f);
        GameObject instantArrow = Instantiate(SynergeArrow, posionArrowPos.position, posionArrowPos.rotation);
        // ¼¦
        yield return new WaitForSeconds(1f);
        Destroy(instantArrow);

        GameObject instantEffect = Instantiate(SynergeEffect, transform.position + -transform.right * 7f, transform.rotation);
        yield return new WaitForSeconds(1.0f);
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
        if (GameManager.instance.clientPlayer.character1Hp <= 0 || GameManager.instance.clientPlayer.character2Hp <= 0)
            return;

        if (collision.gameObject.tag == "Boss" && !falling)
        {
            GameObject boss = collision.gameObject;
            Vector3 pos = boss.transform.position - boss.transform.forward * 2f;
            pos.y = transform.position.y;
            transform.LookAt(pos);
            StartCoroutine(FallDown());
        }
        //if (gameObject.CompareTag("MainCharacter"))
        //{
        //    if (collision.gameObject.CompareTag("Enemy1Attack"))
        //        collisionManager.Enemy1Attack();
        //    if (collision.gameObject.CompareTag("Enemy2Attack"))
        //        collisionManager.Enemy2Attack();
        //    if (collision.gameObject.CompareTag("Enemy3Attack"))
        //        collisionManager.Enemy3Attack();
        //    if (collision.gameObject.CompareTag("Enemy4Attack"))
        //        collisionManager.Enemy4Attack();
        //    if (collision.gameObject.CompareTag("Enemy5Attack"))
        //        collisionManager.Enemy5Attack();
        //    if (collision.gameObject.CompareTag("Enemy6Attack"))
        //        collisionManager.Enemy6Attack();
        //    if (collision.gameObject.CompareTag("MiniBossAttack"))
        //        collisionManager.MiniBossAttack();
        //    if (collision.gameObject.CompareTag("BossAttack1"))
        //        collisionManager.BossAttack1();
        //    if (collision.gameObject.CompareTag("BossAttack2"))
        //        collisionManager.BossAttack2();
        //    if (collision.gameObject.CompareTag("BossAttack3"))
        //        collisionManager.BossAttack3();
        //    if (collision.gameObject.CompareTag("BossAttack4"))
        //        collisionManager.BossAttack4();
        //    if (collision.gameObject.CompareTag("BossAttack5"))
        //        collisionManager.BossAttack5();
        //}
    }
}