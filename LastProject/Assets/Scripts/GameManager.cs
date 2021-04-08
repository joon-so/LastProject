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

    public float tagCoolTime = 3.0f;

    public GameObject character1;
    public GameObject character2;

    public GameObject mainCharacter1SkillSlot;
    public GameObject subCharacter1SkillSlot;
    public GameObject mainCharacter2SkillSlot;
    public GameObject subCharacter2SkillSlot;

    public GameObject mainMaskCharacter1;
    public GameObject subMaskCharacter1;
    public GameObject mainMaskCharacter2;
    public GameObject subMaskCharacter2;

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

    public void TagObject1()
    {
        character1.gameObject.tag = "SubCharacter";
        character2.gameObject.tag = "MainCharacter";
        character1.gameObject.layer = 7;
        character2.gameObject.layer = 6;
    }
    public void TagObject2()
    {
       character1.gameObject.tag = "MainCharacter";
       character2.gameObject.tag = "SubCharacter";
       character1.gameObject.layer = 6;
       character2.gameObject.layer = 7;
    }
    public void TagSkillSlot1()
    {
        mainCharacter1SkillSlot.SetActive(false);
        subCharacter1SkillSlot.SetActive(true);

        mainCharacter2SkillSlot.SetActive(true);
        subCharacter2SkillSlot.SetActive(false);
    }
    public void TagSkillSlot2()
    {
        mainCharacter1SkillSlot.SetActive(true);
        subCharacter1SkillSlot.SetActive(false);

        mainCharacter2SkillSlot.SetActive(false);
        subCharacter2SkillSlot.SetActive(true);
    }
    public void TagMask1()
    {
        mainMaskCharacter1.SetActive(false);
        subMaskCharacter1.SetActive(true);
        mainMaskCharacter2.SetActive(true);
        subMaskCharacter2.SetActive(false);
    }
    public void TagMask2()
    {
        mainMaskCharacter1.SetActive(true);
        subMaskCharacter1.SetActive(false);
        mainMaskCharacter2.SetActive(false);
        subMaskCharacter2.SetActive(true);
    }
    public void TagHpEp()
    {
        float temp = mainPlayerHp;
        mainPlayerHp = subPlayerHp;
        subPlayerHp = temp;

        temp = GameManager.instance.mainPlayerEp;
        mainPlayerEp = subPlayerEp;
        subPlayerEp = temp;

        temp = GameManager.instance.mainPlayerMaxHp;
        mainPlayerMaxHp = subPlayerMaxHp;
        subPlayerMaxHp = temp;

        temp = GameManager.instance.mainPlayerMaxEp;
        mainPlayerMaxEp = subPlayerMaxEp;
        subPlayerMaxEp = temp;
    }
    public void EffectFillBar()
    {
        float temp = mainPlayerHp;
        mainPlayerHp = 0;
        if (mainPlayerHp < temp)
        {
            mainPlayerHp += 1.0f;
        }
        temp = mainPlayerEp;
        mainPlayerEp = 0;
        if (mainPlayerEp < temp)
        {
            mainPlayerEp += 1.0f;
        }
        temp = subPlayerHp;
        subPlayerHp = 0;
        if (subPlayerHp < temp)
        {
            subPlayerHp += 1.0f;
        }
        temp = subPlayerEp;
        subPlayerEp = 0;
        if (subPlayerEp < temp)
        {
            subPlayerEp += 1.0f;
        }
    }
}