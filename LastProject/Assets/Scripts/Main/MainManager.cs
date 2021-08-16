using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MainManager : MonoBehaviour
{
    [SerializeField] AudioClip uiButtonSound;

    public void OnClickPvE()
    {
       // SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("SelectPvEMode");
    }
    public void OnClickPvP()
    {
     //   SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("ServerLogin");
    }
    public void OnClickExit()
    {
       // SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("Main");
    }
    public void OnClickPvENewGame()
    {
      //  SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("CharacterSelect");
    }
    public void OnClickPvELoadGame()
    {
      //  SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("LoadPvELevel");
    }
    public void OnClickCharacterSelect()
    {
      //  SoundManager.instance.SFXPlay("Click", uiButtonSound);
        SceneManager.LoadScene("CharacterSelect");
    }
}



