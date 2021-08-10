using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject GroundPattern1Bullet;
    public GameObject BulletPos;
    [SerializeField] GameObject GroundPattern2Effect;
    [SerializeField] GameObject GroundPattern2Gage;
    [SerializeField] GameObject GroundPattern3Effect;
    [SerializeField] GameObject GroundPattern3Gage;

    [SerializeField] GameObject FlyPattern1Effect;
    public GameObject Frame;
    [SerializeField] GameObject FlyPattern2Effect;
    //[SerializeField] GameObject FlyPattern3Effect;

    public GameObject FlyEffect;

    GameObject targetCharacter;

    Rigidbody rigidbody;
    Animator anim;
    NavMeshAgent nav;
    BoxCollider boxCollider;

    bool canMove;
    bool canAttack;
    bool canSkill;

    float playerDistance;
    float shootDistance;
    float detectDistance;
    float moveSpeed;
    float GroundPattern2Distance;
    float spinSpeed;

    int page;
    int pattern;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();

        targetCharacter = GameObject.FindGameObjectWithTag("SubCharacter");
        FlyEffect.SetActive(false);
        Frame.SetActive(false);
        canMove = true;
        canAttack = true;
        canSkill = true;

        shootDistance = 2f;

        page = 1;
        pattern = 0;

        moveSpeed = 10f;
        spinSpeed = 300f;
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
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            pattern = 1;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            pattern = 2;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            pattern = 3;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            pattern = 4;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            pattern = 5;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            pattern = 6;
            canAttack = true;
        }
        if (canMove)
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetCharacter.transform.position - transform.position);
            Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            transform.rotation = Quaternion.Euler(0, euler.y, 0);
        }
        if (canAttack)
        {
            canAttack = false;

            if(pattern == 1)
            {
                StartCoroutine(Pattern1());
            }
            else if (pattern == 2)
            {
                StartCoroutine(Pattern2());
            }
            else if (pattern == 3)
            {
                StartCoroutine(Pattern3());
            }
            else if (pattern == 4)
            {
                StartCoroutine(Pattern4());
            }
            // pattern 5 : Fly idle
            else if (pattern == 6)
            {
                StartCoroutine(Pattern6());
            }
        }
    }
    IEnumerator StartEffect()
    {
        yield return new WaitForSeconds(22.8f);
        Instantiate(GroundPattern3Effect, transform.position + transform.up * 0.5f, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(27f);
        canAttack = true;
    }
    IEnumerator Pattern1()
    {
        canMove = false;
        anim.SetInteger("Pattern", pattern);
        //transform.LookAt(targetCharacter.transform);
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i< 20; i++)
        {
            Instantiate(GroundPattern1Bullet, BulletPos.transform.position, transform.rotation);
            yield return new WaitForSeconds(0.16f);
        }
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canAttack = true;
        canMove = true;
    }
    IEnumerator Pattern2()
    {
        canMove = false;
        //transform.LookAt(targetCharacter.transform.position);
        anim.SetInteger("Pattern", pattern);
        Instantiate(GroundPattern2Gage, transform.position, transform.rotation * Quaternion.Euler(0f, 90f, 0));

        yield return new WaitForSeconds(1.2f);

        Instantiate(GroundPattern2Effect, transform.position + transform.up * 3f + transform.forward * 2.5f, transform.rotation);
        float skillTime = 0f;
        while(skillTime < 1.5f) {
            skillTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * GroundPattern2Distance, moveSpeed * Time.deltaTime);
            yield return null;
        }

        anim.SetTrigger("SprintEnd");
        yield return new WaitForSeconds(0.2f);
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canAttack = true;
        canMove = true;
    }
    IEnumerator Pattern3()
    {
        canMove = false;
        anim.SetInteger("Pattern", pattern);
        Vector3 pos = transform.position + transform.forward * 2.2f - transform.right * 0.9f + transform.up * 0.5f;
        Instantiate(GroundPattern3Gage, pos, Quaternion.Euler(0f, 90f, 0));
        yield return new WaitForSeconds(0.6f);
        Instantiate(GroundPattern3Effect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(0.72f);
        Instantiate(GroundPattern3Effect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(0.72f);
        Instantiate(GroundPattern3Effect, pos, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(1f);
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canAttack = true;
        canMove = true;
    }
    IEnumerator Pattern4()
    {
        canMove = false;
        FlyEffect.SetActive(true);
        anim.SetInteger("Pattern", pattern);
        Instantiate(GroundPattern2Gage, transform.position, transform.rotation * Quaternion.Euler(0f, 90f, 0));
        //transform.LookAt(targetCharacter.transform);
        yield return new WaitForSeconds(1.6f);
        rigidbody.useGravity = false;
        float bezierValue = 0f;
        float shootTime = 0.8f;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y/4, boxCollider.size.z);
        Vector3 P1 = transform.position;
        Vector3 P2 = targetCharacter.transform.position + new Vector3(0, -5f, 0);
        Vector3 P3 = targetCharacter.transform.position + transform.forward * 9f;
        Vector3 bezier;
        while (bezierValue < shootTime)
        {
            bezierValue += Time.deltaTime;

            bezier = Bezier(P1, P2, P3, bezierValue * 1 / shootTime);
            //transform.LookAt(bezier);
            transform.position = bezier;
            yield return null;
        }
        rigidbody.useGravity = true;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y * 4, boxCollider.size.z);
        //yield return new WaitForSeconds(0.1f);
        float rotateTime = 0;
        while(rotateTime < 0.8f)
        {
            rotateTime += Time.deltaTime;
            Quaternion lookRotation = Quaternion.LookRotation(targetCharacter.transform.position - transform.position);
            Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            transform.rotation = Quaternion.Euler(0, euler.y, 0);

            yield return null;
        }
        //transform.LookAt(targetCharacter.transform);
        Frame.SetActive(true);
        canMove = true;
        yield return new WaitForSeconds(2.7f);
        Frame.SetActive(false);
        //yield return new WaitForSeconds(f);
        pattern = 5;
        anim.SetInteger("Pattern", pattern);
        canAttack = true;
        //canMove = true;
    }
    IEnumerator Pattern6()
    {
        canMove = false;
        anim.SetInteger("Pattern", pattern);
        //transform.LookAt(targetCharacter.transform);
        Instantiate(GroundPattern2Gage, transform.position, transform.rotation * Quaternion.Euler(0f, 90f, 0));
        yield return new WaitForSeconds(1f);

        rigidbody.useGravity = false;
        float bezierValue = 0f;
        float shootTime = 0.8f;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y / 4, boxCollider.size.z);
        Vector3 P1 = transform.position;
        Vector3 P2 = targetCharacter.transform.position + new Vector3(0, -5f, 0);
        Vector3 P3 = targetCharacter.transform.position + transform.forward * 9f;
        Vector3 bezier;
        while (bezierValue < shootTime)
        {
            bezierValue += Time.deltaTime;

            bezier = Bezier(P1, P2, P3, bezierValue * 1 / shootTime);
            //transform.LookAt(bezier);
            transform.position = bezier;
            yield return null;
        }
        rigidbody.useGravity = true;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y * 4, boxCollider.size.z);

        yield return new WaitForSeconds(0.4f);
        Instantiate(GroundPattern3Effect, transform.position, Quaternion.Euler(90f, 0f, 0));
        FlyEffect.SetActive(false);
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canAttack = true;
        canMove = true;
    }
    Vector3 Bezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, float value)
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, value);
        Vector3 B = Vector3.Lerp(P_2, P_3, value);
        Vector3 D = Vector3.Lerp(A, B, value);

        return D;
    }
}
