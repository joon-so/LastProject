using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leina0To1Script : MonoBehaviour
{
    Vector3 movePoint1;
    Vector3 movePoint2;
    Vector3 movePoint3;

    Rigidbody rigidbody;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;

        movePoint1 = new Vector3(-6.09f, transform.position.y, -17.51f);
        movePoint2 = new Vector3(1.94f, transform.position.y, 15.15f);
        movePoint3 = new Vector3(6.35f, transform.position.y, 9.48f);
        StartCoroutine(Motion());
    }

    IEnumerator Motion()
    {
        anim.speed = 0f;
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        yield return new WaitForSeconds(5.30f);

        rigidbody.constraints = RigidbodyConstraints.None;
        Vector3 pos = new Vector3(-31.63f, 9.37f, -20.03f);
        transform.position = pos;
        rigidbody.useGravity = true;
        anim.speed = 1f;
        anim.SetTrigger("Start");
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.95)
        {
            if(anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.23f
                 && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.10f)
            {
                anim.speed = 0.31f;
            }
            else
            {
                anim.speed = 1f;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        anim.SetTrigger("Stretching");
        yield return new WaitForSeconds(5.9f);

        anim.SetBool("Run", true);
        movePoint1.y = transform.position.y;
        transform.LookAt(movePoint1);
        while (Vector3.Distance(transform.position, movePoint1) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint1, 6.5f * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("Run", false);
    }
}
