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

    [SerializeField] CapsuleCollider leftStaff;
    [SerializeField] CapsuleCollider rightStaff;
    [SerializeField] GameObject leftfoot;

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
    ServerCollisionManager collisionManager;
    ServerSkillEpManager skillEpManager;

    int characterIndex;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        collisionManager = GameObject.Find("ServerIngameManager").GetComponent<ServerCollisionManager>();
        skillEpManager = GameObject.Find("ServerIngameManager").GetComponent<ServerSkillEpManager>();
    }

    void Start()
    {
        curDodgeCoolTime = dodgeCoolTime;
        curQSkillCoolTime = qSkillCoolTime;
        curWSkillCoolTime = wSkillCoolTime;

        if (gameObject.transform.CompareTag("MainCharacter"))
        {
            tagCharacter = ServerMyPlayerManager.instance.character2;

            characterIndex = 1;

            ServerMyPlayerManager.instance.c1DodgeCoolTime = dodgeCoolTime;
            ServerMyPlayerManager.instance.c1QSkillCoolTime = qSkillCoolTime;
            ServerMyPlayerManager.instance.c1WSkillCoolTime = wSkillCoolTime;

            ServerMyPlayerManager.instance.curC1DodgeCoolTime = curDodgeCoolTime;
            ServerMyPlayerManager.instance.curC1QSkillCoolTime = curQSkillCoolTime;
            ServerMyPlayerManager.instance.curC1WSkillCoolTime = curWSkillCoolTime;
            nav.enabled = false;
        }
        else if (gameObject.transform.CompareTag("SubCharacter"))
        {
            tagCharacter = ServerMyPlayerManager.instance.character1;

            characterIndex = 2;

            ServerMyPlayerManager.instance.c2DodgeCoolTime = dodgeCoolTime;
            ServerMyPlayerManager.instance.c2QSkillCoolTime = qSkillCoolTime;
            ServerMyPlayerManager.instance.c2WSkillCoolTime = wSkillCoolTime;

            ServerMyPlayerManager.instance.curC2DodgeCoolTime = curDodgeCoolTime;
            ServerMyPlayerManager.instance.curC2QSkillCoolTime = curQSkillCoolTime;
            ServerMyPlayerManager.instance.curC2WSkillCoolTime = curWSkillCoolTime;
            nav.enabled = true;
        }

        FindPlayers();

        vecTarget = transform.position;

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
        if (gameObject.transform.CompareTag("MainCharacter"))
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
            Dead();
        }
        else if (gameObject.transform.CompareTag("SubCharacter"))
        {
            attackDelay += Time.deltaTime;
            distance = Vector3.Distance(tagCharacter.transform.position, transform.position);

            if (currentState == characterState.trace)
            {
                MainCharacterTrace(tagCharacter.transform.position);
                myAnimator.SetBool("Run", true);
                attackDelay = 1f;
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
                if (attackDelay > subAttackDelay && target != null)
                {
                    moveSpeed = 0f;
                    myAnimator.SetBool("Run", false);
                    myAnimator.SetTrigger("Throwing");
                    vecTarget = transform.position;

                    attackDelay = 0;
                }
            }
            else if (currentState == characterState.idle)
            {
                Idle();
                myAnimator.SetBool("Run", false);
                attackDelay = 1f;
            }
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
                leftStaff.enabled = true;
                rightStaff.enabled = true;
                ServerLoginManager.playerList[0].mainCharacterBehavior = 3; // Attack

                curAttackDelay = 0;

                StartCoroutine(AttackDelay());
            }
        }
        if (Input.GetMouseButtonUp(0))
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    void Dead()
    {
        if(characterIndex == 1)
        {
            if(ServerLoginManager.playerList[0].character1Hp <= 0)
            {
                StartCoroutine(Death());
            }
        }
        else if (characterIndex == 2)
        {
            if (ServerLoginManager.playerList[0].character2Hp <= 0)
            {
                StartCoroutine(Death());
            }
        }
    }

    void CoolTime()
    {
        if (curDodgeCoolTime < dodgeCoolTime)
        {
            curDodgeCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                ServerMyPlayerManager.instance.curC1DodgeCoolTime = curDodgeCoolTime;
            else if (characterIndex == 2)
                ServerMyPlayerManager.instance.curC2DodgeCoolTime = curDodgeCoolTime;
        }
        else
            onDodge = true;

        if (curQSkillCoolTime < qSkillCoolTime)
        {
            curQSkillCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                ServerMyPlayerManager.instance.curC1QSkillCoolTime = curQSkillCoolTime;
            else if (characterIndex == 2)
                ServerMyPlayerManager.instance.curC2QSkillCoolTime = curQSkillCoolTime;
        }
        else
            onQSkill = true;

        if (curWSkillCoolTime < wSkillCoolTime)
        {
            curWSkillCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                ServerMyPlayerManager.instance.curC1WSkillCoolTime = curWSkillCoolTime;
            else if (characterIndex == 2)
                ServerMyPlayerManager.instance.curC2WSkillCoolTime = curWSkillCoolTime;
        }
        else
            onWSkill = true;
    }
    void Tag()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (ServerMyPlayerManager.instance.onTag)
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

            if (ServerLoginManager.playerList[0].is_Main_Character == 1)
                ServerLoginManager.playerList[0].character1Ep -= skillEpManager.KarmenQSkill();
            else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                ServerLoginManager.playerList[0].character2Ep -= skillEpManager.KarmenQSkill();

            StartCoroutine(BigAttack());
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

            if (ServerLoginManager.playerList[0].is_Main_Character == 1)
                ServerLoginManager.playerList[0].character1Ep -= skillEpManager.KarmenWSkill();
            else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                ServerLoginManager.playerList[0].character2Ep -= skillEpManager.KarmenWSkill();

            StartCoroutine(StraightAttack());
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
        leftStaff.enabled = false;
        rightStaff.enabled = false;
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

    IEnumerator Death()
    {
        Debug.Log("my jade Á×À½");
        canMove = false;
        canAttack = false;
        canSkill = false;
        canDodge = false;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 6;
        myAnimator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.9f);
    }

    IEnumerator BigAttack()
    {
        curQSkillCoolTime = 0.0f;

        leftStaffEffect.SetActive(false);
        rightStaffEffect.SetActive(false);

        ServerLoginManager.playerList[0].mainCharacterBehavior = 4; // WSkill
        myAnimator.SetTrigger("QSkill");

        myAnimator.SetFloat("Speed", 0.2f);
        yield return new WaitForSeconds(0.5f);
        qSkill.SetActive(true);
        myAnimator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(1.0f);
        myAnimator.SetFloat("Speed", 1.0f);
        yield return new WaitForSeconds(1.0f);
        qSkill.SetActive(false);
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

        ServerLoginManager.playerList[0].mainCharacterBehavior = 5; // WSkill
        myAnimator.SetTrigger("WSkill");
        wLeftEffect.SetActive(true);
        wRightEffect.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        leftfoot.SetActive(true);
        yield return new WaitForSeconds(0.8f);

        wLeftEffect.SetActive(false);
        wRightEffect.SetActive(false);
        leftfoot.SetActive(false);

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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
            return;

        if (ServerLoginManager.playerList[0].character1Hp <= 0 || ServerLoginManager.playerList[0].character2Hp <= 0)
            return;
        
        if (gameObject.CompareTag("MainCharacter"))
        {
            if (collision.gameObject.CompareTag("KarmenAttack"))
                collisionManager.KarmenBasicAttack();
            if (collision.gameObject.CompareTag("KarmenQSkill"))
                collisionManager.KarmenQSkillAttack();
            if (collision.gameObject.CompareTag("KarmenWSkill"))
                collisionManager.KarmenWSkillAttack();
            if (collision.gameObject.CompareTag("JadeAttack"))
                collisionManager.JadeBasicAttack();
            if (collision.gameObject.CompareTag("JadeQSkill"))
                collisionManager.JadeQSkillAttack();
            if (collision.gameObject.CompareTag("JadeWSkill"))
                collisionManager.JadeWSkillAttack();
            if (collision.gameObject.CompareTag("LeinaAttack"))
                collisionManager.LeinaBasicAttack();
            if (collision.gameObject.CompareTag("LeinaQSkill"))
                collisionManager.LeinaQSkillAttack();
            if (collision.gameObject.CompareTag("LeinaWSkill"))
                collisionManager.LeinaWSkillAttack();
            if (collision.gameObject.CompareTag("EvaAttack"))
                collisionManager.EvaBasicAttack();
            if (collision.gameObject.CompareTag("EvaWSkill"))
                collisionManager.EvaWSkillAttack();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 10)
            return;

        if (ServerLoginManager.playerList[0].character1Hp <= 0 || ServerLoginManager.playerList[0].character2Hp <= 0)
            return;

        if (gameObject.CompareTag("MainCharacter"))
        {
            if (other.gameObject.CompareTag("EvaQSkill"))
                collisionManager.EvaQSkillAttack();
        }
    }
}