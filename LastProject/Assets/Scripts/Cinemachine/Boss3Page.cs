using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Page : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        GameManager.instance.bossPage = 3;
        SetActiveManager.instance.SetActiveFalse();

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(8.8f);
        levelLoader.LoadBossStage();
    }
}
