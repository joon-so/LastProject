using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    void Start()
    {
        InitCharacterSetting();
        StartCoroutine(FillbarAndStageInfo());
    }

    void Update()
    {
        FillBar();
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
        if (GameManager.instance.mainPlayerHp < GameManager.instance.mainPlayerMaxHp)
        {
            GameManager.instance.mainPlayerHp += 1.0f;
        }
        if (GameManager.instance.mainPlayerEp < GameManager.instance.mainPlayerMaxEp)
        {
            GameManager.instance.mainPlayerEp += 1.0f;
        }
        if (GameManager.instance.subPlayerHp < GameManager.instance.subPlayerMaxHp)
        {
            GameManager.instance.subPlayerHp += 1.0f;
        }
        if (GameManager.instance.subPlayerEp < GameManager.instance.subPlayerMaxEp)
        {
            GameManager.instance.subPlayerEp += 1.0f;
        }
    }

    IEnumerator FillbarAndStageInfo()
    {
        yield return new WaitForSeconds(0.5f);
        stageInformation.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        stageInformation.SetActive(false);
    }
}
