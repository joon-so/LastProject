using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    [SerializeField] AudioClip uiButtonSound;

    public void OnClickPvE()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("CharacterSelect");
    }
    public void OnClickPvP()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);

    }
    public void OnClickExit()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);
        Invoke("LoadLogin", 0.7f);

    }
    void LoadLogin()
    {
        SceneManager.LoadScene("Login");
    }
}



