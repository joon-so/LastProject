using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ServerMyJade : ServerSubAIManager
{
    [SerializeField] GameObject useAssaultRifle;
    [SerializeField] GameObject backAssaultRifle;

    [SerializeField] Transform assaultRifleBulletPos;
    [SerializeField] GameObject assaultRifleBullet;

    [SerializeField] GameObject useMissileLauncher;
    [SerializeField] GameObject backMissileLauncher;

    [SerializeField] Transform missileBulletPos;
    [SerializeField] GameObject missileBullet;
    [SerializeField] GameObject missileRange;
    [SerializeField] GameObject missileEffect;

    [SerializeField] Transform grenadePos;
    [SerializeField] GameObject Grenade;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float fireDelay = 0.8f;
    public float subFireDelay = 1.5f;
    public float followDistance = 5.0f;
    public float grenadeDistance = 10.0f;

    public static float qSkillCoolTime = 5.0f;
    public static float wSkillCoolTime = 5.0f;

    float curFireDelay;
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

    Vector3 vecTarget;
    Animator myAnimator;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
    }

    void Start()
    {

        vecTarget = transform.position;
        curDodgeCoolTime = dodgeCoolTime;
        curQSkillCoolTime = qSkillCoolTime;
        curWSkillCoolTime = wSkillCoolTime;

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
            }
            Stop();
            CoolTime();
        }
        else if (gameObject.transform.tag == "SubCharacter")
        {

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

            moveSpeed *= 2;
            myAnimator.SetTrigger("Dodge");
            ServerLoginManager.playerList[0].mainCharacterBehavior = 2;

            StartCoroutine(DodgeDelay());

        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("DodgeForward"))
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            vecTarget = transform.position;
            myAnimator.SetBool("Run", false);
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

                GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
                Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = assaultRifleBulletPos.forward;

                myAnimator.SetBool("Run", false);
                vecTarget = transform.position;

                myAnimator.SetTrigger("shootAssaultRifle");
                ServerLoginManager.playerList[0].mainCharacterBehavior = 3;
                curFireDelay = 0;

                StartCoroutine(AttackDelay());
            }
        }
        if (Input.GetMouseButtonUp(0))
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }
    void CoolTime()
    {
        if (curDodgeCoolTime < dodgeCoolTime)
            curDodgeCoolTime += Time.deltaTime;
        else
            onDodge = true;
        if (curQSkillCoolTime < qSkillCoolTime)
            curQSkillCoolTime += Time.deltaTime;
        else
            onQSkill = true;
        if (curWSkillCoolTime < wSkillCoolTime)
            curWSkillCoolTime += Time.deltaTime;
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
            myAnimator.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            StartCoroutine(ShootGrenade());
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
        yield return new WaitForSeconds(0.3f);
        canMove = true;
        canDodge = true;
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
        canMove = true;
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    IEnumerator DrawAssaultRifle()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(1.0f);
        backAssaultRifle.SetActive(false);
        useAssaultRifle.SetActive(true);
        canMove = true;
        canAttack = true;
        canDodge = true;
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    IEnumerator ShootMissile()
    {
        vecTarget = transform.position;

        ServerLoginManager.playerList[0].mainCharacterBehavior = 4;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            myAnimator.SetTrigger("drawMissileLauncher");
            yield return new WaitForSeconds(0.5f);
            useAssaultRifle.SetActive(false);
            useMissileLauncher.SetActive(true);

            myAnimator.SetBool("AimMissile", true);
            yield return new WaitForSeconds(0.5f);
            missileEffect.SetActive(true);
            //SoundManager.instance.SFXPlay("Attack", qSkillClip);

            yield return new WaitForSeconds(1.0f);
            myAnimator.SetBool("AimMissile", false);
            missileEffect.SetActive(false);

            myAnimator.SetTrigger("shootMissileLauncher");
            GameObject instantMissile = Instantiate(missileBullet, missileBulletPos.position, missileBulletPos.rotation);
            Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
            missileRigid.velocity = missileBulletPos.forward;

            yield return new WaitForSeconds(1.0f);

            myAnimator.SetTrigger("drawAssaultRifle");
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
        ServerLoginManager.playerList[0].mainCharacterBehavior = 5;

        myAnimator.SetTrigger("shootGrenade");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            if (Vector3.Distance(nextVec, transform.position) > 10.0f)
                nextVec = nextVec.normalized * 10.0f;

            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            //SoundManager.instance.SFXPlay("Grenade", wSkillClip);
            
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
}