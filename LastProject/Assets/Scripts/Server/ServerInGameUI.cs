using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ServerInGameUI : MonoBehaviour
{
    [SerializeField] GameObject mainKarmenMask;
    [SerializeField] GameObject mainJadeMask;
    [SerializeField] GameObject mainLeinaMask;
    [SerializeField] GameObject mainEvaMask;

    [SerializeField] GameObject subKarmenMask;
    [SerializeField] GameObject subJadeMask;
    [SerializeField] GameObject subLeinaMask;
    [SerializeField] GameObject subEvaMask;

    [SerializeField] Image imageMainHpFill;
    [SerializeField] Image imageMainEpFill;
    [SerializeField] Image imageSubHpFill;
    [SerializeField] Image imageSubEpFill;
    
    [SerializeField] Text textMainHp;
    [SerializeField] Text textMainEp;
    [SerializeField] Text textSubHp;
    [SerializeField] Text textSubEp;

    [SerializeField] GameObject gameMenu;

    [SerializeField] GameObject hpPotionSlot;
    [SerializeField] GameObject epPotionSlot;

    [SerializeField] Text hpCount;
    [SerializeField] Text epCount;

    [SerializeField] Image hpCoolFill;
    [SerializeField] Image epCoolFill;

    private GameObject mainC1;
    private GameObject subC1;
    private GameObject mainC2;
    private GameObject subC2;

    private float c1MaxHp;
    private float c1MaxEp;
    private float c2MaxHp;
    private float c2MaxEp;

    [SerializeField] GameObject karmenSlot;
    [SerializeField] GameObject jadeSlot;
    [SerializeField] GameObject leinaSlot;
    [SerializeField] GameObject evaSlot;

    private GameObject c1Slot;
    private GameObject c2Slot;

    [SerializeField] Image dodgeCoolFill;

    [SerializeField] Image qSkillCoolFill;
    [SerializeField] Image wSkillCoolFill;

    void Start()
    {
        if (ServerLoginManager.playerList[0].selectMainCharacter == 1)
        {
            mainKarmenMask.SetActive(true);
            c1MaxHp = 500.0f;
            c1MaxEp = 100.0f;
            mainC1 = mainKarmenMask;
            subC1 = subKarmenMask;
            c1Slot = karmenSlot;
            c1Slot.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
        {
            mainJadeMask.SetActive(true);
            c1MaxHp = 400.0f;
            c1MaxEp = 200.0f;
            mainC1 = mainJadeMask;
            subC1 = subJadeMask;
            c1Slot = jadeSlot;
            c1Slot.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
        {
            mainLeinaMask.SetActive(true);
            c1MaxHp = 400.0f;
            c1MaxEp = 200.0f;
            mainC1 = mainLeinaMask;
            subC1 = subLeinaMask;
            c1Slot = leinaSlot;
            c1Slot.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
        {
            mainEvaMask.SetActive(true);
            c1MaxHp = 500.0f;
            c1MaxEp = 100.0f;
            mainC1 = mainEvaMask;
            subC1 = subEvaMask;
            c1Slot = evaSlot;
            c1Slot.SetActive(true);
        }

        if (ServerLoginManager.playerList[0].selectSubCharacter == 1)
        {
            subKarmenMask.SetActive(true);
            c2MaxHp = 500.0f;
            c2MaxEp = 100.0f;
            mainC2 = mainKarmenMask;
            subC2 = subKarmenMask;
            c2Slot = karmenSlot;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 2)
        {
            subJadeMask.SetActive(true);
            c2MaxHp = 400.0f;
            c2MaxEp = 200.0f;
            mainC2 = mainJadeMask;
            subC2 = subJadeMask;
            c2Slot = jadeSlot;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 3)
        {
            subLeinaMask.SetActive(true);
            c2MaxHp = 400.0f;
            c2MaxEp = 200.0f;
            mainC2 = mainLeinaMask;
            subC2 = subLeinaMask;
            c2Slot = leinaSlot;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 4)
        {
            subEvaMask.SetActive(true);
            c2MaxHp = 500.0f;
            c2MaxEp = 100.0f;
            mainC2 = mainEvaMask;
            subC2 = subEvaMask;
            c2Slot = evaSlot;
        }
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameMenu.activeSelf)
                gameMenu.SetActive(false);
            else
                gameMenu.SetActive(true);
        }

        if (ServerLoginManager.playerList[0].character1Hp > 0 || ServerLoginManager.playerList[0].character2Hp > 0)
        {
            UpdateHp();
            UpdateCoolTimeUI();
            if (Input.GetKeyDown(KeyCode.F))
            {
                TagCharacterMask();
                TagCharacterSlot();
            }
        }
    }

    void TagCharacterMask()
    {
        if (ServerLoginManager.playerList[0].is_Main_Character == 1)
        {
            mainC1.SetActive(false);
            subC1.SetActive(true);

            mainC2.SetActive(true);
            subC2.SetActive(false);
        }
        else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
        {
            mainC1.SetActive(true);
            subC1.SetActive(false);

            mainC2.SetActive(false);
            subC2.SetActive(true);
        }
    }
    void TagCharacterSlot()
    {
        if (ServerLoginManager.playerList[0].is_Main_Character == 1)
        {
            c1Slot.SetActive(false);
            c2Slot.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
        {
            c1Slot.SetActive(true);
            c2Slot.SetActive(false);
        }
    }

    void UpdateHp()
    {
        if (ServerLoginManager.playerList[0].is_Main_Character == 1)
        {
            imageMainHpFill.fillAmount = Convert.ToSingle(ServerLoginManager.playerList[0].character1Hp) / c1MaxHp;
            imageMainEpFill.fillAmount = Convert.ToSingle(ServerLoginManager.playerList[0].character1Ep) / c1MaxEp;
            imageSubHpFill.fillAmount = Convert.ToSingle(ServerLoginManager.playerList[0].character2Hp) / c2MaxHp;
            imageSubEpFill.fillAmount = Convert.ToSingle(ServerLoginManager.playerList[0].character2Ep) / c2MaxEp;

            textMainHp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character1Hp, c1MaxHp);
            textMainEp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character1Ep, c1MaxEp);
            textSubHp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character2Hp, c2MaxHp);
            textSubEp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character2Ep, c2MaxEp);
        }
        else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
        {
            imageMainHpFill.fillAmount = Convert.ToSingle(ServerLoginManager.playerList[0].character2Hp) / c2MaxHp;
            imageMainEpFill.fillAmount = Convert.ToSingle(ServerLoginManager.playerList[0].character2Ep) / c2MaxEp;
            imageSubHpFill.fillAmount = Convert.ToSingle(ServerLoginManager.playerList[0].character1Hp) / c1MaxHp;
            imageSubEpFill.fillAmount = Convert.ToSingle(ServerLoginManager.playerList[0].character1Ep) / c1MaxEp;

            textMainHp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character2Hp, c2MaxHp);
            textMainEp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character2Ep, c2MaxEp);
            textSubHp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character1Hp, c1MaxHp);
            textSubEp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character1Ep, c1MaxEp);
        }
    }

    void UpdateCoolTimeUI()
    {
        if (ServerLoginManager.playerList[0].is_Main_Character == 1)
        {
            dodgeCoolFill.fillAmount = 1 - ServerMyPlayerManager.instance.curC1DodgeCoolTime / ServerMyPlayerManager.instance.c1DodgeCoolTime;
            qSkillCoolFill.fillAmount = 1 - ServerMyPlayerManager.instance.curC1QSkillCoolTime / ServerMyPlayerManager.instance.c1QSkillCoolTime;
            wSkillCoolFill.fillAmount = 1 - ServerMyPlayerManager.instance.curC1WSkillCoolTime / ServerMyPlayerManager.instance.c1WSkillCoolTime;
        }
        else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
        {
            dodgeCoolFill.fillAmount = 1 - ServerMyPlayerManager.instance.curC2DodgeCoolTime / ServerMyPlayerManager.instance.c2DodgeCoolTime;
            qSkillCoolFill.fillAmount = 1 - ServerMyPlayerManager.instance.curC2QSkillCoolTime / ServerMyPlayerManager.instance.c2QSkillCoolTime;
            wSkillCoolFill.fillAmount = 1 - ServerMyPlayerManager.instance.curC2WSkillCoolTime / ServerMyPlayerManager.instance.c2WSkillCoolTime;
        }

        hpCoolFill.fillAmount = 1 - ServerMyPlayerManager.instance.curHpCoolTime / ServerMyPlayerManager.instance.hpCoolTime;
        epCoolFill.fillAmount = 1 - ServerMyPlayerManager.instance.curEpCoolTime / ServerMyPlayerManager.instance.epCoolTime;

        hpCount.text = string.Format("{0}", ServerMyPlayerManager.instance.myHpPotionCount);
        epCount.text = string.Format("{0}", ServerMyPlayerManager.instance.myEpPotionCount);
    }
}