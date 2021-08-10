using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public struct ClientPlayer
    {
        // C1 : 1,  C2 : 2
        public short curMainCharacter;

        //  K: 1 ~ E : 4
        public short selectCharacter1;
        public short selectCharacter2;

        public int character1Hp;
        public int character1Ep;

        public int character2Hp;
        public int character2Ep;
    }

    public ClientPlayer clientPlayer = new ClientPlayer();

    // player Info
    public int playerKill = 0;
    public int playerDeath = 0;
    public int playerScore = 0;

    public int character1MaxHp;
    public int character1MaxEp;
    public int character2MaxHp;
    public int character2MaxEp;

    public float tagCoolTime = 5.0f;

    // Item Info
    public int curHpPotionCount;
    public int curEpPotionCount;

    public int hpPotionValue;
    public int epPotionValue;

    // stage info
    public string stageInfo;

    public GameObject character1;
    public GameObject character2;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // 초기 Hp Ep Max값, 개수 설정
        character1MaxEp = clientPlayer.character1Hp;
        character1MaxEp = clientPlayer.character1Ep;

        character2MaxEp = clientPlayer.character2Hp;
        character2MaxEp = clientPlayer.character2Ep;

        curHpPotionCount = 3;
        curEpPotionCount = 3;
    }
}