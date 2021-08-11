using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public AudioClip uiButtonSound;
    public InputField inputID;
    public InputField inputPW;

    // 플레이어 ID, PW 정보 들어있는 곳
    public static string playerID;
    public static string playerPW;

    public void OnEndEditPlayerID(InputField inputField)
    {
        playerID = inputField.text;
    }

    public void OnEndEditPlayerPassWord(InputField inputField)
    {
        playerPW = inputField.text;
    }

    public void OnClickCreateButton()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);

        // Exit or DB
        
    }

    public void OnClickLoginButton()
    {
        SoundManager.instance.SFXPlay("Click", uiButtonSound);

        Invoke("LoadMain", 0.7f);
    }

    void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }
}