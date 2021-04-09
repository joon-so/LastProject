using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy3 : MonoBehaviour
{
    [SerializeField] [Range(30f, 100f)] float detectDistance;
    [SerializeField] [Range(30f, 100f)] float shootDistance = 30f;
    [SerializeField] GameObject bullet = null;
    [SerializeField] Transform bulletLeftStartPoint;
    [SerializeField] Transform bulletRightStartPoint;

    float straightTime = 0.2f;
    public float shootCooltime = 1.0f;
    public float spinSpeed = 50.0f;

    bool shootable = true;
    bool movable = true;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;
    Animator anim;
    Vector3 startPoint;

    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        InvokeRepeating("Find", 0f, 0.5f);
        startPoint = transform.position;
    }
                                                                                                   
    void FixedUpdate()
    {
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
                //anim.SetBool("isAttack", true);

                if (shootable)
                    StartCoroutine(Attack());
            }
            //Detect
            else if (playerDistance < detectDistance)
            {
                if (movable)
                    nav.SetDestination(mainCharacter.transform.position);

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
        // ÃÑ¾Ë »ý¼º
        Instantiate(bullet, bulletLeftStartPoint.position, bulletLeftStartPoint.rotation);
        movable = false;
        shootable = false;
        //anim.SetBool("isAttack", true);
        //ÁÂ¿ì 2¹ß
        yield return new WaitForSeconds(straightTime);
        Instantiate(bullet, bulletRightStartPoint.position, bulletRightStartPoint.rotation);
        yield return new WaitForSeconds(straightTime);
        Instantiate(bullet, bulletLeftStartPoint.position, bulletLeftStartPoint.rotation);
        yield return new WaitForSeconds(straightTime);
        Instantiate(bullet, bulletRightStartPoint.position, bulletRightStartPoint.rotation);
        //ÄðÅ¸ÀÓ
        movable = true;
        yield return new WaitForSeconds(shootCooltime);
        shootable = true;
    }
}