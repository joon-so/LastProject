using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    [SerializeField] [Range(30f, 100f)] float detectDistance;
    [SerializeField] [Range(30f, 100f)] float shootDistance = 30f;
    [SerializeField] Transform bulletPos = null;
    [SerializeField] GameObject bullet = null;
    [SerializeField] Transform bulletStartPoint;
    [SerializeField] Transform bulletEndPoint;

    float shootCurTime = 0.0f;
    public float shootCooltime = 3.0f;
    float chargingTime = 1.2f;
    float spinSpeed = 200.0f;

    bool charging = false;
    bool shootable = true;
    bool movable = true;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;
    Animator anim;
    LineRenderer bulletLine;
    Vector3 startPoint;

    [SerializeField] float angle = 0f;
    [SerializeField] float distance = 0f;
    [SerializeField] LayerMask layerMask = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        bulletLine = GetComponent<LineRenderer>();

        bulletLine.SetVertexCount(2);
        bulletLine.enabled = false;

        InvokeRepeating("Find", 0f, 0.5f);
        startPoint = transform.position;
    }

    void FixedUpdate()
    {
        Find();
    }

    void Sight()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, distance, layerMask);

        if (colliders.Length > 0)
        {
            Transform player = colliders[0].transform;

            Vector3 direction = (player.position - transform.position).normalized;
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
        Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        shootCurTime = shootCooltime;
        charging = true;
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
        charging = false;
        movable = true;
        //쿨타임 지난 후 
        yield return new WaitForSeconds(shootCooltime - chargingTime);
        shootable = true;
    }
}