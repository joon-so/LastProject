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

    // stage info
    public string stageInfo;


    // player info
    public int playerKill = 0;
    public int playerDeath = 0;
    public int playerScore = 0;

    public float mainPlayerMaxHp;
    public float mainPlayerMaxEp;
    public float subPlayerMaxHp;
    public float subPlayerMaxEp;

    public float mainPlayerHp;
    public float mainPlayerEp;
    public float subPlayerHp;
    public float subPlayerEp;

    // 초상화
    public GameObject mainMask;
    public GameObject subMask;

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


    // 태그 여기서 정의

}