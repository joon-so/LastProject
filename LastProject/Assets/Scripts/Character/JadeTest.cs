using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JadeTest : MonoBehaviour
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

    public float moveSpeed = 5.0f;

    public float fireCoolTime = 0.5f;

    public float followDistance = 5.0f;

    public float dodgeCoolTime = 3.0f;
    float curDodgeCoolTime = 0;

    // ��ų
    public float qskillCoolTime = 5.0f;
    float curQSkillCoolTime = 0;
    bool onQSkill;


    bool onAttack;
    bool onDodge;
    bool endDodge;

    Vector3 vecTarget;

    bool onFire;

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
        endDodge = true;
        onAttack = true;

        curDodgeCoolTime = dodgeCoolTime;
        StartCoroutine("DrawAssaultRifle");
        StartCoroutine("DrawAssaultRifle");


        // ��ų
        onQSkill = true;
    }
    void Update()
    {
        if (endDodge)
        {
            Move();
            Stop();
            if (onAttack)
            {
                Attack();
            }
        }
        Dodge();
        AttackRange();
        CoolTime();

        Q_Skill();
        W_Skill();
        E_Skill();
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
        if (Input.GetKeyDown(KeyCode.Space) && curDodgeCoolTime >= dodgeCoolTime && onDodge)
        {
            curDodgeCoolTime = 0.0f;

            moveSpeed = 10.0f;
            anim.SetTrigger("Dodge");

            onDodge = false;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("DodgeForward"))
        {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            vecTarget = transform.position;
            endDodge = false;
        }
    }
    void Attack()
    {
        fireDelay += Time.deltaTime;
        //onFire = weapon.shotSpeed < fireDelay;

        if (Input.GetMouseButtonDown(0))
        {
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
                bulletRigid.velocity = assaultRifleBulletPos.forward * 50;

                moveSpeed = 0f;
                anim.SetBool("Run", false);
                vecTarget = transform.position;

                anim.SetTrigger("shootAssaultRifle");
                fireDelay = 0;
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
    void Missile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 2;
            transform.LookAt(transform.position + nextVec);

            GameObject instantMissile = Instantiate(missileBullet, missileBulletPos.position, missileBulletPos.rotation);
            Rigidbody rigidMissile = instantMissile.GetComponent<Rigidbody>();
            rigidMissile.velocity = missileBulletPos.forward * 50;
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
    }
    void Q_Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            anim.SetBool("Run", false);
            if (onQSkill)
            {
                //onAttack = false;
                // ��ư�� ������ ���� ǥ��
                //attackRange.SetActive(true);
                //missileRange.SetActive(true);

                //missileRange.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                // ���� Ŭ���ϸ�


                onQSkill = false;
                curQSkillCoolTime = 0;
                // ��ų ���
                StartCoroutine("ShootMissile");
            }
        }
        else if (Input.GetKeyUp(KeyCode.Q))
        {

            attackRange.SetActive(false);
        }
    }
    void W_Skill()
    {
        // ����ź
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetBool("Run", false);
            vecTarget = transform.position;

            anim.SetTrigger("shootGrenade");
            // Ŭ��?
    //        StartCoroutine("ShootGrenade");

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if (Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 2;
                transform.LookAt(transform.position + nextVec);

                GameObject instantGrenade = Instantiate(Grenade, grenadePos.position, grenadePos.rotation);
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);
            }
 //           StopCoroutine("ShootGrenade");
        }
    }
    void E_Skill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }

    IEnumerator DrawAssaultRifle()
    {
        anim.SetTrigger("Idle");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(0.5f);
        backAssaultRifle.SetActive(false);
        useAssaultRifle.SetActive(true);
    }
    IEnumerator ChangeRifleToMissile()
    {
        anim.SetTrigger("drawMissileLauncher");
        yield return new WaitForSeconds(0.5f);
        useAssaultRifle.SetActive(false);
        useMissileLauncher.SetActive(true);
    }
    IEnumerator ChangeMissileToRifle()
    {
        anim.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(0.5f);
        useMissileLauncher.SetActive(false);
        useAssaultRifle.SetActive(true);
    }
    IEnumerator ShootMissile()
    {
        vecTarget = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, 100))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 2;
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
            Rigidbody rigidMissile = instantMissile.GetComponent<Rigidbody>();
            rigidMissile.velocity = missileBulletPos.forward * 50;
        
            yield return new WaitForSeconds(1.0f);

            // ������ ����
            anim.SetTrigger("drawAssaultRifle");
            yield return new WaitForSeconds(0.5f);
            useMissileLauncher.SetActive(false);
            useAssaultRifle.SetActive(true);
            onAttack = true;
        }
    }
    IEnumerator ShootGrenade()
    {
        anim.SetTrigger("shootGrenade");
        yield return new WaitForSeconds(3.0f);
    }



    // �±׽� �ʱ�ȭ�� �͵��� �����غ��� ex) �÷��̾� �̵��ӵ�, Ŭ���ߴ� ��ǥ ��
}