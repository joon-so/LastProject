using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Page : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        GameManager.instance.bossPage = 2; 
        SetActiveManager.instance.SetActiveFalse();

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(19.5f);
        levelLoader.LoadBossStage();
    }
}