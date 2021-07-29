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

        canAttack = false;

        page = 0;
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
                StartCoroutine(ChangePage(3));
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
        yield return null;
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
