using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage0 : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(12.5f);
        levelLoader.LoadNextLevel();
    }
}
