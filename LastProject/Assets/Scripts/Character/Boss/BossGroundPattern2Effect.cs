using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundPattern2Effect : MonoBehaviour
{
    private float LifeTime;
    GameObject boss;

    void Start()
    {
        boss = GameObject.Find("Boss");
        LifeTime = 1.5f;
    }

    void FixedUpdate()
    {
        transform.position = boss.transform.position + boss.transform.up * 3f + boss.transform.forward * 2.5f;
        
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            Destroy(gameObject);
    }
}