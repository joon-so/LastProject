using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameMenu : MonoBehaviour
{
    public void ClickExitButton()
    {
        //Destroy(GameManager.instance.gameObject);
        //Destroy(SoundManager.instance.gameObject);
        //Destroy(PlayerManager.instance.gameObject);

        Application.Quit();
    }
}
