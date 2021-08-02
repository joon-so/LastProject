using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEvaWSkill : MonoBehaviour
{
    [SerializeField] GameObject obj1;
    [SerializeField] GameObject obj2;
    [SerializeField] GameObject obj3;
    [SerializeField] GameObject obj4;

    float curScale;
    float maxScale;

    void Start()
    {
        curScale = 0.1f;
        maxScale = 1.0f;
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            if (curScale < maxScale)
            {
                curScale += Time.deltaTime;
                transform.localScale = new Vector3(curScale, curScale, curScale);
                obj1.transform.localScale = new Vector3(curScale, curScale, curScale);
                obj2.transform.localScale = new Vector3(curScale, curScale, curScale);
                obj3.transform.localScale = new Vector3(curScale, curScale, curScale);
                obj4.transform.localScale = new Vector3(curScale, curScale, curScale);
            }
        }
    }

    void OnDisable()
    {
        curScale = 0.1f;
    }
}
