using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // characterselect
    public bool isMainKarmen;
    public bool isMainJade;
    public bool isMainLeina;
    public bool isMainEva;

    public bool isSubKarmen;
    public bool isSubJade;
    public bool isSubLeina;
    public bool isSubEva;

    
    // player info to server
    public int playerKill = 0;
    public int playerDeath = 0;
    public int playerAssist = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
    }

    void PlayerKill()
    {

    }

    void PlayerDeath()
    {

    }

    void PlayerAssist()
    {

    }
}