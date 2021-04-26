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

    public int maxHp = 200;
    public int currentHp;

    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        bulletLine = GetComponent<LineRenderer>();

        bulletLine.SetVertexCount(2);
        bulletLine.enabled = false;

        InvokeRepeating("Find", 0f, 0.5f);
        startPoint = transform.position;

        currentHp = maxHp;
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
            if(playerDistance < shootDistance)
            {
                if (movable)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(mainCharacter.transform.position - transform.position);
                    Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
                    transform.rotation = Quaternion.Euler(0, euler.y, 0);
                }
                nav.SetDestination(transform.position);
                anim.SetBool("isAttack", true);

                if (shootable)
                    StartCoroutine(Attack());
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
                if(Vector3.Distance(startPoint, transform.position) < 0.1f)
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
        Instantiate(explosion, transform.position + new Vector3(0, 10, 0), transform.rotation);
        shootable = false;
        movable = false;
        bulletLine.enabled = false;
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            JadeAssaultRifleBullet bullet = collision.gameObject.GetComponent<JadeAssaultRifleBullet>();
            if (currentHp > 0)
            {
                currentHp -= bullet.damage;
                Debug.Log("Emeny HP: " + currentHp);
            }
            else
            {
                // die animation
            }
            GetComponent<Rigidbody>().isKinematic = true;
        }

        if (collision.gameObject.tag == "MainCharacter" || collision.gameObject.tag == "Enemy")
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
}