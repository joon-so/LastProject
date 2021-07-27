using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvaKarmenSynerge : MonoBehaviour
{
    List<GameObject> targetEnemys;
    public float gatherSpeed = 1.5f;
    float gatherTime = 2f;

    void Start()
    {
        targetEnemys = Eva.targetEnemys;
    }

    void FixedUpdate()
    {
        gatherTime -= Time.deltaTime;

        if (gatherTime < 0)
        {
            Destroy(gameObject);
        }

        for (int i = 0; i < targetEnemys.Count; i++)
        {
            if (targetEnemys[i])
            {
                targetEnemys[i].transform.position = Vector3.MoveTowards(targetEnemys[i].transform.position,
                            transform.position, gatherSpeed * Time.deltaTime);
            }
        }
    }
}
