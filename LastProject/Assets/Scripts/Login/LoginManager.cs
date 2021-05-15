using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public AudioClip uiButtonSound;

    public void OnClickCreateButton()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);
    }

    public void OnClickLoginButton()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);

        SceneManager.LoadScene("Main");
    }
}