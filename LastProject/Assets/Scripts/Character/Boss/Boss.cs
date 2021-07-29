using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject GroundPattern1Effect;
    [SerializeField] GameObject GroundPattern2Effect;
    [SerializeField] GameObject GroundPattern3Effect;

    [SerializeField] GameObject PageChangeEffect;

    [SerializeField] GameObject FlyPattern1Effect;
    [SerializeField] GameObject FlyPattern2Effect;
    //[SerializeField] GameObject FlyPattern3Effect;

    GameObject targetCharacter;

    Rigidbody rigidbody;
    Animator anim;
    NavMeshAgent nav;

    bool canAttack;

    int page;
    int pattern;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        targetCharacter = GameObject.FindGameObjectWithTag("MainCharacter");

        canAttack = true;

        page = 2;
        pattern = 1;
    }

    void FixedUpdate()
    {
        if (canAttack)
        {
            if(page == 0)
            {

            }
            else if(page == 1)
            {
                if(pattern == 1)
                {
                    StartCoroutine(GroundPattern1());
                }
                else if (pattern == 2)
                {
                    StartCoroutine(GroundPattern2());
                }
                else if (pattern == 3)
                {
                    StartCoroutine(GroundPattern3());
                }
            }
            else if(page == 2)
            {
                StartCoroutine(ChangePage(2));
            }
            else if(page == 3)
            {
                if (pattern == 1)
                {
                    StartCoroutine(FlyPattern1());
                }
                else if (pattern == 2)
                {
                    StartCoroutine(FlyPattern2());
                }
            }
        }
    }

    IEnumerator GroundPattern1()
    {
        yield return null;
    }
    IEnumerator GroundPattern2()
    {
        yield return null;
    }
    IEnumerator GroundPattern3()
    {
        yield return null;
    }
    IEnumerator ChangePage(int changePage)
    {
        canAttack = false;
        Vector3 pos = transform.position + transform.forward * 2.2f - transform.right * 0.9f + transform.up * 0.5f;
        yield return new WaitForSeconds(0.6f);
        Instantiate(PageChangeEffect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(0.72f);
        Instantiate(PageChangeEffect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(0.72f);
        Instantiate(PageChangeEffect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(1f);
        page = changePage;
    }
    IEnumerator FlyPattern1()
    {
        yield return null;
    }
    IEnumerator FlyPattern2()
    {
        yield return null;
    }
}
