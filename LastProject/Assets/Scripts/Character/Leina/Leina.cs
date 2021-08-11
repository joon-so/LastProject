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

    bool onDodge;
    bool onQSkill;
    bool onWSkill;
    bool onESkill;

    float distanceWithPlayer;

    Vector3 vecTarget;

    Animator anim;
    int characterIndex;
    void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
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
            nav.enabled = false;
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
            nav.enabled = true;
        }

        FindEnemys();

        nav = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();

        vecTarget = transform.position;

        canMove = true;
        canDodge = true;
        canAttack = true;
        canSkill = true;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;
        onESkill = true;

        curFireDelay = fireDelay;
    }
    void Update()
    {
        curFireDelay += Time.deltaTime;
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
                MainCharacterTrace(tagCharacter.transform.position);
                anim.SetBool("Run", true);
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
                    anim.SetBool("Run", false);
                    vecTarget = transform.position;

                    anim.SetTrigger("Attack");
                    curFireDelay = 0;

                    StartCoroutine(AttackDelay());
                }
            }
            else if (currentState == characterState.idle)
            {
                Idle();
                anim.SetBool("Run", false);
                curFireDelay = 1f;
            }
        }
        if (canSkill)
        {
            E_Skill();
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
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            vecTarget = transform.position;
            anim.SetBool("Run", false);
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
                anim.SetBool("Run", false);
                vecTarget = transform.position;

                anim.SetTrigger("Attack");
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

            StartCoroutine(ChargingShot());
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

            StartCoroutine(WideShot());
        }
    }
    void E_Skill()
    {
        if (Input.GetKeyDown(KeyCode.E) && onESkill && gameObject.transform.tag == "MainCharacter")
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
            anim.SetBool("Run", false);
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
        else if (Input.GetKeyDown(KeyCode.E) && onESkill && gameObject.transform.tag == "SubCharacter")
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
            anim.SetBool("Run", false);
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
            vecTarget = transform.position;
        }
    }
    IEnumerator AttackDelay()
    {
        CharacterState.attackCheck = true;
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canDodge = true;
        canSkill = true;
        CharacterState.attackCheck = false;
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
        anim.SetTrigger("QSkill");
        // ��¡
        yield return new WaitForSeconds(1.4f);
        GameObject instantArrow = Instantiate(posionArrow, posionArrowPos.position, posionArrowPos.rotation);
        LeinaPosionArrow.speed = 0;
        // ��
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

        anim.SetBool("Run", false);
        vecTarget = transform.position;

        anim.SetTrigger("Attack");
        // ��
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
        anim.SetTrigger("QSkill");
        // ��¡
        LeinaSynergeSkill.speed = 0;
        //SynergeFirstArrow.SetActive(true);

        yield return new WaitForSeconds(0.1f);
        GameObject instantArrow = Instantiate(SynergeArrow, posionArrowPos.position, posionArrowPos.rotation);
        // ��
        yield return new WaitForSeconds(1f);
        Destroy(instantArrow);

        GameObject instantEffect = Instantiate(SynergeEffect, transform.position + -transform.right * 7f, transform.rotation);
        yield return new WaitForSeconds(1.0f);
    }
    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Enemy1Attack")
        //{
        //    if (GameManager.instance.mainPlayerHp > 0)
        //        GameManager.instance.mainPlayerHp -= Enemy1.damage;
        //}
        //if (collision.gameObject.tag == "Enemy2Attack")
        //{
        //    if (GameManager.instance.mainPlayerHp > 0)
        //        GameManager.instance.mainPlayerHp -= Enemy2.damage;
        //}
    }
}
