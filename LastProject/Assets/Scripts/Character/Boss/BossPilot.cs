using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossPilot : MonoBehaviour
{
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

    void Start()
    {
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
        if(targetCharacter == null)
        {
            targetCharacter = GameObject.FindGameObjectWithTag("MainCharacter");
        }
        //1. 일정 시간마다 적 소환
        //2. 기본 공격
        if(curRespownCooltime <= respownCooltime)
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
        // 총알 생성
        canMove = false;
        canAttack = false;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        Instantiate(bullet, bulletShootPos.position, bulletShootPos.rotation);
        yield return new WaitForSeconds(0.3f);

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        Instantiate(bullet, bulletShootPos.position, bulletShootPos.rotation);
        //쿨타임
        yield return new WaitForSeconds(1f);
        canMove = true;
        canAttack = true;
    }
}
