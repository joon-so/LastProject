  using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossPilot : MonoBehaviour
{
    public static BossPilot instance;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject respownEnemy;
    [SerializeField] Transform bulletShootPos;

    Animator anim;
    Rigidbody rigid;
    NavMeshAgent nav;

    GameObject targetCharacter;

    protected List<GameObject> targets;

    bool canMove;
    bool canAttack;
    bool canDodge;
    bool canSkill;

    float playerDistance;
    float shootDistance;
    float detectDistance;
    float moveSpeed;
    float respownCooltime;
    float curRespownCooltime;

    ClientCollisionManager collisionManager;
    BossManager bossManager;

    void Start()
    {
        collisionManager = GameObject.Find("GameManager").GetComponent<ClientCollisionManager>();
        bossManager = GameObject.Find("BossManager").GetComponent<BossManager>();

        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();

        targets = GameObject.Find("Enemys").GetComponent<EnemyList>().Enemys;

        canMove = true;
        canAttack = true;
        canDodge = true;
        canSkill = true;

        shootDistance = 7f;
        respownCooltime = 15f;
        curRespownCooltime = 15f;
    }

    void FixedUpdate()
    {
        if (targetCharacter == null)
        {
            targetCharacter = GameObject.FindGameObjectWithTag("MainCharacter");
        }
        //1. ���� �ð����� �� ��ȯ
        //2. �⺻ ����
        if (curRespownCooltime <= respownCooltime)
        {
            curRespownCooltime += Time.deltaTime;
        }

        Pattern();

        //Change Target
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (PlayerManager.instance.onTag)
                targetCharacter = GameObject.FindGameObjectWithTag("SubCharacter");
        }
    }

    void Pattern()
    {
        if (targetCharacter == null)
        {
            return;
        }
        else
        {
            playerDistance = Vector3.Distance(targetCharacter.transform.position, transform.position);

            //Attack
            if (playerDistance < shootDistance)
            {
                Quaternion lookRotation = Quaternion.LookRotation(targetCharacter.transform.position - transform.position);
                Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, 100f * Time.deltaTime).eulerAngles;
                transform.rotation = Quaternion.Euler(0, euler.y, 0);

                anim.SetBool("Run", false);
                nav.enabled = false;
                if (canAttack)
                {
                    //pattern1 respown enemy
                    if (curRespownCooltime > respownCooltime)
                    {
                        StartCoroutine(RespownEnemy());
                    }
                    //pattern2 base attack
                    else
                    {
                        StartCoroutine(Attack());
                    }
                }
            }
            //Detect
            else
            {
                if (canMove)
                {
                    nav.enabled = true;
                    nav.SetDestination(targetCharacter.transform.position);
                    anim.SetBool("Run", true);
                }
            }
        }
    }
    IEnumerator RespownEnemy()
    {
        curRespownCooltime = 0f;
        canMove = false;
        canDodge = false;
        canAttack = false;
        canSkill = false;
        anim.SetTrigger("Respown");
        targets.Add(Instantiate(respownEnemy, transform.position + transform.forward * Random.Range(-2f, -1f) +
            transform.right * Random.Range(-2f, -1f) + transform.up * 45f, transform.rotation));
        yield return new WaitForSeconds(0.5f);

        targets.Add(Instantiate(respownEnemy, transform.position + transform.forward * Random.Range(1f, 2f) +
            transform.right * Random.Range(1f, 2f) + transform.up * 45f, transform.rotation));
        yield return new WaitForSeconds(0.5f);
        canMove = true;
        canDodge = true;
        canAttack = true;
        canSkill = true;
    }

    IEnumerator Attack()
    {
        // �Ѿ� ����
        canMove = false;
        canAttack = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        Instantiate(bullet, bulletShootPos.position, bulletShootPos.rotation);
        yield return new WaitForSeconds(0.3f);

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        Instantiate(bullet, bulletShootPos.position, bulletShootPos.rotation);
        //��Ÿ��
        yield return new WaitForSeconds(1f);
        canMove = true;
        canAttack = true;
    }

    public void HitJadeGrenade()
    {
        if (GameManager.instance.bossPage == 1)
            bossManager.curBoss1PageHp -= collisionManager.jadeWSkillDamage;
    }
    public void HitEvaQSkill()
    {
        if (GameManager.instance.bossPage == 1)
            bossManager.curBoss1PageHp -= collisionManager.evaQSkillDamage;
    }
    void OnCollisionEnter(Collision collision)
    {
        // Karmen
        if (collision.gameObject.tag == "KarmenAttack")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.karmenAttackDamage;
        }
        if (collision.gameObject.tag == "KarmenQSkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.karmenQSkillDamage;
        }
        if (collision.gameObject.tag == "KarmenWSkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.karmenWSkillDamage;
        }
        if (collision.gameObject.tag == "KarmenESkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.karmenESkillDamage;
        }
        // Jade
        if (collision.gameObject.tag == "JadeAttack")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.jadeAttackDamage;
        }
        if (collision.gameObject.tag == "JadeQSkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.jadeQSkillDamage;
        }
        if (collision.gameObject.tag == "JadeESkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.jadeESkillDamage;
        }
        // Leina
        if (collision.gameObject.tag == "LeinaAttack")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.leinaAttackDamage;
        }
        if (collision.gameObject.tag == "LeinaQSkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.leinaQSkillDamage;
        }
        if (collision.gameObject.tag == "LeinaWSkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.leinaWSkillDamage;
        }
        if (collision.gameObject.tag == "LeinaESkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.leinaESkillDamage;
        }
        // Eva
        if (collision.gameObject.tag == "EvaAttack")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.evaAttackDamage;
        }
        if (collision.gameObject.tag == "EvaQSkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.evaQSkillDamage;
        }
        if (collision.gameObject.tag == "EvaWSkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.evaWSkillDamage;
        }
        if (collision.gameObject.tag == "EvaESkill")
        {
            if (GameManager.instance.bossPage == 1)
                bossManager.curBoss1PageHp -= collisionManager.evaESkillDamage;
        }
    }
}