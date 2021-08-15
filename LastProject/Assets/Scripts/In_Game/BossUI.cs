using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUI : MonoBehaviour
{
    [SerializeField] Image boss1PageHp;
    [SerializeField] Image boss2PageHp;
    [SerializeField] Image boss3PageHp;

    [SerializeField] Image boss1PageMask;
    [SerializeField] Image boss2PageMask;
    [SerializeField] Image boss3PageMask;

    private int bossPage;

    void Start()
    {
        bossPage = GameObject.Find("BossManager").GetComponent<BossManager>().bossPage;
    }

    void Update()
    {
        BossPilotHpUI();
        BossHpUI();
    }

    void BossMask()
    {
    }

    void BossPilotHpUI()
    {

    }
    void BossHpUI()
    {

    }
}
