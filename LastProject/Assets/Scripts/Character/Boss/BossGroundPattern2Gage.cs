using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGroundPattern2Gage : MonoBehaviour
{
    private float LifeTime;

    void Start()
    {
        LifeTime = 1.8f;
    }

    void FixedUpdate()
    {
        LifeTime -= Time.deltaTime;
        if (LifeTime < 0)
            Destroy(gameObject);
    }
}