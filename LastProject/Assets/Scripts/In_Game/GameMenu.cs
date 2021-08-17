using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMenu : MonoBehaviour
{
    public void ClickResumeButton()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
    public void ClickExitButton()
    {
        Time.timeScale = 1f;
        GameManager.instance.ChangeSceneLogin();
        GameManager.instance.DestroyAllInstance();
    }
}
