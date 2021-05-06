using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Eva : MonoBehaviour
{
    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject qSkill = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 5.0f;
    public float qSkillCoolTime = 10.0f;
    public float wSkillCoolTime = 5.0f;
    public float followDistance = 5.0f;

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
    
    bool motionEndCheck;
    bool comboContinue;
    
    float distanceWithPlayer;

    Vector3 vecTarget;

    Animator anim;
    NavMeshAgent nav;
    GameObject tagCharacter;

    void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
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
            qSkill.SetActive(false);
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
        //if (doAttack)
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
        //            doAttack = false;
        //            anim.SetBool("doAttack", doAttack);

        //        }
        //        motionEndCheck = true;
        //    }
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    isRun = false;
        //    anim.SetBool("isRun", isRun);

        //    if ((doAttack && anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f
        //         && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.8f)
        //         || anim.GetCurrentAnimatorStateInfo(0).IsName("Idle_01")
        //         || anim.GetCurrentAnimatorStateInfo(0).IsName("Run"))
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
        //    doAttack = true;
        //    anim.SetBool("doAttack", doAttack);
        //}

        //if (doAttack && Input.GetMouseButtonDown(1))
        //{
        //    doAttack = false;
        //    anim.SetBool("doAttack", doAttack);
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

            StartCoroutine(FireGun());
        }
    }

    void W_Skill()
    {

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

    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
        canMove = true;
        canSkill = true;
    }

    IEnumerator FireGun()
    {
        qSkill.SetActive(true);
        yield return new WaitForSeconds(3.3f);
        qSkill.SetActive(false);
    }
}