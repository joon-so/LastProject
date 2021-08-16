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
            bossPilot.SetActive(true);
            boss.SetActive(false);

            if (curBoss1PageHp <= 0)
            {
                levelLoader.LoadBossPage2();
                curBoss1PageHp = 0;
            }
        }
        if (GameManager.instance.bossPage == 2)
        {
            bossPilot.SetActive(false);
            boss.SetActive(true);

            if (curBoss2PageHp <= 0)
            {
                levelLoader.LoadBossPage3();
                curBoss2PageHp = 0;
            }
        }
        if (GameManager.instance.bossPage == 3)
        {
            bossPilot.SetActive(false);
            boss.SetActive(true);

            if (curBoss3PageHp <= 0)
            {
                levelLoader.LoadBossPage3();
                curBoss3PageHp = 0;
            }
        }
    }
}
