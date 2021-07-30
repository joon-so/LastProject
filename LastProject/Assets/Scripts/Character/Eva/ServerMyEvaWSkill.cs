using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMyEvaWSkill : MonoBehaviour
{
    [SerializeField] GameObject obj1 = null;
    [SerializeField] GameObject obj2 = null;
    [SerializeField] GameObject obj3 = null;
    [SerializeField] GameObject obj4 = null;
    [SerializeField] GameObject obj5 = null;

    float curScale;
    float maxScale;

    void Start()
    {
        curScale = 0.1f;
        maxScale = 1.0f;
        StartCoroutine(DestroyObject());
    }

    void Update()
    {
        gameObject.transform.position = GameObject.Find("ServerEva").GetComponent<ServerMyEva>().wSkillPos.position;

        if (curScale < maxScale)
        {
            curScale += Time.deltaTime;
            transform.localScale = new Vector3(curScale, curScale, curScale);
            obj1.transform.localScale = new Vector3(curScale, curScale, curScale);
            obj2.transform.localScale = new Vector3(curScale, curScale, curScale);
            obj3.transform.localScale = new Vector3(curScale, curScale, curScale);
            obj4.transform.localScale = new Vector3(curScale, curScale, curScale);
            obj5.transform.localScale = new Vector3(curScale, curScale, curScale);
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(1.4f);
        Destroy(gameObject);
    }
}
