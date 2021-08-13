using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leina3To4Script : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] GameObject bulletPos;
    [SerializeField] GameObject explosion;

    Vector3 movePoint1;
    Vector3 movePoint2;
    Vector3 ExplosionPoint;
    Vector3 movePoint3;

    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
        movePoint1 = new Vector3(1.97f, transform.position.y, 20.3f);
        movePoint2 = new Vector3(1.94f, transform.position.y, 15.15f);
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
            transform.position = Vector3.MoveTowards(transform.position, movePoint1, 4f * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("Run", false);

        yield return new WaitForSeconds(0.1f);
        transform.LookAt(movePoint2);
        anim.SetTrigger("Attack");
        Instantiate(bullet, bulletPos.transform.position, bulletPos.transform.rotation);
        yield return new WaitForSeconds(0.1f);
        Instantiate(explosion, ExplosionPoint, explosion.transform.rotation);

        yield return new WaitForSeconds(0.8f);
        anim.SetBool("Run", true);
        transform.LookAt(movePoint3);
        while (Vector3.Distance(transform.position, movePoint3) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint3, 4f * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("Run", false);
    }
}
