using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Eva : SubAI
{
    [SerializeField] BoxCollider basicAttack1Collider;
    [SerializeField] BoxCollider basicAttack2Collider;

    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject qSkill = null;
    [SerializeField] GameObject wSkillEffect = null;
    [SerializeField] GameObject wSkillShockEffect = null;
    [SerializeField] GameObject EvaJadeSkillEffect = null;
    [SerializeField] GameObject EvaKarmenSkillEffect = null;
    [SerializeField] GameObject eSkillHitBox = null;
    [SerializeField] GameObject EvaHammer = null;
    public Transform wSkillPos = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float followDistance = 5.0f;

    public static float qSkillCoolTime = 10.0f;
    public static float wSkillCoolTime = 5.0f;
    public static float eSkillCoolTime = 5.0f;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;
    float curESkillCoolTime;

    float attackDelay = 1.65f;
    float curAttackDelay;
    float subAttackDelay = 1.5f;

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

    public static List<GameObject> targetEnemys = new List<GameObject>();


    [SerializeField] AudioClip evaAttackSound;
    [SerializeField] AudioClip evaQSkillSound;
    [SerializeField] AudioClip evaWSkillSound;
    [SerializeField] AudioClip evaESkillSound;

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

        canMove = true;
        canDodge = true;
        canAttack = true;
        canSkill = true;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;
        onESkill = true;
        falling = false;
        dead = false;

        doingAttack = false;
        motionEndCheck = true;
        comboContinue = true;

        curAttackDelay = attackDelay;

        eSkillHitBox.SetActive(false);

        attackDistance = 3.5f;

        qSkill.SetActive(false);
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
                    E_Skill();
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
                    Instantiate(EvaHammer, transform.position + transform.up * 1.5f + transform.forward* 0.5f, transform.rotation);
                    vecTarget = transform.position;

                    curAttackDelay = 0;
                }
            }
            else if (currentState == characterState.idle)
            {
                Idle();
                animator.SetBool("Run", false);
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
            //moveSpeed *= 2;
            animator.SetTrigger("Dodge");
            StartCoroutine(DodgeDelay());
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            vecTarget = transform.position;
            animator.SetBool("Run", false);
        }
        else
        {
            moveSpeed = 5f;
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


                SoundManager.instance.SFXPlay("EvaAttack", evaAttackSound);

                vecTarget = transform.position;
                animator.SetTrigger("Attack");
                basicAttack1Collider.enabled = true;
                basicAttack2Collider.enabled = true;

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
            canDodge = false;
            canSkill = false;

            if (GameManager.instance.clientPlayer.curMainCharacter == 1)
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.EvaQSkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.EvaQSkill();

            StartCoroutine(FireGun());
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
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.EvaWSkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.EvaWSkill();

            StartCoroutine(ShockWave());
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
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.EvaESkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.EvaESkill();

            StartCoroutine(EvaJadeSynerge());

            //if (tagCharacter.name == "Jade")
            //{
            //    moveSpeed = 0f;
            //    anim.SetBool("Run", false);
            //    vecTarget = transform.position;

            //    StartCoroutine(EvaJadeSynerge());
            //}
            //else if (tagCharacter.name == "Karmen")
            //{
            //    moveSpeed = 0f;
            //    anim.SetBool("Run", false);
            //    vecTarget = transform.position;

            //    StartCoroutine(EvaKarmenSynerge());
            //}
            //else if (tagCharacter.name == "Leina")
            //{

            //}
        }
        else if(Input.GetKeyDown(KeyCode.E) && onESkill && gameObject.transform.CompareTag("SubCharacter"))
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

            StartCoroutine(EvaJadeSynerge());

            //if (tagCharacter.name == "Jade")
            //{
            //    StartCoroutine(EvaJadeSynerge());
            //}
            //else if (tagCharacter.name == "Karmen")
            //{
            //    StartCoroutine(EvaKarmenSynerge());
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
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1.1f);
        basicAttack1Collider.enabled = false;
        basicAttack2Collider.enabled = false;
        yield return new WaitForSeconds(0.65f);
        canMove = true;
        canDodge = true;
        canSkill = true;
    }
    IEnumerator DodgeDelay()
    {
        canAttack = false;
        canMove = false;
        canSkill = false;
        yield return new WaitForSeconds(0.3f);
        moveSpeed = 7f;
        yield return new WaitForSeconds(1f);
        moveSpeed = 0f;
        yield return new WaitForSeconds(0.5f);
        moveSpeed = 5f;
        canAttack = true;
        canMove = true;
        canSkill = true;
    }
    IEnumerator FireGun()
    {
        qSkill.SetActive(true);
        canMove = false;
        animator.SetTrigger("QSkill");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }

        SoundManager.instance.SFXPlay("EvaQSkill", evaQSkillSound);

        yield return new WaitForSeconds(5.0f);
        qSkill.SetActive(false);

        vecTarget = transform.position;
        animator.SetBool("Run", false);

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

        animator.SetTrigger("WSkill");
        Instantiate(wSkillEffect, wSkillPos.position, wSkillPos.rotation);
        yield return new WaitForSeconds(0.5f);
        animator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(0.5f);
        animator.SetFloat("Speed", 1.0f);


        yield return new WaitForSeconds(0.3f);
        SoundManager.instance.SFXPlay("EvaWSkill", evaWSkillSound);

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
        animator.SetFloat("Speed", 1.0f);


        vecTarget = transform.position;
       
        curWSkillCoolTime = 0.0f;

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
    IEnumerator EvaJadeSynerge()
    {
        canAttack = false;
        canMove = false;
        canDodge = false;
        canSkill = false;
        curWSkillCoolTime = 0.0f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }

        animator.SetTrigger("WSkill");

        Instantiate(wSkillEffect, wSkillPos.position, wSkillPos.rotation);
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(-90, 0, 0);
        Instantiate(EvaJadeSkillEffect, wSkillPos.position, rotation);
        yield return new WaitForSeconds(0.5f);
        animator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(0.5f);
        animator.SetFloat("Speed", 1.0f);
        SoundManager.instance.SFXPlay("EvaEkill", evaESkillSound);

        eSkillHitBox.SetActive(true);
        yield return new WaitForSeconds(1f);
        eSkillHitBox.SetActive(false);
        animator.SetFloat("Speed", 1.0f);


        vecTarget = transform.position;

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }
    IEnumerator EvaKarmenSynerge()
    {
        canAttack = false;
        canMove = false;
        canDodge = false;
        canSkill = false;
        curWSkillCoolTime = 0.0f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }

        animator.SetTrigger("EvaKarmenSynerge");

        float ditectedDistance = 10f;

        for (int i = 0; i < targets.Count; i++)
        {
            if (Vector3.Distance(transform.position, targets[i].transform.position) < ditectedDistance)
            {
                targetEnemys.Add(targets[i]);
            }
        }

        Instantiate(wSkillEffect, wSkillPos.position, wSkillPos.rotation);
        Quaternion rotation = Quaternion.identity;
        rotation.eulerAngles = new Vector3(-90, 0, 0);
        Instantiate(EvaKarmenSkillEffect, wSkillPos.position, rotation);
        yield return new WaitForSeconds(0.5f);
        animator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(0.5f);
        animator.SetFloat("Speed", 1.0f);

        yield return new WaitForSeconds(1f);
        animator.SetFloat("Speed", 1.0f);

        vecTarget = transform.position;

        targetEnemys.Clear();
        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
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
        yield return new WaitForSeconds(2.2f);
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