using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jade : MonoBehaviour
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

    public float moveSpeed = 30.0f;

    public float fireCoolTime = 0.5f;

    public float followDistance = 5.0f;

    public float dodgeCoolTime = 3.0f;
    float curDodgeCoolTime = 0;

    public float qskillCoolTime = 7.0f;
    float curQSkillCoolTime = 0;
    public float wskillCoolTime = 4.0f;
    float curWSkillCoolTime = 0;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;

    bool canMove;
    bool canAttack;
    bool canDodge;
    bool canSkill;

    bool doingDodge;

    Vector3 vecTarget;

    float fireDelay;

    Animator anim;
    Weapon weapon;

    float distanceWithPlayer;
    GameObject tagCharacter;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    void Start()
    {
        vecTarget = transform.position;
        onDodge = true;
        onQSkill = true;
        onWSkill = true;

        canAttack = false;
        canMove = false;
        canDodge = false;
        canSkill = false;

        doingDodge = false;

        curDodgeCoolTime = dodgeCoolTime;
        StartCoroutine(DrawAssaultRifle());
    }

    void Update()
    {
        if (gameObject.transform.tag == "MainCharacter")
        {
            fireDelay += Time.deltaTime;
            if (canMove)
                Move();
            if (canAttack)
                Attack();
            if (canDodge)
                Dodge();
            if (doingDodge)
            {
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                vecTarget = transform.position;
            }

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
    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //            canAttack = false;
            //            canMove = false;
            //            canSkill = false;

            //            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //            RaycastHit rayHit;
            //            if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
            //            {
            //                vecTarget = rayHit.point - transform.position;
            //                transform.LookAt(transform.position + vecTarget);
            //            }
            //            curDodgeCoolTime = 0.0f;

            //            moveSpeed *=2;

            //            transform.position = Vector3.MoveTowards(transform.position, vecTarget, moveSpeed * Time.deltaTime);
            ////            transform.position += transform.position + Vector3.forward * moveSpeed * Time.deltaTime;
            ////            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            //            vecTarget = transform.position;

            //            anim.SetTrigger("Dodge");
            //            StartCoroutine(DodgeDelay());
            StartCoroutine(dodge());
        }
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            canMove = false;
            canDodge = false;
            canSkill = false;

            if (fireDelay > fireCoolTime)
            {
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

                moveSpeed = 0f;
                anim.SetBool("Run", false);
                vecTarget = transform.position;

                anim.SetTrigger("shootAssaultRifle");
                fireDelay = 0;

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
        // ȸ��
        if (curDodgeCoolTime < dodgeCoolTime)
        {
            curDodgeCoolTime += Time.deltaTime;
        }
        else
        {
            canDodge = true;
            onDodge = true;
        }
        // Q��ų
        if (curQSkillCoolTime < qskillCoolTime)
        {
            curQSkillCoolTime += Time.deltaTime;
        }
        else
        {
            onQSkill = true;
        }
        // W��ų
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

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.2f);
        canMove = true;
        canDodge = true;
        canSkill = true;
    }
    IEnumerator dodge()
    {
        //0.9sec
        curDodgeCoolTime = 0.0f;
        anim.SetTrigger("Dodge");
        canMove = false;
        canAttack = false;
        canSkill = false;
        doingDodge = true;
        moveSpeed = 60.0f;
        yield return new WaitForSeconds(0.6f);
        moveSpeed = 18.0f;
        yield return new WaitForSeconds(0.25f);
        moveSpeed = 30.0f;
        canMove = true;
        canAttack = true;
        canSkill = true;
        doingDodge = false;
    }
    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(0.9f);
        canAttack = true;
        canMove = true;
        canSkill = true;
    }
    IEnumerator DrawAssaultRifle()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(0.5f);
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

            // �̻��� ����
            anim.SetTrigger("drawMissileLauncher");
            yield return new WaitForSeconds(0.5f);
            useAssaultRifle.SetActive(false);
            useMissileLauncher.SetActive(true);

            // �������
            anim.SetBool("AimMissile", true);
            yield return new WaitForSeconds(0.5f);
            missileEffect.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            anim.SetBool("AimMissile", false);
            missileEffect.SetActive(false);

            anim.SetTrigger("shootMissileLauncher");
            GameObject instantMissile = Instantiate(missileBullet, missileBulletPos.position, missileBulletPos.rotation);
            Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
            missileRigid.velocity = missileBulletPos.forward * 100;

            yield return new WaitForSeconds(1.0f);

            // ������ ����
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
            if (Vector3.Distance(nextVec, transform.position) > 80.0f)
                nextVec = nextVec.normalized * 80.0f;
            
            Debug.Log(nextVec);
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            GameObject instantGrenade = Instantiate(Grenade, grenadePos.position, grenadePos.rotation);
            Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
            rigidGrenade.AddForce(nextVec * 0.5f, ForceMode.Impulse);
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

    // �±׽� �ʱ�ȭ�� �͵��� �����غ��� ex) �÷��̾� �̵��ӵ�, Ŭ���ߴ� ��ǥ ��
}