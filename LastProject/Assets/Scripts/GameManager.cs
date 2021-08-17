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

    public int character1MaxHp;
    public int character1MaxEp;
    public int character2MaxHp;
    public int character2MaxEp;

    public float tagCoolTime;

    // Item Info
    public int curHpPotionCount;
    public int curEpPotionCount;

    public int hpPotionValue;
    public int epPotionValue;

    // stage info
    public string stageInfo;

    public GameObject character1;
    public GameObject character2;

    public int bossPage;

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
        tagCoolTime = 5.0f;

        curHpPotionCount = 3;
        curEpPotionCount = 3;
        
        hpPotionValue = 50;
        epPotionValue = 50;
    }

    public void DestroyGameManager()
    {
        Destroy(gameObject);
    }

    public void DestroyAllInstance()
    {
        Destroy(gameObject);
        Destroy(InGameUI.instance.gameObject);
        Destroy(PlayerManager.instance.gameObject);
        Destroy(SetActiveManager.instance.gameObject);
    }


    // Mode Change
    public void ChangeHpEp()
    {
        clientPlayer.character1Hp = 100;
        clientPlayer.character2Hp = 100;
        clientPlayer.character1Ep = 50;
        clientPlayer.character2Ep = 50;
    }

    public void ChangeSceneLogin()
    {
        SceneManager.LoadScene("Login");
    }
    public void ChangeSceneStage0()
    {
        SceneManager.LoadScene("Stage0");
    }
    public void ChangeSceneStage1()
    {
        SceneManager.LoadScene("Stage1");
    }
    public void ChangeSceneStage2()
    {
        SceneManager.LoadScene("Stage2");
    }
    public void ChangeSceneStage3()
    {
        SceneManager.LoadScene("Stage3");
    }
    public void ChangeSceneStage4()
    {
        SceneManager.LoadScene("Stage4");
    }
    public void ChangeSceneStage5()
    {
        SceneManager.LoadScene("Stage5");
    }
    public void ChangeSceneStage0To1()
    {
        SceneManager.LoadScene("Stage0To1");
    }
    public void ChangeSceneStage1To2()
    {
        SceneManager.LoadScene("Stage1To2");
    }
    public void ChangeSceneStage2To3()
    {
        SceneManager.LoadScene("Stage2To3");
    }
    public void ChangeSceneStage3To4()
    {
        SceneManager.LoadScene("Stage3To4");
    }
    public void ChangeSceneBoss1PageEnter()
    {
        SceneManager.LoadScene("StageBoss1PageEnter");
    }
    public void ChangeSceneBoss2PageEnter()
    {
        SceneManager.LoadScene("StageBoss2PageEnter");
    }
    public void ChangeSceneBoss3PageEnter()
    {
        SceneManager.LoadScene("StageBoss3PageEnter");
    }
}