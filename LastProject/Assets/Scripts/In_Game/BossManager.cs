using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    LevelLoader levelLoader;

    [SerializeField] GameObject bossPilot;
    [SerializeField] GameObject boss;

    public float boss1PageHp;
    public float curBoss1PageHp;

    public float boss2PageHp;
    public float curBoss2PageHp;

    public float boss3PageHp;
    public float curBoss3PageHp;

    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        boss1PageHp = 500.0f;
        boss2PageHp = 1000.0f;
        boss3PageHp = 1000.0f;

        curBoss1PageHp = boss1PageHp;
        curBoss2PageHp = boss2PageHp;
        curBoss3PageHp = boss3PageHp;
    }

    void Update()
    {
        SetBossPage();
    }

    void SetBossPage()
    {
        if(GameManager.instance.bossPage == 1)
        {
            if (boss1PageHp <= 0)
            {
                levelLoader.LoadBossPage2();
            }
        }
        if (GameManager.instance.bossPage == 2)
        {
            if (boss2PageHp <= 0)
            {
                levelLoader.LoadBossPage3();
            }
        }
        if (GameManager.instance.bossPage == 3)
        {
            if (boss2PageHp <= 0)
            {
                levelLoader.LoadBossPage3();
            }
        }

        if (boss1PageHp <= 0)
        {
            levelLoader.LoadBossPage2();
        }

        if (boss2PageHp <= 0)
        {
            levelLoader.LoadBossPage3();
        }

        if (boss3PageHp <= 0)
        {
            levelLoader.LoadMain();
        }

        if(GameManager.instance.bossPage == 1)
        {
            bossPilot.SetActive(true);
            boss.SetActive(false);
        }
        else if (GameManager.instance.bossPage == 2)
        {
            bossPilot.SetActive(false);
            boss.SetActive(true);
        }
        else if (GameManager.instance.bossPage == 3)
        {
            bossPilot.SetActive(false);
            boss.SetActive(true);
        }
    }
}
