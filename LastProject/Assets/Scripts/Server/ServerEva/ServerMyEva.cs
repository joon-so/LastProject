using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ServerMyEva : ServerSubAIManager
{
    [SerializeField] BoxCollider basicAttack1Collider;
    [SerializeField] BoxCollider basicAttack2Collider;

    [SerializeField] GameObject qSkill;
    [SerializeField] GameObject wSkillEffect;
    [SerializeField] GameObject wSkillShockEffect;
    public Transform wSkillPos = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float followDistance = 5.0f;
    public float attackDelay = 1.1f;

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
        FindPlayers();

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
        }

        StartCoroutine(StartMotion());

        qSkill.SetActive(false);
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
            //distance = Vector3.Distance(tagCharacter.transform.position, transform.position);

            //if (currentState == characterState.trace)
            //{
            //    MainCharacterTrace(tagCharacter.transform.position);
            //    myAnimator.SetBool("Run", true);
            //}
            //else if (currentState == characterState.attack)
            //{
            //    SubAttack();
            //}
            //else if (currentState == characterState.idle)
            //{
            //    Idle();
            //    myAnimator.SetBool("Run", false);
            //}
        }
        Tag();
        CoolTime();
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
                basicAttack1Collider.enabled = true;
                basicAttack2Collider.enabled = true;
                ServerLoginManager.playerList[0].mainCharacterBehavior = 3;

                curAttackDelay = 0;

                StartCoroutine(AttackDelay());
            }
        }
        if (Input.GetMouseButtonUp(0))
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    void Dead()
    {
        if (characterIndex == 1)
        {
            if (ServerLoginManager.playerList[0].character1Hp <= 0)
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

            if (ServerLoginManager.playerList[0].is_Main_Character == 1)
                ServerLoginManager.playerList[0].character1Ep -= skillEpManager.EvaQSkill();
            else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                ServerLoginManager.playerList[0].character2Ep -= skillEpManager.EvaQSkill();
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

            if(ServerLoginManager.playerList[0].is_Main_Character == 1)
                ServerLoginManager.playerList[0].character1Ep -= skillEpManager.EvaWSkill();
            else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                ServerLoginManager.playerList[0].character2Ep -= skillEpManager.EvaWSkill();
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
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
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

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(1.1f);
        canMove = true;
        canDodge = true;
        canSkill = true;
        basicAttack1Collider.enabled = false;
        basicAttack2Collider.enabled = false;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    IEnumerator StartMotion()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetTrigger("StartMotion");
        yield return new WaitForSeconds(1.8f);
        canMove = true;
        canAttack = true;
        canDodge = true;
        canSkill = true;
    }

    IEnumerator FireGun()
    {
        qSkill.SetActive(true);
        
        myAnimator.SetTrigger("QSkill");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }
        ServerLoginManager.playerList[0].mainCharacterBehavior = 4;

        yield return new WaitForSeconds(5.0f);
        qSkill.SetActive(false);

        vecTarget = transform.position;
        myAnimator.SetBool("Run", false);
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;

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
        wSkillEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetFloat("Speed", 1.0f);
        wSkillEffect.SetActive(false);


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

        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetFloat("Speed", 1.0f);

        vecTarget = transform.position;

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