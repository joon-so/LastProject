using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEndingPage : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        GameManager.instance.bossPage = 0;
        SetActiveManager.instance.SetActiveFalse();

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(9.0f);
        levelLoader.LoadLogin();
    }
}
