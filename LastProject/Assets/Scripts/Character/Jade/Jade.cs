using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jade : SubAI
{
    [SerializeField] GameObject attackRange = null;
    [SerializeField] GameObject useAssaultRifle = null;
    [SerializeField] GameObject useMissileLauncher = null;
    [SerializeField] GameObject backAssaultRifle = null;
    [SerializeField] GameObject backMissileLauncher = null;

    [SerializeField] Transform assaultRifleBulletPos = null;
    [SerializeField] GameObject assaultRifleBullet = null;

    [SerializeField] Transform missileBulletPos = null;
    [SerializeField] GameObject missileBullet = null;
    [SerializeField] GameObject missileRange = null;
    [SerializeField] GameObject missileEffect = null;

    [SerializeField] Transform grenadePos = null;
    [SerializeField] GameObject Grenade = null;

    public AudioClip attackClip;
    public AudioClip qSkillClip;
    public AudioClip wSkillClip;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float fireDelay = 0.5f;
    public float subFireDelay = 1.5f;
    public float followDistance = 5.0f;
    public float grenadeDistance = 10.0f;

    public static float qSkillCoolTime = 5.0f;
    public static float wSkillCoolTime = 5.0f;
    public static float eSkillCoolTime = 5.0f;
    public static int attackDamage = 20;
    public static int qSkillDamage = 70;
    public static int wSkillDamage = 50;


    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;

    float curFireDelay;

    bool canMove;
    bool canDodge;
    bool canAttack;
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
        if (GameManager.instance.isMainJade)
        {
            nav.enabled = false;
            tagCharacter = GameManager.instance.character2;
        }
        else if (GameManager.instance.isSubJade)
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
        
        canMove = false;
        canDodge = false;
        canAttack = false;
        canSkill = false;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;

        curFireDelay = fireDelay;

        StartCoroutine(DrawAssaultRifle());
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
                    GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
                    Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                    bulletRigid.velocity = assaultRifleBulletPos.forward;
                    
                    moveSpeed = 0f;
                    anim.SetBool("Run", false);
                    vecTarget = transform.position;

                    anim.SetTrigger("shootAssaultRifle");
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
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("DodgeForward"))
        {
            transform.Translate(Vector3.forward *5* Time.deltaTime);
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

                GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
                Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = assaultRifleBulletPos.forward;


                moveSpeed = 0f;
                anim.SetBool("Run", false);
                vecTarget = transform.position;

                anim.SetTrigger("shootAssaultRifle");
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

            StartCoroutine(ShootMissile());
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

            StartCoroutine(ShootGrenade());
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
        if(Input.GetKeyDown(KeyCode.F))
        {
            vecTarget = transform.position;
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.2f);
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
    IEnumerator DrawAssaultRifle()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(1.0f);
        backAssaultRifle.SetActive(false);
        useAssaultRifle.SetActive(true);
        canMove = true;
        canAttack = true;
        canDodge = true;
        canSkill = true;
    }
    IEnumerator ShootMissile()
    {
        vecTarget = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            // 미사일 장착
            anim.SetTrigger("drawMissileLauncher");
            yield return new WaitForSeconds(0.5f);
            useAssaultRifle.SetActive(false);
            useMissileLauncher.SetActive(true);

            // 기모으기
            anim.SetBool("AimMissile", true);
            yield return new WaitForSeconds(0.5f);
            missileEffect.SetActive(true);
            SoundManager.instance.SFXPlay("Attack", qSkillClip);

            yield return new WaitForSeconds(1.0f);
            anim.SetBool("AimMissile", false);
            missileEffect.SetActive(false);

            anim.SetTrigger("shootMissileLauncher");
            GameObject instantMissile = Instantiate(missileBullet, missileBulletPos.position, missileBulletPos.rotation);
            Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
            missileRigid.velocity = missileBulletPos.forward;

            yield return new WaitForSeconds(1.0f);

            // 라이플 장착
            anim.SetTrigger("drawAssaultRifle");
            yield return new WaitForSeconds(0.5f);
            useMissileLauncher.SetActive(false);
            useAssaultRifle.SetActive(true);

            yield return new WaitForSeconds(0.3f);
            canAttack = true;
            canMove = true;
            canDodge = true;
            canSkill = true;
        }
    }
    IEnumerator ShootGrenade()
    {
        vecTarget = transform.position;
        anim.SetTrigger("shootGrenade");
     
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            if (Vector3.Distance(nextVec, transform.position) > 10.0f)
                nextVec = nextVec.normalized * 10.0f;
            
            Debug.Log(nextVec);
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            SoundManager.instance.SFXPlay("Grenade", wSkillClip);
            GameObject instantGrenade = Instantiate(Grenade, grenadePos.position, grenadePos.rotation);
            Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
            rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
            rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.3f);
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