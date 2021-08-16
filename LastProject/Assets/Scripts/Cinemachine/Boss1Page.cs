using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Page : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        GameManager.instance.bossPage = 1;
        SetActiveManager.instance.SetActiveFalse();

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(11.2f);
        levelLoader.LoadBossStage();
    }
}
