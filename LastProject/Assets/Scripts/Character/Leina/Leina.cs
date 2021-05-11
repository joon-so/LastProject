using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Leina : MonoBehaviour
{
    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject arrow = null;
    [SerializeField] Transform arrowPos = null;

    [SerializeField] GameObject chargingEffect = null;
    [SerializeField] Transform chargingShotPos = null;
    [SerializeField] GameObject posionArrow = null;
    [SerializeField] Transform posionArrowPos = null;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float qSkillCoolTime = 5.0f;
    public float wSkillCoolTime = 5.0f;
    public float fireDelay = 1.0f;
    public float followDistance = 5.0f;

    public static int attackDamage = 20;
    public static int qSkillDamage = 70;
    public static int wSkillDamage = 30;

    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;

    float curFireDelay;

    bool canAttack;
    bool canDodge;
    bool canMove;
    bool canSkill;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;

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

        curFireDelay = fireDelay;
    }
    void Update()
    {
        if (gameObject.transform.tag == "MainCharacter")
        {
            curFireDelay += Time.deltaTime;
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
            canMove = false;
            canDodge = false;
            canSkill = false;

            if (curFireDelay > fireDelay)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 nextVec = hit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }

                GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
                Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
                arrowRigid.velocity = arrowPos.forward;

                moveSpeed = 0f;
                anim.SetBool("Run", false);
                vecTarget = transform.position;

                anim.SetTrigger("shotArrow");
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

            StartCoroutine(ChargingShot());
        }
    }
    void W_Skill()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            onWSkill = false;
            curWSkillCoolTime = 0;
            anim.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            StartCoroutine(ContinuityShot());
        }
    }
    void E_Skill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }
    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canDodge = true;
        canSkill = true;
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

            anim.SetTrigger("chargingShot");
            // Â÷Â¡
            yield return new WaitForSeconds(3.0f);
            chargingEffect.SetActive(true);
            GameObject instantArrow = Instantiate(posionArrow, posionArrowPos.position, posionArrowPos.rotation);
            LeinaPosionArrow.speed = 0;
            // ¼¦
            yield return new WaitForSeconds(2.3f);
            chargingEffect.SetActive(false);

            Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
            arrowRigid.velocity = chargingShotPos.forward;
            LeinaPosionArrow.speed = 40;
        }
        yield return new WaitForSeconds(1.0f);

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }

    IEnumerator ContinuityShot()
    {
        yield return new WaitForSeconds(1);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy1Attack")
        {
            if (GameManager.instance.mainPlayerHp > 0)
                GameManager.instance.mainPlayerHp -= Enemy1.damage;
        }
    }
}
