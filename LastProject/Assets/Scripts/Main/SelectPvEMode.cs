using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectPvEMode : MonoBehaviour
{
    [SerializeField] Image loadButtonImage;
    [SerializeField] Button loadButton;
    [SerializeField] AudioClip uiButtonSound;

    void Start()
    {
        if(DataBaseManager.PlayerPvELevel == 0)
        {
            loadButton.interactable = false;
            Color color = loadButtonImage.color;
            color.a = 0.3f;
            loadButtonImage.color = color;
        }

    }

    public void OnClickCharacterSelect()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("LoadCharacterSelect", 1f);
    }
    void LoadCharacterSelect()
    {
        SceneManager.LoadScene("CharacterSelect");
    }
    public void OnClickExitGOMain()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("LoadMain", 1f);
    }
    void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }
}
