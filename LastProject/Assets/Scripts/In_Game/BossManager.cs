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

    public int bossPage;

    void Start()
    {
        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        boss1PageHp = 500.0f;
        boss2PageHp = 1000.0f;
        boss3PageHp = 1000.0f;

        curBoss1PageHp = boss1PageHp;
        curBoss2PageHp = boss2PageHp;
        curBoss3PageHp = boss3PageHp;

        bossPage = 1;
    }

    void Update()
    {
        SetBossPage();
    }

    void SetBossPage()
    {
        if (boss1PageHp <= 0)
        {
            bossPage = 2;
            levelLoader.LoadBossPage2();
        }

        if (boss2PageHp <= 0)
        {
            bossPage = 3;
            levelLoader.LoadBossPage3();
        }

        if (boss3PageHp <= 0)
        {
            bossPage = 0;
            levelLoader.LoadMain();
        }

        if(bossPage == 1)
        {
            bossPilot.SetActive(true);
            boss.SetActive(false);
        }
        else if (bossPage == 2)
        {
            bossPilot.SetActive(false);
            boss.SetActive(true);
        }
        else if (bossPage == 3)
        {
            bossPilot.SetActive(false);
            boss.SetActive(true);
        }
    }
}
