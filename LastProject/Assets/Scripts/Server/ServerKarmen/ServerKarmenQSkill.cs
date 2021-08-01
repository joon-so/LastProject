using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerKarmenQSkill : MonoBehaviour
{
    [SerializeField] GameObject obj1;
    [SerializeField] GameObject obj2;
    [SerializeField] GameObject obj3;

    float curScale;
    float maxScale;
    void Start()
    {
        curScale = 1;
        maxScale = 2;
    }
    void Update()
    {
        if(gameObject.activeSelf)
        {
            if (curScale < maxScale)
            {
                curScale += Time.deltaTime;
                transform.localScale = new Vector3(curScale, curScale, curScale);
                obj1.transform.localScale = new Vector3(curScale, curScale, curScale);
                obj2.transform.localScale = new Vector3(curScale, curScale, curScale);
                obj3.transform.localScale = new Vector3(curScale, curScale, curScale);
            }
        }
    }
    void OnDisable()
    {
        curScale = 1;
    }
}
