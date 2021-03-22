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
    [SerializeField] Transform bulletPos;
    [SerializeField] GameObject bullet;

    public float moveSpeed = 5.0f;

    public float fireCoolTime = 0.5f;

    public float followDistance = 5.0f;

    public float dodgeCoolTime = 3.0f;
    float curDodgeCoolTime = 0;

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

        curDodgeCoolTime = dodgeCoolTime;
        StartCoroutine("DrawAssaultRifle");
        StartCoroutine("DrawAssaultRifle");
    }

    void Update()
    {
        if (endDodge)
        {
            Move();
            Stop();
            //Attack();
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

                GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
                Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = bulletPos.forward * 50;

                moveSpeed = 0f;
                anim.SetBool("isRun", false);
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

    void CoolTime()
    {
        // 회피
        if (curDodgeCoolTime < dodgeCoolTime)
        {
            curDodgeCoolTime += Time.deltaTime;
        }
        else
        {
            onDodge = true;
        }
    }
    void Q_Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine("ChangeRifleToMissile");
            StartCoroutine("ChangeRifleToMissile");
            // 스킬 


            // 스킬끝
            //StartCoroutine("ChangeMissileToRifle");
            //StartCoroutine("ChangeMissileToRifle");
        }
    }
    void W_Skill()
    {
        // 수류탄
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine("ShootGrenade");
            StopCoroutine("ShootGrenade");
        }
    }
    void E_Skill()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

        }
    }

    // 시작
    IEnumerator DrawAssaultRifle()
    {
        anim.SetTrigger("Idle");
        yield return new WaitForSeconds(0.5f);
        anim.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(0.5f);
        backAssaultRifle.SetActive(false);
        useAssaultRifle.SetActive(true);
    }
    // 무기 교체
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
    IEnumerator ShootGrenade()
    {
        anim.SetTrigger("shootGrenade");
        yield return new WaitForSeconds(3.0f);
    }


    // 태그시 초기화할 것들을 생각해보자 ex) 플레이어 이동속도, 클릭했던 좌표 등
}