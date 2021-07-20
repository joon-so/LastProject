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

    [SerializeField] GameObject wLeftEffect = null;
    [SerializeField] GameObject wRightEffect = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float followDistance = 5.0f;

    public static float qSkillCoolTime = 5.0f;
    public static float wSkillCoolTime = 5.0f;
    public static float eSkillCoolTime = 5.0f;
    public static int attackDamage = 20;
    public static int qSkillDamage = 60;
    public static int wSkillDamage = 10;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;
    float curESkillCoolTime;

    float curFireDelay;
    float subFireDelay = 1.5f;

    bool canMove;
    bool canDodge;
    bool canAttack;
    bool canSkill;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;
    bool onESkill;

    bool doingAttack;
    bool motionEndCheck;
    bool comboContinue;

    bool canCombo;
    int comboStep;

    float distanceWithPlayer;

    Vector3 vecTarget;

    Animator anim;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (GameManager.instance.isMainKarmen)
        {
            nav.enabled = false;
            tagCharacter = GameManager.instance.character2;
        }
        else if (GameManager.instance.isSubKarmen)
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
        curESkillCoolTime = 0;


        canMove = false;
        canDodge = false;
        canAttack = false;
        canSkill = false;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;
        onESkill = true;

        doingAttack = false;
        motionEndCheck = true;
        comboContinue = true;

        attackDistance = 3.5f;

        StartCoroutine(StartMotion());
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
            curFireDelay += Time.deltaTime;
            distance = Vector3.Distance(tagCharacter.transform.position, transform.position);

            if (currentState == characterState.trace)
            {
                MainCharacterTrace(tagCharacter.transform.position);
                anim.SetBool("isRun", true);
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
                    moveSpeed = 0f;
                    anim.SetBool("isRun", false);
                    anim.SetTrigger("Throwing");
                    vecTarget = transform.position;

                    curFireDelay = 0;
                }
            }
            else if (currentState == characterState.idle)
            {
                Idle();
                anim.SetBool("isRun", false);
                curFireDelay = 1f;
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
        anim.SetBool("isRun", vecTarget != transform.position);

        if (doingAttack)
        {
            anim.SetBool("isRun", false);
            vecTarget = transform.position;
        }
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
            anim.SetTrigger("Dodge");

            StartCoroutine(DodgeDelay());
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("salto2SS"))
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            vecTarget = transform.position;
            anim.SetBool("Run", false);
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
                    anim.SetBool("doAttack", doingAttack);

                }
                motionEndCheck = true;
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            canMove = false;
            anim.SetBool("isRun", canMove);

            if ((doingAttack && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
                 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle1SS")
                 || anim.GetCurrentAnimatorStateInfo(0).IsName("runSS"))
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
            anim.SetBool("doAttack", doingAttack);
        }

        if (doingAttack && Input.GetMouseButtonDown(1))
        {
            doingAttack = false;
            anim.SetBool("doAttack", doingAttack);
            canMove = true;
            anim.SetBool("isRun", canMove);
        }
    }
    public void CanCombo()
    {
        canCombo = true;
    }

    public void Combo()
    {
        if (comboStep == 2)
            anim.Play("Attack2");
        if (comboStep == 3)
            anim.Play("Attack3");
        if (comboStep == 4)
            anim.Play("Attack4");
    }

    public void ComboReset()
    {
        canCombo = false;
        comboStep = 0;
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
        if (curESkillCoolTime < eSkillCoolTime)
        {
            curESkillCoolTime += Time.deltaTime;
        }
        else
        {
            onESkill = true;
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
            canMove = false;
            canDodge = false;
            canSkill = false;
            
            StartCoroutine(BigAttack());
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
             
            StartCoroutine(StraightAttack());
        }
    }
    void E_Skill()
    {
        if (Input.GetKeyDown(KeyCode.E) && onESkill && gameObject.transform.tag == "MainCharacter")
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            Vector3 frontVec = transform.position;
            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                frontVec = rayHit.point - transform.position;
                frontVec.y = 0;
                transform.LookAt(transform.position + frontVec);
            }

            if (tagCharacter.name == "Jade")
            {
                moveSpeed = 0f;
                anim.SetBool("Run", false);
                vecTarget = transform.position;

            }
            else if (tagCharacter.name == "Eva")
            {
                moveSpeed = 0f;
                anim.SetBool("Run", false);
                vecTarget = transform.position;

                StartCoroutine(KarmenEvaSynerge());
            }
            else if (tagCharacter.name == "Leina")
            {

            }
        }
        else if (Input.GetKeyDown(KeyCode.E) && onESkill && gameObject.transform.tag == "SubCharacter")
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            Vector3 frontVec = transform.position;
            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            {
                frontVec = rayHit.point - transform.position;
                frontVec.y = 0;
                transform.LookAt(transform.position + frontVec);
            }

            if (tagCharacter.name == "Jade")
            {
            }
            else if (tagCharacter.name == "Eva")
            {

            }
            else if (tagCharacter.name == "Leina")
            {

            }
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
        canMove = true;
        canDodge = true;
        canSkill = true;
        yield return new WaitForSeconds(0.2f);
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
        anim.SetTrigger("StartMotion");
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
      
        anim.SetTrigger("QSkill");
        anim.SetFloat("Speed", 0.2f);
        yield return new WaitForSeconds(0.5f);
        Instantiate(qSkill, qSkillPos.position, qSkillPos.rotation);
        anim.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(1.0f);
        anim.SetFloat("Speed", 1.0f);
        yield return new WaitForSeconds(1.0f);

        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        vecTarget = transform.position;
        anim.SetBool("Run", false);

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

        anim.SetTrigger("WSkill");
        wLeftEffect.SetActive(true);
        wRightEffect.SetActive(true);
        yield return new WaitForSeconds(2.8f);

        wLeftEffect.SetActive(false);
        wRightEffect.SetActive(false);

        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        vecTarget = transform.position;
        anim.SetBool("Run", false);

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
        anim.SetTrigger("KarmenEvaSynerge");
        yield return null;
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