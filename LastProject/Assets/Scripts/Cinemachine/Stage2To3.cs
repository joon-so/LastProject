using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2To3 : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        SetActiveManager.instance.SetActiveFalse();

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(8.5f);
        levelLoader.LoadNextLevel();
    }
}
