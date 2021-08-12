using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageEndManager : MonoBehaviour
{
    LevelLoader levelLoader;

    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        levelLoader.LoadNextLevel();
    }
}
