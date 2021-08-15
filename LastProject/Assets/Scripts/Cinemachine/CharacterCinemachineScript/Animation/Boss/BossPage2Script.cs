using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPage2Script : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;
    [SerializeField] GameObject backFire;
    [SerializeField] GameObject Frame;
    // Start is called before the first frame update
    void Start()
    {
        backFire.SetActive(false);
        Frame.SetActive(false);
        StartCoroutine(Exposion());
    }

    IEnumerator Exposion()
    {
        yield return new WaitForSeconds(0.72f);
        Instantiate(explosionEffect, transform.position + transform.up * 5.87f + transform.right * 1f, transform.rotation);
        yield return new WaitForSeconds(2.4f);
        backFire.SetActive(true);
        //yield return new WaitForSeconds(1.18f);
        yield return new WaitForSeconds(2f);
        //float flyTime = 1.5f;
        //while(flyTime > 0)
        //{
        //    //flyTime -= Time.deltaTime;
        //    //transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * 1f, 25f * Time.deltaTime);
        //    //yield return null;
        //}
        //transform.LookAt(transform.position - transform.forward);
        yield return new WaitForSeconds(2.5f);
        Frame.SetActive(true);
    }

}
