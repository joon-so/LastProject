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

    float shootCurTime = 0.0f;
    public float shootCooltime = 7.0f;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;
    Animator anim;
    Vector3 startPoint;

    [SerializeField] float angle = 0f;
    [SerializeField] float distance = 0f;
    [SerializeField] LayerMask layerMask = 0;

    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        InvokeRepeating("Find", 0f, 0.5f);
        startPoint = transform.position;
    }

    void Update()
    {
        Find();
        //Attack();
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
                nav.SetDestination(transform.position);
                anim.SetBool("isAttack", true);
                //shoot = true;
                if (shootCurTime > 0.0f)
                    shootCurTime -= Time.deltaTime;
                else
                {
                    GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
                    Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                    shootCurTime = shootCooltime;
                }
            }
            //Detect
            else if (playerDistance < detectDistance)
            {
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
                nav.SetDestination(startPoint);
            }
        }
    }

    //void Attack()
    //{

    //    if (charging)
    //    {
    //        bulletScale += Time.deltaTime;
    //        //bullet.transform.GetChild(1).transform.localScale = new Vector3(bulletScale, bulletScale, bulletScale);

    //        if (bulletScale > bulletMaxSize)
    //            charging = false;
    //    }
    //    else
    //    {
    //        //น฿ป็
    //        bulletScale = 0.0f;
    //        charging = false;
    //    }

    //    if (shoot && !charging)
    //    {
    //        //GameObject instantBullet = null;
    //        //Rigidbody bulletRigid = null;
    //        if (!charging)
    //        {
    //            GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
    //            Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
    //            //bulletRigid.velocity = bulletPos.forward * 30;

    //            shoot = false;
    //            charging = true;
    //        }
    //    }

    //}
}