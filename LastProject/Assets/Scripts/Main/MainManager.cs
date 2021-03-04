using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnClickPvE()
    {
        SceneManager.LoadScene("Stage1-1");
    }
    public void OnClickPvP()
    {

    }
    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}