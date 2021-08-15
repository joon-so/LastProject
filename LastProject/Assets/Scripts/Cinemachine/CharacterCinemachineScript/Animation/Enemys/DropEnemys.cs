using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropEnemys : MonoBehaviour
{
    [SerializeField] GameObject explosionEffect;

    void Start()
    {
        StartCoroutine(Exposion());
    }

    IEnumerator Exposion()
    {
        bool once = false;
        while (true)
        {
            if (transform.position.y <= 0.3f && !once)
            {
                once = true;
                Instantiate(explosionEffect, transform.position, transform.rotation);
            }
            yield return null;
        }

    }
}
