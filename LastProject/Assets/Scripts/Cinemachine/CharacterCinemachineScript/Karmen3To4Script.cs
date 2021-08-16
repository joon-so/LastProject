using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Karmen3To4Script : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] GameObject weapon;
    [SerializeField] AudioClip karmenAttackSound;

    Vector3 movePoint1;
    Vector3 lookPoint1;
    Vector3 movePoint2;
    Vector3 ExplosionPoint;
    Vector3 movePoint3;

    Animator anim;
    void Start()
    {
        weapon.SetActive(false);
        anim = GetComponent<Animator>();
        movePoint1 = new Vector3(1.86f, transform.position.y, 16.13f);
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
            transform.position = Vector3.MoveTowards(transform.position, movePoint1, 6f * Time.deltaTime);
            yield return null;
        }
        anim.SetBool("Run", false);

        yield return new WaitForSeconds(0.1f);
        transform.LookAt(lookPoint1);
        anim.SetBool("Attack",true);
        SoundManager.instance.SFXPlay("KarmenAttack", karmenAttackSound);
        yield return new WaitForSeconds(0.3f);
        Instantiate(explosion, ExplosionPoint, explosion.transform.rotation);
        yield return new WaitForSeconds(0.65f);
        anim.SetBool("Attack", false);

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
