using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [SerializeField] GameObject startPosition1;
    [SerializeField] GameObject startPosition2;

    void Start()
    {
        GameManager.instance.character1.transform.position = startPosition1.transform.position;
        GameManager.instance.character2.transform.position = startPosition2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
