using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karmen0To1Script : MonoBehaviour
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

        movePoint1 = new Vector3(-13.62f, transform.position.y, -19.11f);
        movePoint2 = new Vector3(1.94f, transform.position.y, 15.15f);
        movePoint3 = new Vector3(6.35f, transform.position.y, 9.48f);
        StartCoroutine(Motion());
    }

    IEnumerator Motion()
    {
        anim.SetTrigger("Start");
        yield return null;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.95f)
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
        anim.SetTrigger("Stretching");
        yield return new WaitForSeconds(6.35f);

        anim.SetBool("Run", true);
        rigidbody.freezeRotation = true;
        rigidbody.constraints = RigidbodyConstraints.FreezePositionY;
        movePoint1 = new Vector3(-13.62f, transform.position.y, -19.11f);

        transform.LookAt(movePoint1);
        while (Vector3.Distance(transform.position, movePoint1) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint1, 5f * Time.deltaTime);
            yield return null;
        }
    }
}
