using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherKarmenQSkill : ServerOtherKarmen
{
    [SerializeField] GameObject obj1 = null;
    [SerializeField] GameObject obj2 = null;
    [SerializeField] GameObject obj3 = null;

    [SerializeField] GameObject otherPlayer;

    float curScale;
    float maxScale;

    int idx;

    void Start()
    {
        curScale = 1;
        maxScale = 2;
        StartCoroutine(DestroyObject());
    }

    void Update()
    {
        Debug.Log(GetComponent<ServerOtherKarmen>().getIndex());
        switch (GetComponent<ServerOtherKarmen>().getIndex())
        {
            case 1:
                gameObject.transform.position = GameObject.Find("Server2Karmen").GetComponent<ServerOtherKarmen>().qSkillPos.position;
                break;
            case 2:
                gameObject.transform.position = GameObject.Find("Server3Karmen").GetComponent<ServerOtherKarmen>().qSkillPos.position;
                break;
            case 3:
                break;
            default:
                break;
        }
        if (curScale < maxScale)
        {
            curScale += Time.deltaTime;
            transform.localScale = new Vector3(curScale, curScale, curScale);
            obj1.transform.localScale = new Vector3(curScale, curScale, curScale);
            obj2.transform.localScale = new Vector3(curScale, curScale, curScale);
            obj3.transform.localScale = new Vector3(curScale, curScale, curScale);
        }
    }

    IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(2.0f);
        Destroy(gameObject);
    }
}
