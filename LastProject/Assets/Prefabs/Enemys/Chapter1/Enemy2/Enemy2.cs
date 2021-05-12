using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    [SerializeField] [Range(0f, 10f)] float spownDistance = 5f;
    [SerializeField] [Range(10f, 30f)] float detectDistance = 15f;
    [SerializeField] [Range(10f, 20f)] float shootDistance = 10f;
    [SerializeField] GameObject targetMark = null;
    [SerializeField] GameObject bullet = null;
    [SerializeField] GameObject explosion = null;
    [SerializeField] GameObject dropEffect = null;
    [SerializeField] Transform bulletStartPoint;

    float straightTime = 0.2f;
    public float shootCooltime = 1.0f;
    public float spinSpeed = 50.0f;

    bool shootable = true;
    bool movable = true;
    bool alive = true;
    bool born = false;
    bool drop = false;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;
    Animator anim;
    Vector3 startPoint;
    Rigidbody rigid;

    // 체력
    public int maxHp = 200;
    public int currentHp;
    public HpBar hpBar;

    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();

        targetMark.SetActive(false);
        transform.position = new Vector3(transform.position.x, 45f, transform.position.z);
        rigid.useGravity = false;
        nav.enabled = false;

        InvokeRepeating("Find", 0f, 0.5f);
        startPoint = transform.position;

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
    }

    void Waiting()
    {
        mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter");

        playerDistance = Vector3.Distance(
            new Vector3(mainCharacter.transform.position.x, 0, mainCharacter.transform.position.z),
            new Vector3(transform.position.x, 0, transform.position.z));

        if (playerDistance < spownDistance && !drop)
        {
            StartCoroutine(DropAndExplosion());
            drop = true;
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
            if(playerDistance < shootDistance)
            {
                if (movable)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(mainCharacter.transform.position - transform.position);
                    Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
                    transform.rotation = Quaternion.Euler(0, euler.y, 0);
                }
                //nav.SetDestination(transform.position);
                nav.enabled = false;
                if (shootable)
                    StartCoroutine(Attack());
            }
            //Detect
            else if (playerDistance < detectDistance)
            {
                if (movable)
                {
                    nav.enabled = true;
                    nav.SetDestination(mainCharacter.transform.position);
                }

                //anim.SetBool("isAttack", false);
                //anim.SetBool("isDetected", true);
                //anim.SetBool("isRun", true);
            }
            else
            {
                if(Vector3.Distance(startPoint, transform.position) < 0.1f)
                {
                    //anim.SetBool("isRun", false);
                }
                else
                {
                // anim.SetBool("isRun", true);
                }
                //anim.SetBool("isAttack", false);
                //anim.SetBool("isDetected", false);

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
        //anim.SetBool("isAttack", true);
        //연속 2발
        yield return new WaitForSeconds(straightTime);
        Instantiate(bullet, bulletStartPoint.position, bulletStartPoint.rotation);
        //쿨타임
        movable = true;
        yield return new WaitForSeconds(shootCooltime);
        shootable = true;
    }

    IEnumerator ExploseAndDistroy()
    {
        shootable = false;
        movable = false;
        Instantiate(explosion, transform.position, transform.rotation);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
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
            Debug.Log(currentHp);
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