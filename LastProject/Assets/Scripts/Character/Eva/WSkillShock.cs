using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WSkillShock : MonoBehaviour
{
    float destroyTime = 1.8f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime < 0)
            Destroy(gameObject);
    }
}
