using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1To2 : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        if (SetActiveManager.instance != null)
           SetActiveManager.instance.SetActiveFalse();

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(6.5f);
        levelLoader.LoadNextLevel();
    }
}