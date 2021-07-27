using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeinaSynergeEffect : MonoBehaviour
{
    [SerializeField] GameObject Arrow;
    bool once;
    float destroyTime;

    void Start()
    {
        once = false;
        destroyTime = 3.8f;
    }

    void FixedUpdate()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
            Destroy(gameObject);
        if (!once)
        {
            once = true;
            StartCoroutine(MakeArrow());
        }
    }
    IEnumerator MakeArrow()
    {
        LeinaSynergeSkill.speed = 20;
        yield return new WaitForSeconds(0.2f);
        for(int i = 0; i<20; i++)
        {
            Instantiate(Arrow, transform.position + transform.up * 13f + new Vector3(Random.Range(-3.5f, 3.5f), 0, Random.Range(-3.5f, 3.5f)), Quaternion.Euler(90f, 0f, 0));
            yield return new WaitForSeconds(0.15f);
        }
        yield return new WaitForSeconds(0.8f);
        LeinaSynergeSkill.speed = 0;
    }
}
