using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InGameUI : MonoBehaviour
{
    [SerializeField] GameObject mainKarmenMask;
    [SerializeField] GameObject mainJadeMask;
    [SerializeField] GameObject mainLeinaMask;
    [SerializeField] GameObject mainEvaMask;

    [SerializeField] GameObject subKarmenMask;
    [SerializeField] GameObject subJadeMask;
    [SerializeField] GameObject subLeinaMask;
    [SerializeField] GameObject subEvaMask;

    [SerializeField] GameObject karmenSlot;
    [SerializeField] GameObject jadeSlot;
    [SerializeField] GameObject leinaSlot;
    [SerializeField] GameObject evaSlot;

    [SerializeField] Image imageMainHpFill;
    [SerializeField] Image imageMainEpFill;
    [SerializeField] Image imageSubHpFill;
    [SerializeField] Image imageSubEpFill;

    [SerializeField] Text textMainHp;
    [SerializeField] Text textMainEp;
    [SerializeField] Text textSubHp;
    [SerializeField] Text textSubEp;

    [SerializeField] GameObject hpPotionSlot;
    [SerializeField] GameObject epPotionSlot;

    [SerializeField] Text hpCount;
    [SerializeField] Text epCount;

    [SerializeField] Image hpCoolFill;
    [SerializeField] Image epCoolFill;

    [SerializeField] Image dodgeCoolFill;
    [SerializeField] Image tagCoolFill;

    [SerializeField] Image qSkillCoolFill;
    [SerializeField] Image wSkillCoolFill;
    [SerializeField] Image eSkillCoolFill;

    [SerializeField] Text playerScore;

    [SerializeField] GameObject gameMenu;

    private GameObject mainC1;
    private GameObject subC1;
    private GameObject mainC2;
    private GameObject subC2;

    private GameObject c1Slot;
    private GameObject c2Slot;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        InitializeUI();
    }
    void Update()
    {
        if (GameManager.instance.clientPlayer.character1Hp > 0 || GameManager.instance.clientPlayer.character2Hp > 0)
        {
            UpdateHp();
            UpdateCoolTimeUI();
            UpdatePlayerScore();
            if (Input.GetKeyDown(KeyCode.F))
            {
                if(PlayerManager.instance.onTag)
                {
                    TagCharacterMask();
                    TagCharacterSlot();
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameMenu.activeSelf)
                gameMenu.SetActive(false);
            else
                gameMenu.SetActive(true);
        }
    }

    void InitializeUI()
    {
        if (GameManager.instance.clientPlayer.selectCharacter1 == 1)
        {
            mainKarmenMask.SetActive(true);
            mainC1 = mainKarmenMask;
            subC1 = subKarmenMask;
            c1Slot = karmenSlot;
            c1Slot.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 2)
        {
            mainJadeMask.SetActive(true);
            mainC1 = mainJadeMask;
            subC1 = subJadeMask;
            c1Slot = jadeSlot;
            c1Slot.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 3)
        {
            mainLeinaMask.SetActive(true);
            mainC1 = mainLeinaMask;
            subC1 = subLeinaMask;
            c1Slot = leinaSlot;
            c1Slot.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 4)
        {
            mainEvaMask.SetActive(true);
            mainC1 = mainEvaMask;
            subC1 = subEvaMask;
            c1Slot = evaSlot;
            c1Slot.SetActive(true);
        }

        if (GameManager.instance.clientPlayer.selectCharacter2 == 1)
        {
            subKarmenMask.SetActive(true);
            mainC2 = mainKarmenMask;
            subC2 = subKarmenMask;
            c2Slot = karmenSlot;
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 2)
        {
            subJadeMask.SetActive(true);
            mainC2 = mainJadeMask;
            subC2 = subJadeMask;
            c2Slot = jadeSlot;
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 3)
        {
            subLeinaMask.SetActive(true);
            mainC2 = mainLeinaMask;
            subC2 = subLeinaMask;
            c2Slot = leinaSlot;
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 4)
        {
            subEvaMask.SetActive(true);
            mainC2 = mainEvaMask;
            subC2 = subEvaMask;
            c2Slot = evaSlot;
        }
    }

    void UpdateHp()
    {
        if (GameManager.instance.clientPlayer.curMainCharacter == 1)
        {
            imageMainHpFill.fillAmount = Convert.ToSingle(GameManager.instance.clientPlayer.character1Hp) / Convert.ToSingle(GameManager.instance.character1MaxHp);
            imageMainEpFill.fillAmount = Convert.ToSingle(GameManager.instance.clientPlayer.character1Ep) / Convert.ToSingle(GameManager.instance.character1MaxEp);
            imageSubHpFill.fillAmount = Convert.ToSingle(GameManager.instance.clientPlayer.character2Hp) / Convert.ToSingle(GameManager.instance.character2MaxHp);
            imageSubEpFill.fillAmount = Convert.ToSingle(GameManager.instance.clientPlayer.character2Ep) / Convert.ToSingle(GameManager.instance.character2MaxEp);

            textMainHp.text = string.Format("{0} / {1}", GameManager.instance.clientPlayer.character1Hp, GameManager.instance.character1MaxHp);
            textMainEp.text = string.Format("{0} / {1}", GameManager.instance.clientPlayer.character1Ep, GameManager.instance.character1MaxEp);
            textSubHp.text = string.Format("{0} / {1}", GameManager.instance.clientPlayer.character2Hp, GameManager.instance.character2MaxHp);
            textSubEp.text = string.Format("{0} / {1}", GameManager.instance.clientPlayer.character2Ep, GameManager.instance.character2MaxEp);
        }
        else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
        {
            imageMainHpFill.fillAmount = Convert.ToSingle(GameManager.instance.clientPlayer.character2Hp) / Convert.ToSingle(GameManager.instance.character2MaxHp);
            imageMainEpFill.fillAmount = Convert.ToSingle(GameManager.instance.clientPlayer.character2Ep) / Convert.ToSingle(GameManager.instance.character2MaxEp);
            imageSubHpFill.fillAmount = Convert.ToSingle(GameManager.instance.clientPlayer.character1Hp) / Convert.ToSingle(GameManager.instance.character1MaxHp);
            imageSubEpFill.fillAmount = Convert.ToSingle(GameManager.instance.clientPlayer.character1Ep) / Convert.ToSingle(GameManager.instance.character1MaxEp);

            textMainHp.text = string.Format("{0} / {1}", GameManager.instance.clientPlayer.character2Hp, GameManager.instance.character2MaxHp);
            textMainEp.text = string.Format("{0} / {1}", GameManager.instance.clientPlayer.character2Ep, GameManager.instance.character2MaxEp);
            textSubHp.text = string.Format("{0} / {1}", GameManager.instance.clientPlayer.character1Hp, GameManager.instance.character1MaxHp);
            textSubEp.text = string.Format("{0} / {1}", GameManager.instance.clientPlayer.character1Ep, GameManager.instance.character1MaxEp);
        }
    }

    void UpdateCoolTimeUI()
    {
        if (GameManager.instance.clientPlayer.curMainCharacter == 1)
        {
            dodgeCoolFill.fillAmount = 1 - PlayerManager.instance.curC1DodgeCoolTime / PlayerManager.instance.c1DodgeCoolTime;
            qSkillCoolFill.fillAmount = 1 - PlayerManager.instance.curC1QSkillCoolTime / PlayerManager.instance.c1QSkillCoolTime;
            wSkillCoolFill.fillAmount = 1 - PlayerManager.instance.curC1WSkillCoolTime / PlayerManager.instance.c1WSkillCoolTime;
            eSkillCoolFill.fillAmount = 1 - PlayerManager.instance.curC1ESkillCoolTime / PlayerManager.instance.c1ESkillCoolTime;
        }
        else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
        {
            dodgeCoolFill.fillAmount = 1 - PlayerManager.instance.curC2DodgeCoolTime / PlayerManager.instance.c2DodgeCoolTime;
            qSkillCoolFill.fillAmount = 1 - PlayerManager.instance.curC2QSkillCoolTime / PlayerManager.instance.c2QSkillCoolTime;
            wSkillCoolFill.fillAmount = 1 - PlayerManager.instance.curC2WSkillCoolTime / PlayerManager.instance.c2WSkillCoolTime;
            eSkillCoolFill.fillAmount = 1 - PlayerManager.instance.curC2ESkillCoolTime / PlayerManager.instance.c2ESkillCoolTime;
        }

        hpCoolFill.fillAmount = 1 - PlayerManager.instance.curHpCoolTime / PlayerManager.instance.hpCoolTime;
        epCoolFill.fillAmount = 1 - PlayerManager.instance.curEpCoolTime / PlayerManager.instance.epCoolTime;

        hpCount.text = string.Format("{0}", GameManager.instance.curHpPotionCount);
        epCount.text = string.Format("{0}", GameManager.instance.curEpPotionCount);

        tagCoolFill.fillAmount = 1 - PlayerManager.instance.curTagCoolTime / GameManager.instance.tagCoolTime;
    }

    void TagCharacterMask()
    {
        if (GameManager.instance.clientPlayer.curMainCharacter == 1)
        {
            mainC1.SetActive(false);
            subC1.SetActive(true);

            mainC2.SetActive(true);
            subC2.SetActive(false);
        }
        else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
        {
            mainC1.SetActive(true);
            subC1.SetActive(false);

            mainC2.SetActive(false);
            subC2.SetActive(true);
        }
    }

    void TagCharacterSlot()
    {
        if (GameManager.instance.clientPlayer.curMainCharacter == 1)
        {
            c1Slot.SetActive(false);
            c2Slot.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
        {
            c1Slot.SetActive(true);
            c2Slot.SetActive(false);
        }
    }

    void UpdatePlayerScore()
    {
        playerScore.text = string.Format("{0}", GameManager.instance.playerScore);
    }
}