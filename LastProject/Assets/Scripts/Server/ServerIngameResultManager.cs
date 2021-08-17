using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ServerIngameResultManager : MonoBehaviour
{
    [SerializeField] List<GameObject> playerInfo;

    private void OnEnable()
    {
        for (int i = 0; i < ServerIngameManager.instance.playerCount; ++i)
        {
            playerInfo[i].SetActive(true);
        }
    }


    public void ResultExitButton()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("ServerLogin");
    }
}
