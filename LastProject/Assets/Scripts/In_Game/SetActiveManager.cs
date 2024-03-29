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
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    public void SetActiveFalse()
    {
        gameUI.SetActive(false);
        player.SetActive(false);
    }
    public void SetActiveTrue()
    {
        gameUI.SetActive(false);
        player.SetActive(false);
        gameUI.SetActive(true);
        player.SetActive(true);
    }
}
