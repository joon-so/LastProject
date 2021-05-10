using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Karmen : MonoBehaviour
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
    public float qSkillCoolTime = 5.0f;
    public float wSkillCoolTime = 5.0f;
    public float followDistance = 5.0f;

    public static int attackDamage = 20;
    public static int qSkillDamage = 60;
    public static int wSkillDamage = 10;

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

    bool canCombo;
    int comboStep;

    float distanceWithPlayer;

    Vector3 vecTarget;

    Animator anim;
    NavMeshAgent nav;
    GameObject tagCharacter;

    void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        vecTarget = transform.position;

        curDodgeCoolTime = 0;
        curQSkillCoolTime = 0;
        curWSkillCoolTime = 0;

        canMove = false;
        canDodge = false;
        canAttack = false;
        canSkill = false;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;

        doingAttack = false;
        motionEndCheck = true;
        comboContinue = true;

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
           // Follow();
        }
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

        if (doingAttack)
        {
            anim.SetBool("Run", false);
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
        if (Input.GetMouseButtonDown(0))
        {
            canMove = false;
            canSkill = false;

            doingAttack = true;


            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
            vecTarget = transform.position;
            anim.SetBool("Attack", true);

            if(comboStep == 0)
            {
                anim.Play("attack2SS");
                comboStep = 1;
                return;
            }
            if(comboStep!=0)
            {
                if (canCombo)
                {
                    canCombo = false;
                    comboStep += 1;
                }
            }


            //if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
            //    && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
            //{
            //        if (comboContinue)
            //            comboContinue = false;
            //    motionEndCheck = false;
            //}


        }


        //if (doingAttack)
        //{
        //    if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
        //        && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.7f)
        //    {
        //        if (Input.GetMouseButtonDown(0))
        //            if (comboContinue)
        //                comboContinue = false;
        //        motionEndCheck = false;
        //    }
        //    else if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1f && !motionEndCheck)
        //    {
        //        if (!comboContinue)
        //        {
        //            anim.SetTrigger("nextCombo");
        //            comboContinue = true;
        //        }
        //        else if (comboContinue)
        //        {
        //            doingAttack = false;
        //            anim.SetBool("Attack", false);
        //        }
        //        motionEndCheck = true;
        //    }
        //}
        //if (Input.GetMouseButtonDown(0))
        //{
        //    canMove = false;
        //    canDodge = false;
        //    canSkill = false;
        //    anim.SetBool("Run", false);

        //    if ((doingAttack && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
        //         && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
        //         || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle1SS")
        //         || anim.GetCurrentAnimatorStateInfo(0).IsName("runSS"))
        //    {
        //        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        RaycastHit hit;
        //        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //        {
        //            Vector3 nextVec = hit.point - transform.position;
        //            nextVec.y = 0;
        //            transform.LookAt(transform.position + nextVec);
        //        }
        //        vecTarget = transform.position;
        //    }

        //    moveSpeed = 0f;
        //    doingAttack = true;
        //    anim.SetBool("Attack", true);
        //}

        //if (Input.GetMouseButtonDown(1) && doingAttack)
        //{
        //    anim.SetBool("Attack", false);
        //    //canMove = true;
        //    //canDodge = true;
        //    //canSkill = true;
        //    StartCoroutine(AttackDelay());
        //}
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
    void Follow()
    {
        //distanceWithPlayer = Vector3.Distance(tagCharacter.transform.position, transform.position);

        //if (distanceWithPlayer > followDistance)
        //{
        //    nav.SetDestination(tagCharacter.transform.position);
        //    anim.SetBool("Run", true);
        //}
        //else
        //{
        //    nav.SetDestination(transform.position);
        //    anim.SetBool("Run", false);
        //}
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy1Bullet")
        {
            Enemy1Bullet enemy1bullet = collision.gameObject.GetComponent<Enemy1Bullet>();
            if (GameManager.instance.mainPlayerHp > 0)
            {
                GameManager.instance.mainPlayerHp -= enemy1bullet.damage;
            }
        }
        if (collision.gameObject.tag == "Enemy2Bullet")
        {
            Enemy2Bullet enemy2bullet = collision.gameObject.GetComponent<Enemy2Bullet>();
            if (GameManager.instance.mainPlayerHp > 0)
            {
                GameManager.instance.mainPlayerHp -= enemy2bullet.damage;
            }
        }
    }
}