using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCinemachine : MonoBehaviour
{
    private GameObject character1;
    private GameObject character2;

    void Start()
    {
        character1 = GameManager.instance.character1;
        character2 = GameManager.instance.character2;
    }

    void Update()
    {
        FirstAnimation();
    }

    void FirstAnimation()
    {

    }
}
