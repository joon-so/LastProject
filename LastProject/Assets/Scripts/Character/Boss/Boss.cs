using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject GroundPattern1Effect;
    [SerializeField] GameObject GroundPattern2Effect;
    [SerializeField] GameObject GroundPattern2Gage;
    [SerializeField] GameObject GroundPattern3Effect;

    [SerializeField] GameObject PageChangeEffect;

    [SerializeField] GameObject FlyPattern1Effect;
    [SerializeField] GameObject FlyPattern2Effect;
    //[SerializeField] GameObject FlyPattern3Effect;

    GameObject targetCharacter;

    Rigidbody rigidbody;
    Animator anim;
    NavMeshAgent nav;

    bool canMove;
    bool canAttack;
    bool canDodge;
    bool canSkill;

    float playerDistance;
    float shootDistance;
    float detectDistance;
    float moveSpeed;
    float GroundPattern2Distance;

    int page;
    int pattern;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();

        targetCharacter = GameObject.FindGameObjectWithTag("SubCharacter");

        canMove = true;
        canAttack = true;
        canDodge = true;
        canSkill = true;

        shootDistance = 2f;

        page = 1;
        pattern = 0;

        moveSpeed = 10f;
        GroundPattern2Distance = 20f;
        //StartCoroutine(StartEffect());
    }

    void FixedUpdate()
    {
        if (targetCharacter == null)
        {
            targetCharacter = GameObject.FindGameObjectWithTag("MainCharacter");
        }
        //Change Target
        if (Input.GetKeyDown(KeyCode.F))
        {
            targetCharacter = GameObject.FindGameObjectWithTag("SubCharacter");
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            pattern = 0;
            anim.SetInteger("Pattern", pattern);
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            pattern = 1;
            anim.SetInteger("Pattern", pattern);
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            pattern = 2;
            anim.SetInteger("Pattern", pattern);
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            pattern = 3;
            anim.SetInteger("Pattern", pattern);
            canAttack = true;
        }

        if (canAttack)
        {
            canAttack = false;
            if (page == 0)
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
    IEnumerator StartEffect()
    {
        yield return new WaitForSeconds(22.8f);
        Instantiate(PageChangeEffect, transform.position + transform.up * 0.5f, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(27f);
        canAttack = true;
    }
    IEnumerator GroundPattern1()
    {
        yield return null;
    }
    IEnumerator GroundPattern2()
    {
        anim.SetInteger("Pattern", pattern);
        Instantiate(GroundPattern2Gage, transform.position, transform.rotation * Quaternion.Euler(0f, 90f, 0));

        yield return new WaitForSeconds(1.8f);

        Instantiate(GroundPattern2Effect, transform.position + transform.up * 3f + transform.forward * 2.5f, transform.rotation);
        float skillTime = 0f;
        while(skillTime < 1.5f) {
            skillTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * GroundPattern2Distance, moveSpeed * Time.deltaTime);
            yield return null;
        }

        anim.SetTrigger("SprintEnd");
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canAttack = true;
    }
    IEnumerator GroundPattern3()
    {
        Vector3 pos = transform.position + transform.forward * 2.2f - transform.right * 0.9f + transform.up * 0.5f;
        yield return new WaitForSeconds(0.6f);
        Instantiate(PageChangeEffect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(0.72f);
        Instantiate(PageChangeEffect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(0.72f);
        Instantiate(PageChangeEffect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(1f);
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canAttack = true;
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
