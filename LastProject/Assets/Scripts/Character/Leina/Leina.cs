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
    [SerializeField] Transform posionArrowPos = null;

    [SerializeField] GameObject rainArrow = null;
    [SerializeField] GameObject effect = null;
    [SerializeField] Transform rainArrowPos = null;

    public AudioClip attackClip;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float qSkillCoolTime = 5.0f;
    public float wSkillCoolTime = 5.0f;
    public float fireDelay = 1.0f;
    public float subFireDelay = 1.5f;
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

    void Awake()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        if (GameManager.instance.isMainLeina)
        {
            nav.enabled = false;
            tagCharacter = GameManager.instance.character2;
        }
        else if (GameManager.instance.isSubLeina)
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
                MainCharacterTrace();
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
        if (Input.GetKeyDown(KeyCode.W) && onWSkill)
        {
            onWSkill = false;
            curWSkillCoolTime = 0;
            anim.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            StartCoroutine(RainShot());
        }
    }
    void E_Skill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

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
        }
        anim.SetTrigger("QSkill");
        // Â÷Â¡
        yield return new WaitForSeconds(3.0f);
        GameObject instantArrow = Instantiate(posionArrow, posionArrowPos.position, posionArrowPos.rotation);
        LeinaPosionArrow.speed = 0;
        // ¼¦
        yield return new WaitForSeconds(2.3f);

        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = posionArrowPos.forward;
        LeinaPosionArrow.speed = 40;
        yield return new WaitForSeconds(1.0f);

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }

    IEnumerator RainShot()
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
        anim.SetTrigger("WSkill");
        yield return new WaitForSeconds(1.5f);
        effect.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        // ¼¦
        GameObject instantArrow = Instantiate(rainArrow, rainArrowPos.position, rainArrowPos.rotation);
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = posionArrowPos.forward;

        GameObject instantArrow2 = Instantiate(rainArrow, rainArrowPos.position + new Vector3(2, 0, 0), rainArrowPos.rotation);
        Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
        arrowRigid2.velocity = posionArrowPos.forward;

        GameObject instantArrow3 = Instantiate(rainArrow, rainArrowPos.position + new Vector3(-2, 0, 0), rainArrowPos.rotation);
        Rigidbody arrowRigid3 = instantArrow3.GetComponent<Rigidbody>();
        arrowRigid3.velocity = posionArrowPos.forward;

        GameObject instantArrow4 = Instantiate(rainArrow, rainArrowPos.position + new Vector3(0, 0, 2), rainArrowPos.rotation);
        Rigidbody arrowRigid4 = instantArrow4.GetComponent<Rigidbody>();
        arrowRigid4.velocity = posionArrowPos.forward;

        GameObject instantArrow5 = Instantiate(rainArrow, rainArrowPos.position + new Vector3(0, 0, -2), rainArrowPos.rotation);
        Rigidbody arrowRigid5 = instantArrow5.GetComponent<Rigidbody>();
        arrowRigid5.velocity = posionArrowPos.forward;

        GameObject instantArrow6 = Instantiate(rainArrow, rainArrowPos.position + new Vector3(1, 0, 1), rainArrowPos.rotation);
        Rigidbody arrowRigid6 = instantArrow5.GetComponent<Rigidbody>();
        arrowRigid6.velocity = posionArrowPos.forward;

        GameObject instantArrow7 = Instantiate(rainArrow, rainArrowPos.position + new Vector3(-1, 0, 1), rainArrowPos.rotation);
        Rigidbody arrowRigid7 = instantArrow5.GetComponent<Rigidbody>();
        arrowRigid7.velocity = posionArrowPos.forward;

        GameObject instantArrow8 = Instantiate(rainArrow, rainArrowPos.position + new Vector3(1, 0, -1), rainArrowPos.rotation);
        Rigidbody arrowRigid8 = instantArrow5.GetComponent<Rigidbody>();
        arrowRigid8.velocity = posionArrowPos.forward;
        GameObject instantArrow9 = Instantiate(rainArrow, rainArrowPos.position + new Vector3(-1, 0, -1), rainArrowPos.rotation);
        Rigidbody arrowRigid9 = instantArrow5.GetComponent<Rigidbody>();
        arrowRigid9.velocity = posionArrowPos.forward;

        yield return new WaitForSeconds(1.0f);
        effect.SetActive(false);

    
        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
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
