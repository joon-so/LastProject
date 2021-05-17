using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameStartManager : MonoBehaviour
{
    [SerializeField] GameObject stageInformation;

    [SerializeField] GameObject mainKarmenMask;
    [SerializeField] GameObject mainJadeMask;
    [SerializeField] GameObject mainLeinaMask;
    [SerializeField] GameObject mainEvaMask;

    [SerializeField] GameObject subKarmenMask;
    [SerializeField] GameObject subJadeMask;
    [SerializeField] GameObject subLeinaMask;
    [SerializeField] GameObject subEvaMask;

    [SerializeField] GameObject mainKarmenSkillSlot;
    [SerializeField] GameObject mainJadeSkillSlot;
    [SerializeField] GameObject mainLeinaSkillSlot;
    [SerializeField] GameObject mainEvaSkillSlot;

    [SerializeField] GameObject subKarmenSkillSlot;
    [SerializeField] GameObject subJadeSkillSlot;
    [SerializeField] GameObject subLeinaSkillSlot;
    [SerializeField] GameObject subEvaSkillSlot;

    private bool isFillBar;

    void Start()
    {
        isFillBar = false;

        InitCharacterSetting();

        StartCoroutine(FillbarAndStageInfo());

        FillBar();
    }

    void Update()
    {
        //if (!isFillBar)
        //    FillBar();
    }

    void InitCharacterSetting()
    {
        if (GameManager.instance.isMainKarmen)
        {
            SetHp(500, true);
            SetEp(150, true);
            GameManager.instance.mainMaskCharacter1 = mainKarmenMask;
            GameManager.instance.subMaskCharacter1 = subKarmenMask;
            GameManager.instance.mainMaskCharacter1.SetActive(true);
            GameManager.instance.mainCharacter1SkillSlot = mainKarmenSkillSlot;
            GameManager.instance.subCharacter1SkillSlot = subKarmenSkillSlot;
            GameManager.instance.mainCharacter1SkillSlot.SetActive(true);
            GameManager.instance.c1_QSkillCoolTime = Karmen.qSkillCoolTime;
            GameManager.instance.c1_WSkillCoolTime = Karmen.wSkillCoolTime;
            GameManager.instance.c1_ESkillCoolTime = Karmen.eSkillCoolTime;
        }
        else if (GameManager.instance.isMainJade)
        {
            SetHp(400, true);
            SetEp(200, true);
            GameManager.instance.mainMaskCharacter1 = mainJadeMask;
            GameManager.instance.subMaskCharacter1 = subJadeMask;
            GameManager.instance.mainMaskCharacter1.SetActive(true);
            GameManager.instance.mainCharacter1SkillSlot = mainJadeSkillSlot;
            GameManager.instance.subCharacter1SkillSlot = subJadeSkillSlot;
            GameManager.instance.mainCharacter1SkillSlot.SetActive(true);
            GameManager.instance.c1_QSkillCoolTime = Jade.qSkillCoolTime;
            GameManager.instance.c1_WSkillCoolTime = Jade.wSkillCoolTime;
            GameManager.instance.c1_ESkillCoolTime = Jade.eSkillCoolTime;
        }
        else if (GameManager.instance.isMainLeina)
        {
            SetHp(400, true);
            SetEp(200, true);
            GameManager.instance.mainMaskCharacter1 = mainLeinaMask;
            GameManager.instance.subMaskCharacter1 = subLeinaMask;
            GameManager.instance.mainMaskCharacter1.SetActive(true);
            GameManager.instance.mainCharacter1SkillSlot = mainLeinaSkillSlot;
            GameManager.instance.subCharacter1SkillSlot = subLeinaSkillSlot;
            GameManager.instance.mainCharacter1SkillSlot.SetActive(true);
            GameManager.instance.c1_QSkillCoolTime = Leina.qSkillCoolTime;
            GameManager.instance.c1_WSkillCoolTime = Leina.wSkillCoolTime;
            GameManager.instance.c1_ESkillCoolTime = Leina.eSkillCoolTime;
        }
        else if (GameManager.instance.isMainEva)
        {
            SetHp(600, true);
            SetEp(150, true);
            GameManager.instance.mainMaskCharacter1 = mainEvaMask;
            GameManager.instance.subMaskCharacter1 = subEvaMask;
            GameManager.instance.mainMaskCharacter1.SetActive(true);
            GameManager.instance.mainCharacter1SkillSlot = mainEvaSkillSlot;
            GameManager.instance.subCharacter1SkillSlot = subEvaSkillSlot;
            GameManager.instance.mainCharacter1SkillSlot.SetActive(true);
            GameManager.instance.c1_QSkillCoolTime = Eva.qSkillCoolTime;
            GameManager.instance.c1_WSkillCoolTime = Eva.wSkillCoolTime;
            GameManager.instance.c1_ESkillCoolTime = Eva.eSkillCoolTime;
        }

        if (GameManager.instance.isSubKarmen)
        {
            SetHp(500, false);
            SetEp(150, false);
            GameManager.instance.mainMaskCharacter2 = mainKarmenMask;
            GameManager.instance.subMaskCharacter2 = subKarmenMask;
            GameManager.instance.subMaskCharacter2.SetActive(true);
            GameManager.instance.mainCharacter2SkillSlot = mainKarmenSkillSlot;
            GameManager.instance.subCharacter2SkillSlot = subKarmenSkillSlot;
            GameManager.instance.subCharacter2SkillSlot.SetActive(true);
            GameManager.instance.c2_QSkillCoolTime = Karmen.qSkillCoolTime;
            GameManager.instance.c2_WSkillCoolTime = Karmen.wSkillCoolTime;
            GameManager.instance.c2_ESkillCoolTime = Karmen.eSkillCoolTime;
        }
        else if (GameManager.instance.isSubJade)
        {
            SetHp(400, false);
            SetEp(200, false);
            GameManager.instance.mainMaskCharacter2 = mainJadeMask;
            GameManager.instance.subMaskCharacter2 = subJadeMask;
            GameManager.instance.subMaskCharacter2.SetActive(true);
            GameManager.instance.mainCharacter2SkillSlot = mainJadeSkillSlot;
            GameManager.instance.subCharacter2SkillSlot = subJadeSkillSlot;
            GameManager.instance.subCharacter2SkillSlot.SetActive(true);
            GameManager.instance.c2_QSkillCoolTime = Jade.qSkillCoolTime;
            GameManager.instance.c2_WSkillCoolTime = Jade.wSkillCoolTime;
            GameManager.instance.c2_ESkillCoolTime = Jade.eSkillCoolTime;
        }
        else if (GameManager.instance.isSubLeina)
        {
            SetHp(400, false);
            SetEp(200, false);
            GameManager.instance.mainMaskCharacter2 = mainLeinaMask;
            GameManager.instance.subMaskCharacter2 = subLeinaMask;
            GameManager.instance.subMaskCharacter2.SetActive(true);
            GameManager.instance.mainCharacter2SkillSlot = mainLeinaSkillSlot;
            GameManager.instance.subCharacter2SkillSlot = subLeinaSkillSlot;
            GameManager.instance.subCharacter2SkillSlot.SetActive(true);
            GameManager.instance.c2_QSkillCoolTime = Leina.qSkillCoolTime;
            GameManager.instance.c2_WSkillCoolTime = Leina.wSkillCoolTime;
            GameManager.instance.c2_ESkillCoolTime = Leina.eSkillCoolTime;
        }
        else if (GameManager.instance.isSubEva)
        {
            SetHp(600, false);
            SetEp(150, false);
            GameManager.instance.mainMaskCharacter2 = mainEvaMask;
            GameManager.instance.subMaskCharacter2 = subEvaMask;
            GameManager.instance.subMaskCharacter2.SetActive(true);
            GameManager.instance.mainCharacter2SkillSlot = mainEvaSkillSlot;
            GameManager.instance.subCharacter2SkillSlot = subEvaSkillSlot;
            GameManager.instance.subCharacter2SkillSlot.SetActive(true);
            GameManager.instance.c2_QSkillCoolTime = Eva.qSkillCoolTime;
            GameManager.instance.c2_WSkillCoolTime = Eva.wSkillCoolTime;
            GameManager.instance.c2_ESkillCoolTime = Eva.eSkillCoolTime;
        }
    }

    void SetHp(float hp, bool isMain)
    {
        if (isMain)
        {
            GameManager.instance.mainPlayerMaxHp = hp;
        }
        else
        {
            GameManager.instance.subPlayerMaxHp = hp;
        }
    }
    void SetEp(float ep, bool isMain)
    {
        if (isMain)
        {
            GameManager.instance.mainPlayerMaxEp = ep;
        }
        else
        {
            GameManager.instance.subPlayerMaxEp = ep;
        }
    }

    void FillBar()
    {
        GameManager.instance.mainPlayerHp = GameManager.instance.mainPlayerMaxHp;
        GameManager.instance.mainPlayerEp = GameManager.instance.mainPlayerMaxEp;
        GameManager.instance.subPlayerHp = GameManager.instance.subPlayerMaxHp;
        GameManager.instance.subPlayerEp = GameManager.instance.subPlayerMaxEp;
    }

    IEnumerator FillbarAndStageInfo()
    {
        yield return new WaitForSeconds(0.5f);
        stageInformation.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        stageInformation.SetActive(false);
    }
}
