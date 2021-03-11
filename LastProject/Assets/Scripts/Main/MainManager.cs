using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    public void OnClickPvE()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
    public void OnClickPvP()
    {

    }
    public void OnClickExit()
    {
        SceneManager.LoadScene("Login");

//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//        Application.Quit();
//#endif
    }
}