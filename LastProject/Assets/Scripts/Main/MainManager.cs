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
        SceneManager.LoadScene("SelectPvEMode");
    }
    public void OnClickPvP()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("ServerLogin");
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
    public void OnClickPvENewGame()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("CharacterSelect");
    }
    public void OnClickPvELoadGame()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("LoadPvELevel");
    }
}



