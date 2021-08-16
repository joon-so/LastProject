using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    [SerializeField] AudioClip uiButtonSound;

    public void OnClickLoadSelectPvEMode()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("LoadPvEModeScene", 1f);
    }
    void LoadPvEModeScene()
    {
        SceneManager.LoadScene("SelectPvEMode");
    }

    public void OnClickPvP()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("LoadPvPScene", 1f);
    }
    void LoadPvPScene()
    {
        SceneManager.LoadScene("ServerLogin");
    }

    public void OnClickExitGOLogin()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("LoadLoginScene", 1f);
    }
    void LoadLoginScene()
    {
        SceneManager.LoadScene("Login");
    }
}