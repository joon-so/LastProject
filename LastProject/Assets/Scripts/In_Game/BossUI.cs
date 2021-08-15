using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] Image boss1PageHp;
    [SerializeField] Image boss2PageHp;
    [SerializeField] Image boss3PageHp;

    [SerializeField] GameObject boss1PageHpObj;
    [SerializeField] GameObject boss2PageHpObj;
    [SerializeField] GameObject boss3PageHpObj;

    [SerializeField] GameObject boss1PageMask;
    [SerializeField] GameObject boss2PageMask;
    [SerializeField] GameObject boss3PageMask;

    private BossManager bossManager;

    void Start()
    {
        bossManager = GameObject.Find("BossManager").GetComponent<BossManager>();
    }

    void Update()
    {
        BossMask();
        BossHpUI();
    }

    void BossMask()
    {
        if (GameManager.instance.bossPage == 1)
        {
            boss1PageMask.SetActive(true);
            boss2PageMask.SetActive(false);
            boss3PageMask.SetActive(false);
        }
        else if (GameManager.instance.bossPage == 2)
        {
            boss1PageMask.SetActive(false);
            boss2PageMask.SetActive(true);
            boss3PageMask.SetActive(false);
        }
        else if (GameManager.instance.bossPage == 3)
        {
            boss1PageMask.SetActive(false);
            boss2PageMask.SetActive(false);
            boss3PageMask.SetActive(true);
        }
    }
    void BossHpUI()
    {
        if (GameManager.instance.bossPage == 1)
        {
            boss1PageHp.fillAmount = bossManager.curBoss1PageHp / bossManager.boss1PageHp;
        }
        if (GameManager.instance.bossPage == 2)
        {
            boss1PageHpObj.SetActive(false);
            boss2PageHp.fillAmount = bossManager.curBoss2PageHp / bossManager.boss2PageHp;
        }
        if (GameManager.instance.bossPage == 3)
        {
            boss1PageHpObj.SetActive(false);
            boss2PageHpObj.SetActive(false); 
            boss3PageHp.fillAmount = bossManager.curBoss3PageHp / bossManager.boss3PageHp;
        }
    }
}