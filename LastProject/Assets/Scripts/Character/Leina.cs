using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leina : MonoBehaviour
{
    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject arrow = null;
    [SerializeField] Transform arrowPos = null;

    public float moveSpeed = 30.0f;

    public float followDistance = 5.0f;

    public float dodgeCoolTime = 3.0f;
    float curDodgeCoolTime = 0;

    // 스킬
    public float qskillCoolTime = 5.0f;
    float curQSkillCoolTime = 0;
    bool onQSkill;


    bool canAttack;
    bool canDodge;
    bool canMove;

    Vector3 vecTarget;

    Animator anim;

    float distanceWithPlayer;
    GameObject tagCharacter;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        vecTarget = transform.position;
        canDodge = false;
        canAttack = true;
        canMove = true;

        curDodgeCoolTime = dodgeCoolTime;
        // 스킬
        onQSkill = true;
    }
    void Update()
    {
        if (gameObject.transform.tag == "MainCharacter")
        {
            if (canMove && canAttack)
            {
                Move();
                Stop();
            }
            if (canAttack)
            {
                Attack();
            }
            Dodge();
            AttackRange();
            CoolTime();

            Q_Skill();
            W_Skill();
            E_Skill();
        }
    }
    void Move()
    {
        if (Input.GetMouseButton(1))
        {
            moveSpeed = 30.0f;
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
    }
    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveSpeed = 0f;
            anim.SetBool("isRun", false);
            vecTarget = transform.position;
        }
    }
    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && curDodgeCoolTime >= dodgeCoolTime && canDodge)
        {
            curDodgeCoolTime = 0.0f;

            moveSpeed = 40.0f;
            anim.SetTrigger("doDodge");

            canDodge = false;
            canMove = false;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            vecTarget = transform.position;
            canDodge = false;
            canDodge = false;
            anim.SetBool("isRun", false);
        }
        else
        {
            canDodge = true;
            canMove = true;
        }
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }

            //GameObject instantBullet = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
            //Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
            //bulletRigid.velocity = arrowPos.forward * 80.0f;
            StartCoroutine(attack());

            //moveSpeed = 0f;
            //anim.SetBool("isRun", false);
            //anim.SetBool("doAttack", true);
            //vecTarget = transform.position;
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
        // 회피
        if (curDodgeCoolTime < dodgeCoolTime)
        {
            curDodgeCoolTime += Time.deltaTime;
        }
        else
        {
            canDodge = true;
        }

        // Q스킬
        if (curQSkillCoolTime < qskillCoolTime)
        {
            curQSkillCoolTime += Time.deltaTime;
        }
        else
        {
            onQSkill = true;
        }
    }
    void Q_Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetBool("isRun", false);
            if (onQSkill)
            {
                onQSkill = false;
                curQSkillCoolTime = 0;
                // 스킬 사용
                //StartCoroutine();
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {
            attackRange.SetActive(false);
        }
    }
    void W_Skill()
    {
        // 수류탄
        if (Input.GetKeyDown(KeyCode.W))
        {
            //anim.SetBool("Run", false);
            //vecTarget = transform.position;

            //anim.SetTrigger("");
            // 클릭?
        }
    }
    void E_Skill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }
    IEnumerator attack()
    {
        //발사
        moveSpeed = 0f;
        vecTarget = transform.position;
        canAttack = false;
        canMove = false;
        anim.SetBool("isRun", canMove);
        anim.SetBool("doAttack", !canAttack);
        yield return new WaitForSeconds(0.3f);
        GameObject instantBullet = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = arrowPos.forward * 80.0f;

        //모션끝날때까지 대기
        yield return new WaitForSeconds(0.4f);
        canAttack = true;
        anim.SetBool("doAttack", !canAttack);
        anim.SetBool("isRun", false);
        canMove = true;
        vecTarget = transform.position;
    }
}
