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
    void Start()
    {
        if (GameManager.instance.isMainKarmen)
        {
            SetHp(500, true);
            SetEp(150, true);
            GameManager.instance.mainMask = mainKarmenMask;
            GameManager.instance.mainMask.SetActive(true);
        }
        else if (GameManager.instance.isMainJade)
        {
            SetHp(400, true);
            SetEp(200, true);
            GameManager.instance.mainMask = mainJadeMask;
            GameManager.instance.mainMask.SetActive(true);
        }
        else if (GameManager.instance.isMainLeina)
        {
            SetHp(400, true);
            SetEp(200, true);
            GameManager.instance.mainMask = mainLeinaMask;
            GameManager.instance.mainMask.SetActive(true);
        }
        else if (GameManager.instance.isMainEva)
        {
            SetHp(600, true);
            SetEp(150, true);
            GameManager.instance.mainMask = mainEvaMask;
            GameManager.instance.mainMask.SetActive(true);
        }

        if (GameManager.instance.isSubKarmen)
        {
            SetHp(500, false);
            SetEp(150, false);
            GameManager.instance.subMask = subKarmenMask;
            GameManager.instance.subMask.SetActive(true);
        }
        else if (GameManager.instance.isSubJade)
        {
            SetHp(400, false);
            SetEp(200, false);
            GameManager.instance.subMask = subJadeMask;
            GameManager.instance.subMask.SetActive(true);
        }
        else if (GameManager.instance.isSubLeina)
        {
            SetHp(400, false);
            SetEp(200, false);
            GameManager.instance.subMask = subLeinaMask;
            GameManager.instance.subMask.SetActive(true);
        }
        else if (GameManager.instance.isSubEva)
        {
            SetHp(600, false);
            SetEp(150, false);
            GameManager.instance.subMask = subEvaMask;
            GameManager.instance.subMask.SetActive(true);
        }

        StartCoroutine(FillbarAndStageInfo());
    }

    void Update()
    {
        FillHpBar();
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

    void FillHpBar()
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
        FillHpBar();
        yield return new WaitForSeconds(0.5f);
        stageInformation.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        stageInformation.SetActive(false);
    }
}
