using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundPattern2Effect : MonoBehaviour
{
    private float LifeTime;

    void Start()
    {
        LifeTime = 1.5f;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * 20f, 10f * Time.deltaTime);
        
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            Destroy(gameObject);
    }
}