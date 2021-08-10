using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] GameObject mainKarmen;
    [SerializeField] GameObject subKarmen;
    [SerializeField] GameObject mainJade;
    [SerializeField] GameObject subJade;
    [SerializeField] GameObject mainLeina;
    [SerializeField] GameObject subLeina;
    [SerializeField] GameObject mainEva;
    [SerializeField] GameObject subEva;
    [SerializeField] GameObject selectEffectKarmen;
    [SerializeField] GameObject selectEffectJade;
    [SerializeField] GameObject selectEffectLeina;
    [SerializeField] GameObject selectEffectEva;

    [SerializeField] GameObject characterInfoMsg;

    [SerializeField] Text characterInfoMsgText;

    private int mainOrsub;  // main: 1 sub: 2
    public AudioClip uiButtonClickSound1;
    public AudioClip uiButtonClickSound2;
    public AudioClip uiCharacterClickSound;

    void Start()
    {
        mainOrsub = 1;
    }

    public void PointUISound()
    {

    }

    public void OnClickSelectKarmen()
    {
        SoundManager.instance.SFXPlay("List", uiCharacterClickSound);
        if (mainOrsub == 1)
        {
            mainKarmen.SetActive(true);
            mainJade.SetActive(false);
            mainLeina.SetActive(false);
            mainEva.SetActive(false);
        }
        if (mainOrsub == 2)
        {
            subKarmen.SetActive(true);
            subJade.SetActive(false);
            subLeina.SetActive(false);
            subEva.SetActive(false);
        }
    }
    public void OnClickSelectJade()
    {
        SoundManager.instance.SFXPlay("List", uiCharacterClickSound);
        if (mainOrsub == 1)
        {
            mainKarmen.SetActive(false);
            mainJade.SetActive(true);
            mainLeina.SetActive(false);
            mainEva.SetActive(false);
        }
        if (mainOrsub == 2)
        {
            subKarmen.SetActive(false);
            subJade.SetActive(true);
            subLeina.SetActive(false);
            subEva.SetActive(false);
        }
    }
    public void OnClickSelectLeina()
    {
        SoundManager.instance.SFXPlay("List", uiCharacterClickSound);
        if (mainOrsub == 1)
        {
            mainKarmen.SetActive(false);
            mainJade.SetActive(false);
            mainLeina.SetActive(true);
            mainEva.SetActive(false);
        }
        if (mainOrsub == 2)
        {
            subKarmen.SetActive(false);
            subJade.SetActive(false);
            subLeina.SetActive(true);
            subEva.SetActive(false);
        }
    }
    public void OnClickSelectEva()
    {
        SoundManager.instance.SFXPlay("List", uiCharacterClickSound);
        if (mainOrsub == 1)
        {
            mainKarmen.SetActive(false);
            mainJade.SetActive(false);
            mainLeina.SetActive(false);
            mainEva.SetActive(true);
        }
        if (mainOrsub == 2)
        {
            subKarmen.SetActive(false);
            subJade.SetActive(false);
            subLeina.SetActive(false);
            subEva.SetActive(true);
        }
    }
    public void OnClickSelectCharacter()
    {
        SoundManager.instance.SFXPlay("Select", uiButtonClickSound1);

        if (mainOrsub == 1)
        {
            if (mainKarmen.activeSelf)
            {
                GameManager.instance.isMainKarmen = true;
                selectEffectKarmen.SetActive(true);
            }
            if (mainJade.activeSelf)
            {
                GameManager.instance.isMainJade = true;
                selectEffectJade.SetActive(true);
            }
            if (mainLeina.activeSelf)
            {
                GameManager.instance.isMainLeina = true;
                selectEffectLeina.SetActive(true);
            }
            if (mainEva.activeSelf)
            {
                GameManager.instance.isMainEva = true;
                selectEffectEva.SetActive(true);
            }
            mainOrsub = 2;
        }
        else if (mainOrsub == 2)
        {
            if (subKarmen.activeSelf)
            {
                GameManager.instance.isSubKarmen = true;
                selectEffectKarmen.SetActive(true);
            }
            if (subJade.activeSelf)
            {
                GameManager.instance.isSubJade = true;
                selectEffectJade.SetActive(true);
            }
            if (subLeina.activeSelf)
            {
                GameManager.instance.isSubLeina = true;
                selectEffectLeina.SetActive(true);
            }
            if (subEva.activeSelf)
            {
                GameManager.instance.isSubEva = true;
                selectEffectEva.SetActive(true);
            }
            mainOrsub = 0;
        }
    }

    public void OnClickStart()
    {
        SoundManager.instance.SFXPlay("Start", uiButtonClickSound2);
        SceneManager.LoadScene("Stage1-51");
    }
    public void OnClickExit()
    {
        SoundManager.instance.SFXPlay("Exit", uiButtonClickSound2);
        SceneManager.LoadScene("Main");
    }

    public void KarmenInfoMsg()
    {
        characterInfoMsgText.text = "카르멘은 근거리 전투 캐릭터입니다.";
    }
    public void JadeInfoMsg()
    {
        characterInfoMsgText.text = "제이드는 원거리 전투 캐릭터입니다.";
    }
    public void LeinaInfoMsg()
    {
        characterInfoMsgText.text = "레이나는 원거리 전투 캐릭터입니다.";
    }
    public void EvaInfoMsg()
    {
        characterInfoMsgText.text = "에바는 근거리 전투 캐릭터입니다.";
    }
}