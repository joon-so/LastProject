using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jade0To1Script : MonoBehaviour
{
    Vector3 movePoint1;
    Vector3 movePoint2;
    Vector3 movePoint3;


    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        movePoint1 = new Vector3(1.97f, transform.position.y, 20.3f);
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
                anim.speed = 0.29f;
            }
            else
            {
                anim.speed = 1f;
            }
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        anim.SetTrigger("Stretching");
    }
}
