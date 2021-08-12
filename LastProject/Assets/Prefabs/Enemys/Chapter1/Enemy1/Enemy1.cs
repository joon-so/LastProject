using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] float spownDistance = 6f;
    [SerializeField] [Range(10f, 30f)] float detectDistance = 15f;
    [SerializeField] [Range(10f, 20f)] float shootDistance = 10f;
    [SerializeField] GameObject targetMark = null;
    [SerializeField] GameObject bullet = null;
    [SerializeField] GameObject explosion = null;
    [SerializeField] GameObject dropEffect = null;
    [SerializeField] Transform bulletStartPoint;
    [SerializeField] Transform bulletEndPoint;
    [SerializeField] AudioClip deadSound;

    protected List<GameObject> targets;

    public float shootCooltime = 3.0f;
    float chargingTime = 1.2f;
    float spinSpeed = 200.0f;

    bool shootable;
    bool movable;
    bool alive = true;
    bool born = false;
    bool drop = false;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;
    Animator anim;
    LineRenderer bulletLine;
    Vector3 startPoint;
    Rigidbody rigid;

    public static int damage = 30;

    // 체력
    public int maxHp = 200;
    public int currentHp;
    public HpBar hpBar;

    void Start()
    {
        // 모든 적은 로직이 비슷하기 때문에 같은 스크립트를 쓰지만
        // Layer나 Tag를 주어 데미지 등 고유 값만 바꾸어주자

        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        bulletLine = GetComponent<LineRenderer>();
        rigid = GetComponent<Rigidbody>();

        targets = GameObject.Find("Enemys").GetComponent<EnemyList>().Enemys;

        targetMark.SetActive(false);
        transform.position= new Vector3(transform.position.x, 45f, transform.position.z);
        rigid.useGravity = false;
        nav.enabled = false;

        bulletLine.SetVertexCount(2);
        bulletLine.enabled = false;

        InvokeRepeating("Find", 0f, 0.5f);
        startPoint = new Vector3(transform.position.x, 0f, transform.position.z);

        currentHp = maxHp;
        hpBar.SetMaxHp(maxHp);

        spownDistance = 5f;
        detectDistance = 15f;
        shootDistance = 10f;
        shootable = false;
        movable = false;
    }

    void FixedUpdate()
    {
        if (mainCharacter == null)
        {
            mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter");
        }
        if (currentHp <= 0 && alive)
        {
            alive = false;
            StartCoroutine(ExploseAndDistroy());
        }
        if (!born)
        {
            Waiting();
        }
        if (born)
        {
            Find();
        }
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (PlayerManager.instance.onTag)
                mainCharacter = GameObject.FindGameObjectWithTag("SubCharacter");
        }
    }

    void Waiting()
    {
        mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter");

        playerDistance = Vector3.Distance(
            new Vector3(mainCharacter.transform.position.x, 0, mainCharacter.transform.position.z),
            new Vector3(transform.position.x, 0, transform.position.z));

        if (playerDistance < spownDistance && !drop)
        {
            drop = true;
            StartCoroutine(DropAndExplosion());
        }
    }

    void Find()
    {
        mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter");

        if (mainCharacter == null)
        {
            return;
        }
        else
        {
            playerDistance = Vector3.Distance(mainCharacter.transform.position, transform.position);

            //Attack
            if (playerDistance < shootDistance)
            {
                if (shootable)
                    StartCoroutine(Attack());
                if (movable)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(mainCharacter.transform.position - transform.position);
                    Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
                    transform.rotation = Quaternion.Euler(0, euler.y, 0);
                }
                //nav.SetDestination(transform.position);
                nav.enabled = false;
                anim.SetBool("isAttack", true);

            }
            //Detect
            else if (playerDistance < detectDistance)
            {
                if (movable)
                {
                    nav.enabled = true;
                    nav.SetDestination(mainCharacter.transform.position);
                }
                anim.SetBool("isAttack", false);
                anim.SetBool("isDetected", true);
                anim.SetBool("isRun", true);
            }
            else
            {
                if (Vector3.Distance(startPoint, transform.position) < 0.1f)
                {
                    anim.SetBool("isRun", false);
                }
                else
                {
                    anim.SetBool("isRun", true);
                }
                anim.SetBool("isAttack", false);
                anim.SetBool("isDetected", false);

                if (movable)
                    nav.SetDestination(startPoint);
            }
        }
    }

    IEnumerator Attack()
    {
        // 총알 생성
        Instantiate(bullet, bulletStartPoint.position, bulletStartPoint.rotation);
        movable = false;
        shootable = false;
        anim.SetBool("isAttack", true);
        //조준선 표시
        bulletLine.SetPosition(0, bulletStartPoint.position);
        bulletLine.SetPosition(1, bulletEndPoint.position);
        bulletLine.enabled = true;
        //총알 차징
        yield return new WaitForSeconds(chargingTime);
        //조준선 제거 후 발사
        bulletLine.enabled = false;
        movable = true;
        //쿨타임 지난 후 
        yield return new WaitForSeconds(shootCooltime - chargingTime);
        shootable = true;
    }

    IEnumerator ExploseAndDistroy()
    {
        SoundManager.instance.SFXPlay("Explosion", deadSound);
        movable = false;
        shootable = false;
        Instantiate(explosion, transform.position, transform.rotation);
        bulletLine.enabled = false;
        targets.Remove(gameObject);
        GetComponent<TriangleExplosion>().ExplosionMesh();
        yield return new WaitForSeconds(1f);
    }

    IEnumerator DropAndExplosion()
    {
        targetMark.SetActive(true);
        targetMark.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        rigid.useGravity = true;
        yield return new WaitForSeconds(3f);
        Instantiate(dropEffect, transform.position, transform.rotation);
        targetMark.SetActive(false);
        this.transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        nav.enabled = true;
        born = true;
        movable = true;
        yield return new WaitForSeconds(1.9f);
        shootable = true;
    }

    public void HitJadeGrenade()
    {
        currentHp -= Jade.wSkillDamage;
        hpBar.SetHp(currentHp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "KarmenAttack")
        {
            currentHp -= Karmen.attackDamage;
            hpBar.SetHp(currentHp);
        }


    }

    void OnCollisionEnter(Collision collision)
    {
        // Karmen
        if (collision.gameObject.tag == "KarmenAttack")
        {
            currentHp -= Karmen.attackDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "KarmenQSkill")
        {
            currentHp -= Karmen.qSkillDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "KarmenWSkill")
        {
            currentHp -= Karmen.wSkillDamage;
            hpBar.SetHp(currentHp);
        }
        // Jade
        if (collision.gameObject.tag == "JadeAttack")
        {
            currentHp -= Jade.attackDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "JadeQSkill")
        {
            currentHp -= Jade.qSkillDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "JadeWSkill")
        {
            currentHp -= Jade.wSkillDamage;
            hpBar.SetHp(currentHp);
        }
        // Leina
        if (collision.gameObject.tag == "LeinaAttack")
        {
            currentHp -= Leina.attackDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "LeinaQSkill")
        {
            currentHp -= Leina.qSkillDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "LeinaWSkill")
        {
            currentHp -= Leina.wSkillDamage;
            hpBar.SetHp(currentHp);
        }
        // Eva
        if (collision.gameObject.tag == "EvaAttack")
        {
            currentHp -= Eva.attackDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "EvaQSkill")
        {
            currentHp -= Eva.qSkillDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "EvaWSkill")
        {
            currentHp -= Eva.wSkillDamage;
            hpBar.SetHp(currentHp);
        }
    }
}