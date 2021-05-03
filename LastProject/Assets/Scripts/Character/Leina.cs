using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leina : MonoBehaviour
{
    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject arrow = null;
    [SerializeField] Transform arrowPos = null;

    [SerializeField] GameObject chargingEffect = null;
    [SerializeField] GameObject posionArrow = null;
    [SerializeField] GameObject posionArrowObj = null;
    [SerializeField] Transform chargingShotPos = null;

    public float moveSpeed = 30.0f;
    public float followDistance = 5.0f;

    public float fireDelay = 1.0f;
    float curfireDelay;

    public float dodgeCoolTime = 3.0f;
    float curDodgeCoolTime = 0;

    public float qskillCoolTime = 5.0f;
    float curQSkillCoolTime = 0;

    public float wskillCoolTime = 5.0f;
    float curWSkillCoolTime = 0;

    bool onQSkill;
    bool onWSkill;

    bool canAttack;
    bool canDodge;
    bool canMove;
    bool canSkill;

    bool onDodge;

    float curScale;
    float maxScale;

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
        onQSkill = true;
        onWSkill = true;
        onDodge = true;
        
        canAttack = true;
        canDodge = true;
        canMove = true;
        canSkill = true;

        curfireDelay = 1.0f;
        curDodgeCoolTime = dodgeCoolTime;
        curScale = 1;
        maxScale = 5;
    }
    void Update()
    {
        if (gameObject.transform.tag == "MainCharacter")
        {
            curfireDelay += Time.deltaTime;
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
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canMove = false;
            canDodge = false;
            canSkill = false;

            if (curfireDelay > fireDelay)
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
                curfireDelay = 0;

                StartCoroutine(AttackDelay());
            }
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
            transform.Translate(Vector3.forward * 40 * Time.deltaTime);
            vecTarget = transform.position;
            anim.SetBool("Run", false);
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
        // È¸ÇÇ
        if (curDodgeCoolTime < dodgeCoolTime)
        {
            curDodgeCoolTime += Time.deltaTime;
        }
        else
        {
            onDodge = true;
        }
        // Q½ºÅ³
        if (curQSkillCoolTime < qskillCoolTime)
        {
            curQSkillCoolTime += Time.deltaTime;
        }
        else
        {
            onQSkill = true;
        }
        // W½ºÅ³
        if (curWSkillCoolTime < wskillCoolTime)
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
        canAttack = true;
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
            //posionArrowObj.SetActive(true);
            //while (curScale < maxScale)
            //{
            //    posionArrowObj.transform.localScale = new Vector3(curScale, curScale, curScale);
            //    curScale += 0.1f;
            //    Debug.Log(curScale);
            //}
            //posionArrowObj.SetActive(false);
            GameObject instantArrow = Instantiate(posionArrow, chargingShotPos.position, chargingShotPos.rotation);
            // ¼¦
            yield return new WaitForSeconds(2.3f);
            
            Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
            arrowRigid.velocity = chargingShotPos.forward * 200;
            chargingEffect.SetActive(false);
        }
        yield return new WaitForSeconds(1);
        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
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
