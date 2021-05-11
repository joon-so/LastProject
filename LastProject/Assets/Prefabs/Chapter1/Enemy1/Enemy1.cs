using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] [Range(30f, 100f)] float detectDistance;
    [SerializeField] [Range(30f, 100f)] float shootDistance = 30f;
    [SerializeField] GameObject bullet = null;
    [SerializeField] GameObject explosion = null;
    [SerializeField] Transform bulletStartPoint;
    [SerializeField] Transform bulletEndPoint;

    public float shootCooltime = 3.0f;
    float chargingTime = 1.2f;
    float spinSpeed = 200.0f;

    bool shootable = true;
    bool movable = true;
    bool alive = true;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;
    Animator anim;
    LineRenderer bulletLine;
    Vector3 startPoint;
    Rigidbody rigid;

    public static int damage = 20;

    // ü��
    public int maxHp = 200;
    public int currentHp;
    public HpBar hpBar;

    void Start()
    {
        // ��� ���� ������ ����ϱ� ������ ���� ��ũ��Ʈ�� ������
        // Layer�� Tag�� �־� ������ �� ���� ���� �ٲپ�����


        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        bulletLine = GetComponent<LineRenderer>();

        bulletLine.SetVertexCount(2);
        bulletLine.enabled = false;

        InvokeRepeating("Find", 0f, 0.5f);
        startPoint = transform.position;

        currentHp = maxHp;
        hpBar.SetMaxHp(maxHp);
    }

    void FixedUpdate()
    {
        if (currentHp <= 0 && alive)
        {
            alive = false;
            StartCoroutine(ExploseAndDistroy());
        }
        Find();
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
                nav.SetDestination(transform.position);
                anim.SetBool("isAttack", true);

            }
            //Detect
            else if (playerDistance < detectDistance)
            {
                if (movable)
                    nav.SetDestination(mainCharacter.transform.position);

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
        // �Ѿ� ����
        Instantiate(bullet, bulletStartPoint.position, bulletStartPoint.rotation);
        movable = false;
        shootable = false;
        anim.SetBool("isAttack", true);
        //���ؼ� ǥ��
        bulletLine.SetPosition(0, bulletStartPoint.position);
        bulletLine.SetPosition(1, bulletEndPoint.position);
        bulletLine.enabled = true;
        //�Ѿ� ��¡
        yield return new WaitForSeconds(chargingTime);
        //���ؼ� ���� �� �߻�
        bulletLine.enabled = false;
        movable = true;
        //��Ÿ�� ���� �� 
        yield return new WaitForSeconds(shootCooltime - chargingTime);
        shootable = true;
    }

    IEnumerator ExploseAndDistroy()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        shootable = false;
        movable = false;
        bulletLine.enabled = false;
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
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