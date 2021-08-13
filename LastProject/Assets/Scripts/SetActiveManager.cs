using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveManager : MonoBehaviour
{
    public static SetActiveManager instance;

    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject player;

    void Awake()
    {
        if (instance == null)
            instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveFalse()
    {
        Debug.Log("����");
        gameUI.SetActive(false);
        player.SetActive(false);
    }
    public void SetActiveTrue()
    {
        Debug.Log("����");
        gameUI.SetActive(true);
        player.SetActive(true);
    }
}
