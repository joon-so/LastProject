using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    public float destroyTime = 1f;

    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
            Destroy(gameObject);
    }
}
