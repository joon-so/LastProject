using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eva3To4Script : MonoBehaviour
{
    [SerializeField] GameObject explosion;

    Vector3 movePoint1;
    Vector3 lookPoint1;
    Vector3 movePoint2;
    Vector3 ExplosionPoint;
    Vector3 movePoint3;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        movePoint1 = new Vector3(1.86f, transform.position.y, 16.43f);
        lookPoint1 = new Vector3(1.86f, transform.position.y, 15.33f);
        movePoint2 = new Vector3(5.53f, transform.position.y, 14.62f);
        movePoint3 = new Vector3(6.35f, transform.position.y, 9.48f);
        ExplosionPoint = new Vector3(1.87f, 1.5f, 15.04f);
        StartCoroutine(Motion());
    }

    IEnumerator Motion()
    {
        anim.SetBool("Run", true);

        transform.LookAt(movePoint1);
        while (Vector3.Distance(transform.position, movePoint1) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint1, 6.5f * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("Run", false);

        transform.LookAt(lookPoint1);
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(0.1f);
        Instantiate(explosion, ExplosionPoint, explosion.transform.rotation);
        yield return new WaitForSeconds(1.1f);

        anim.SetBool("Run", true);
        transform.LookAt(movePoint2);
        while (Vector3.Distance(transform.position, movePoint2) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint2, 3.2f * Time.deltaTime);
            yield return null;
        }

        transform.LookAt(movePoint3);
        while (Vector3.Distance(transform.position, movePoint3) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint3, 3.2f * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("Run", false);
    }
}
