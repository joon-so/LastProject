using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jade : SubAI
{
    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject useAssaultRifle = null;
    [SerializeField] GameObject useMissileLauncher = null;
    [SerializeField] GameObject backAssaultRifle = null;
    [SerializeField] GameObject backMissileLauncher = null;

    [SerializeField] Transform assaultRifleBulletPos = null;
    [SerializeField] GameObject assaultRifleBullet = null;

    [SerializeField] Transform missileBulletPos = null;
    [SerializeField] GameObject missileBullet = null;
    [SerializeField] GameObject missileRange = null;
    [SerializeField] GameObject missileEffect = null;

    [SerializeField] GameObject SynergeBullet = null;

    [SerializeField] Transform grenadePos = null;
    [SerializeField] GameObject Grenade = null;

    public AudioClip attackClip;
    public AudioClip qSkillClip;
    public AudioClip wSkillClip;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float fireDelay = 0.5f;
    public float subFireDelay = 1.5f;
    public float followDistance = 5.0f;
    public float grenadeDistance = 10.0f;

    public static float qSkillCoolTime = 5.0f;
    public static float wSkillCoolTime = 5.0f;
    public static float eSkillCoolTime = 5.0f;
    public static int attackDamage = 20;
    public static int qSkillDamage = 70;
    public static int wSkillDamage = 50;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;
    float curESkillCoolTime;
    
    float curFireDelay;

    bool canMove;
    bool canDodge;
    bool canAttack;
    bool canSkill;
    bool falling;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;
    bool onESkill;

    int characterIndex;

    public static GameObject enemyPos;

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

        vecTarget = transform.position;

        canMove = false;
        canDodge = false;
        canAttack = false;
        canSkill = false;
        falling = false;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;
        onESkill = true;

        curFireDelay = fireDelay;

        StartCoroutine(DrawAssaultRifle());
    }
    void Update()
    {
        CoolTime();
        Tag(); 
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
                    GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
                    Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                    bulletRigid.velocity = assaultRifleBulletPos.forward;
                    
                    moveSpeed = 0f;
                    animator.SetBool("Run", false);
                    vecTarget = transform.position;

                    animator.SetTrigger("shootAssaultRifle");
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
    }
    void FixedUpdate()
    {
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

            moveSpeed *= 2;
            animator.SetTrigger("Dodge");
            
            StartCoroutine(DodgeDelay());
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("DodgeForward"))
        {
            transform.Translate(Vector3.forward *5* Time.deltaTime);
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

                GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
                Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = assaultRifleBulletPos.forward;

                animator.SetBool("Run", false);
                vecTarget = transform.position;

                animator.SetTrigger("shootAssaultRifle");
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
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.JadeQSkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.JadeQSkill();

            StartCoroutine(ShootMissile());
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
                GameManager.instance.clientPlayer.character1Ep -= skillEpManager.JadeWSkill();
            else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                GameManager.instance.clientPlayer.character2Ep -= skillEpManager.JadeWSkill();

            StartCoroutine(ShootGrenade());
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

            StartCoroutine(JadeEvaSynerge());

            //if (tagCharacter.name == "Eva")
            //{
            //    moveSpeed = 0f;
            //    anim.SetBool("Run", false);
            //    vecTarget = transform.position;

            //    StartCoroutine(JadeEvaSynerge());
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

            StartCoroutine(JadeEvaSynerge());

            //if (tagCharacter.name == "Eva")
            //{
            //    StartCoroutine(JadeEvaSynerge());
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
            vecTarget = transform.position;
        }
    }
    IEnumerator AttackDelay()
    {
        //CharacterState.attackCheck = true;
        yield return new WaitForSeconds(0.2f);
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
    IEnumerator DrawAssaultRifle()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(1.0f);
        backAssaultRifle.SetActive(false);
        useAssaultRifle.SetActive(true);
        canMove = true;
        canAttack = true;
        canDodge = true;
        canSkill = true;
    }
    IEnumerator ShootMissile()
    {
        vecTarget = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            animator.SetTrigger("drawMissileLauncher");
            yield return new WaitForSeconds(0.5f);
            useAssaultRifle.SetActive(false);
            useMissileLauncher.SetActive(true);

            animator.SetBool("AimMissile", true);
            yield return new WaitForSeconds(0.5f);
            missileEffect.SetActive(true);
            SoundManager.instance.SFXPlay("Attack", qSkillClip);

            yield return new WaitForSeconds(1.0f);
            animator.SetBool("AimMissile", false);
            missileEffect.SetActive(false);

            animator.SetTrigger("shootMissileLauncher");
            GameObject instantMissile = Instantiate(missileBullet, missileBulletPos.position, missileBulletPos.rotation);
            Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
            missileRigid.velocity = missileBulletPos.forward;

            yield return new WaitForSeconds(1.0f);

            animator.SetTrigger("drawAssaultRifle");
            yield return new WaitForSeconds(0.5f);
            useMissileLauncher.SetActive(false);
            useAssaultRifle.SetActive(true);

            yield return new WaitForSeconds(0.3f);
            canAttack = true;
            canMove = true;
            canDodge = true;
            canSkill = true;
        }
    }
    IEnumerator ShootGrenade()
    {
        vecTarget = transform.position;
        animator.SetTrigger("shootGrenade");
     
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            if (Vector3.Distance(nextVec, transform.position) > 10.0f)
                nextVec = nextVec.normalized * 10.0f;
            
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            SoundManager.instance.SFXPlay("Grenade", wSkillClip);
            GameObject instantGrenade = Instantiate(Grenade, grenadePos.position, grenadePos.rotation);
            Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
            rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
            rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.3f);
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
                StartCoroutine(JadeEvaSynerge());
                yield break;
            }

            yield return null;
        }
    }

    IEnumerator JadeEvaSynerge()
    {
        canAttack = false;
        canMove = false;
        canDodge = false;
        canSkill = false;
        animator.SetTrigger("drawMissileLauncher");
        yield return new WaitForSeconds(0.5f);
        useAssaultRifle.SetActive(false);

        animator.SetBool("AimMissile", true);
        useMissileLauncher.SetActive(true);
        yield return new WaitForSeconds(0.6f);

        animator.SetBool("AimMissile", false);

        animator.SetTrigger("shootMissileLauncher");
        GameObject instantBullet;
        Rigidbody bulletRigid;
        List<GameObject> enemys = new List<GameObject>();
        float ditectedDistance = 10f;
        
        for (int i = 0; i < targets.Count; i++)
        {
            if (Vector3.Distance(transform.position, targets[i].transform.position) < ditectedDistance)
            {
                enemys.Add(targets[i]);
            }
        }
        for (int i = 0; i<10; i++)
        {
            if (enemys.Count != 0)
                enemyPos = enemys[(int)Random.Range(0, enemys.Count)];
            else
                enemyPos = gameObject;
            instantBullet = Instantiate(SynergeBullet, missileBulletPos.position, missileBulletPos.rotation);
            bulletRigid = instantBullet.GetComponent<Rigidbody>();
            bulletRigid.velocity = missileBulletPos.forward;
            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(0.2f);

        animator.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(0.5f);
        useMissileLauncher.SetActive(false);
        useAssaultRifle.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;

        yield break;
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
        if(gameObject.tag == "MainCharacter")
        {
            vecTarget = transform.position;
        }
        else
        {
            navMesh.SetDestination(transform.position);
        }
        vecTarget = transform.position;
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
        if (gameObject.CompareTag("MainCharacter"))
        {
            //if (collision.gameObject.CompareTag("Enemy1Attack"))
            //    collisionManager.Enemy1Attack();
            //if (collision.gameObject.CompareTag("Enemy2Attack"))
            //    collisionManager.Enemy2Attack();
            //if (collision.gameObject.CompareTag("Enemy3Attack"))
            //    collisionManager.Enemy3Attack();
            //if (collision.gameObject.CompareTag("Enemy4Attack"))
            //    collisionManager.Enemy4Attack();
            //if (collision.gameObject.CompareTag("Enemy5Attack"))
            //    collisionManager.Enemy5Attack();
            //if (collision.gameObject.CompareTag("Enemy6Attack"))
            //    collisionManager.Enemy6Attack();
            //if (collision.gameObject.CompareTag("MiniBossAttack"))
            //    collisionManager.MiniBossAttack();
            //if (collision.gameObject.CompareTag("BossAttack1"))
            //    collisionManager.BossAttack1();
            //if (collision.gameObject.CompareTag("BossAttack2"))
            //    collisionManager.BossAttack2();
            //if (collision.gameObject.CompareTag("BossAttack3"))
            //    collisionManager.BossAttack3();
            //if (collision.gameObject.CompareTag("BossAttack4"))
            //    collisionManager.BossAttack4();
            //if (collision.gameObject.CompareTag("BossAttack5"))
            //    collisionManager.BossAttack5();
        }
    }
}