using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StageStartManager : MonoBehaviour
{
    [SerializeField] GameObject startPoint1;
    [SerializeField] GameObject startPoint2;

    void Start()
    {
        SetActiveManager.instance.SetActiveTrue();

        GameManager.instance.character1.GetComponent<NavMeshAgent>().enabled = false;
        GameManager.instance.character2.GetComponent<NavMeshAgent>().enabled = false;

        GameManager.instance.character1.transform.position = startPoint1.transform.position;
        GameManager.instance.character2.transform.position = startPoint2.transform.position;

        if (GameManager.instance.clientPlayer.curMainCharacter == 1)
            GameManager.instance.character2.GetComponent<NavMeshAgent>().enabled = true;
        else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
            GameManager.instance.character1.GetComponent<NavMeshAgent>().enabled = true;

    }
}
