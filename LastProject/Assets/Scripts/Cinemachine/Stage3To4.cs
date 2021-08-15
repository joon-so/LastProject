using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3To4 : MonoBehaviour
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
        yield return new WaitForSeconds(4.46f);
        levelLoader.LoadNextLevel();
    }
}
