using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    [SerializeField] AudioClip uiButtonSound;

    public void OnClickLoadSelectPvEMode()
    {
       // SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("SelectPvEMode");
    }
    public void OnClickPvP()
    {
     //   SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("ServerLogin");
    }
    public void OnClickExitGOLogin()
    {
        // SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("Login");
    }

}



