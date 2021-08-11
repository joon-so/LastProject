using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectManager : MonoBehaviour
{
    [SerializeField] GameObject mainKarmenObj;
    [SerializeField] GameObject mainJadeObj;
    [SerializeField] GameObject mainLeinaObj;
    [SerializeField] GameObject mainEvaObj;

    [SerializeField] GameObject subKarmenObj;
    [SerializeField] GameObject subJadeObj;
    [SerializeField] GameObject subLeinaObj;
    [SerializeField] GameObject subEvaObj;

    [SerializeField] GameObject selectEffectKarmen;
    [SerializeField] GameObject selectEffectJade;
    [SerializeField] GameObject selectEffectLeina;
    [SerializeField] GameObject selectEffectEva;

    [SerializeField] GameObject characterInfoMsg;

    [SerializeField] Text characterInfoMsgText;

    private int mainOrsub;  // main: 1 sub: 2

    private int mainCharacterIndex; // karmen : 1 eva :4
    private int subCharacterIndex;


    public AudioClip uiButtonClickSound1;
    public AudioClip uiButtonClickSound2;
    public AudioClip uiCharacterClickSound;

    [Header("Character Hp Ep Setting")]
    [SerializeField] int karmenHp;
    [SerializeField] int karmenEp;
    [SerializeField] int jadeHp;
    [SerializeField] int jadeEp;
    [SerializeField] int leinaHp;
    [SerializeField] int leinaEp;
    [SerializeField] int evaHp;
    [SerializeField] int evaEp;

    ClientCollisionManager clientCollisionManager;
    ClientSkillEpManager clientSkillEpManager;

    void Start()
    {
        clientCollisionManager = GameObject.Find("GameManager").GetComponent<ClientCollisionManager>();
        clientSkillEpManager = GameObject.Find("GameManager").GetComponent<ClientSkillEpManager>();

        mainOrsub = 1;
    }

    public void OnClickSelectKarmen()
    {
        SoundManager.instance.SFXPlay("List", uiCharacterClickSound);

        if (mainOrsub == 1)
        {
            mainKarmenObj.SetActive(true);
            mainJadeObj.SetActive(false);
            mainLeinaObj.SetActive(false);
            mainEvaObj.SetActive(false);
        }
        else if (mainOrsub == 2)
        {
            if (mainCharacterIndex == 1)
                return;

            subKarmenObj.SetActive(true);
            subJadeObj.SetActive(false);
            subLeinaObj.SetActive(false);
            subEvaObj.SetActive(false);
        }
    }
    public void OnClickSelectJade()
    {
        SoundManager.instance.SFXPlay("List", uiCharacterClickSound);

        if (mainOrsub == 1)
        {
            mainKarmenObj.SetActive(false);
            mainJadeObj.SetActive(true);
            mainLeinaObj.SetActive(false);
            mainEvaObj.SetActive(false);
        }
        else if (mainOrsub == 2)
        {
            if (mainCharacterIndex == 2)
                return;
            subKarmenObj.SetActive(false);
            subJadeObj.SetActive(true);
            subLeinaObj.SetActive(false);
            subEvaObj.SetActive(false);
        }
    }
    public void OnClickSelectLeina()
    {
        SoundManager.instance.SFXPlay("List", uiCharacterClickSound);

        if (mainOrsub == 1)
        {
            mainKarmenObj.SetActive(false);
            mainJadeObj.SetActive(false);
            mainLeinaObj.SetActive(true);
            mainEvaObj.SetActive(false);
        }
        else if (mainOrsub == 2)
        {
            if (mainCharacterIndex == 3)
                return;
            subKarmenObj.SetActive(false);
            subJadeObj.SetActive(false);
            subLeinaObj.SetActive(true);
            subEvaObj.SetActive(false);
        }
    }
    public void OnClickSelectEva()
    {
        SoundManager.instance.SFXPlay("List", uiCharacterClickSound);

        if (mainOrsub == 1)
        {
            mainKarmenObj.SetActive(false);
            mainJadeObj.SetActive(false);
            mainLeinaObj.SetActive(false);
            mainEvaObj.SetActive(true);
        }
        else if (mainOrsub == 2)
        {
            if (mainCharacterIndex == 4)
                return;
            subKarmenObj.SetActive(false);
            subJadeObj.SetActive(false);
            subLeinaObj.SetActive(false);
            subEvaObj.SetActive(true);
        }
    }
    public void OnClickSelectCharacter()
    {
        SoundManager.instance.SFXPlay("Select", uiButtonClickSound1);

        if (mainOrsub == 1)
        {
            if (mainKarmenObj.activeSelf)
            {
                mainCharacterIndex = 1;
                selectEffectKarmen.SetActive(true);
                GameManager.instance.clientPlayer.selectCharacter1 = 1;
                GameManager.instance.clientPlayer.character1Hp = karmenHp;
                GameManager.instance.clientPlayer.character1Ep = karmenEp;
                GameManager.instance.character1MaxHp = karmenHp;
                GameManager.instance.character1MaxEp = karmenEp;
                mainOrsub = 2;
            }
            else if (mainJadeObj.activeSelf)
            {
                mainCharacterIndex = 2;
                selectEffectJade.SetActive(true);
                GameManager.instance.clientPlayer.selectCharacter1 = 2;
                GameManager.instance.clientPlayer.character1Hp = jadeHp;
                GameManager.instance.clientPlayer.character1Ep = jadeEp;
                GameManager.instance.character1MaxHp = jadeHp;
                GameManager.instance.character1MaxEp = jadeEp;
                mainOrsub = 2;
            }
            else if (mainLeinaObj.activeSelf)
            {
                mainCharacterIndex = 3;
                selectEffectLeina.SetActive(true);
                GameManager.instance.clientPlayer.selectCharacter1 = 3;
                GameManager.instance.clientPlayer.character1Hp = leinaHp;
                GameManager.instance.clientPlayer.character1Ep = leinaEp;
                GameManager.instance.character1MaxHp = leinaHp;
                GameManager.instance.character1MaxEp = leinaEp;
                mainOrsub = 2;
            }
            else if (mainEvaObj.activeSelf)
            {
                mainCharacterIndex = 4;
                selectEffectEva.SetActive(true);
                GameManager.instance.clientPlayer.selectCharacter1 = 4;
                GameManager.instance.clientPlayer.character1Hp = evaHp;
                GameManager.instance.clientPlayer.character1Ep = evaEp;
                GameManager.instance.character1MaxHp = evaHp;
                GameManager.instance.character1MaxEp = evaEp;
                mainOrsub = 2;
            }
        }
        else if (mainOrsub == 2)
        {
            if (subKarmenObj.activeSelf)
            {
                subCharacterIndex = 1;
                selectEffectKarmen.SetActive(true);
                GameManager.instance.clientPlayer.selectCharacter2 = 1;
                GameManager.instance.clientPlayer.character2Hp = karmenHp;
                GameManager.instance.clientPlayer.character2Ep = karmenEp;
                GameManager.instance.character2MaxHp = karmenHp;
                GameManager.instance.character2MaxEp = karmenEp;
                mainOrsub = 0;
            }
            else if (subJadeObj.activeSelf)
            {
                subCharacterIndex = 2;
                selectEffectJade.SetActive(true);
                GameManager.instance.clientPlayer.selectCharacter2 = 2;
                GameManager.instance.clientPlayer.character2Hp = jadeHp;
                GameManager.instance.clientPlayer.character2Ep = jadeEp;
                GameManager.instance.character2MaxHp = jadeHp;
                GameManager.instance.character2MaxEp = jadeEp;
                mainOrsub = 0;
            }
            else if (subLeinaObj.activeSelf)
            {
                subCharacterIndex = 3;
                selectEffectLeina.SetActive(true);
                GameManager.instance.clientPlayer.selectCharacter2 = 3;
                GameManager.instance.clientPlayer.character2Hp = leinaHp;
                GameManager.instance.clientPlayer.character2Ep = leinaEp;
                GameManager.instance.character2MaxHp = leinaHp;
                GameManager.instance.character2MaxEp = leinaEp;
                mainOrsub = 0;
            }
            else if (subEvaObj.activeSelf)
            {
                subCharacterIndex = 4;
                selectEffectEva.SetActive(true);
                GameManager.instance.clientPlayer.selectCharacter2 = 4;
                GameManager.instance.clientPlayer.character2Hp = evaHp;
                GameManager.instance.clientPlayer.character2Ep = evaEp;
                GameManager.instance.character2MaxHp = evaHp;
                GameManager.instance.character2MaxEp = evaEp;
                mainOrsub = 0;
            }
        }
    }

    public void OnClickStart()
    {
        GameManager.instance.clientPlayer.curMainCharacter = 1;

        SoundManager.instance.SFXPlay("Start", uiButtonClickSound2);
        SceneManager.LoadScene("Stage1-1");
    }
    public void OnClickExit()
    {
        SoundManager.instance.SFXPlay("Exit", uiButtonClickSound2);
        SceneManager.LoadScene("Main");
    }

    public void KarmenInfoMsg()
    {
        characterInfoMsgText.text = "ī������ �ٰŸ� ���� ĳ�����Դϴ�." + "\n" +
                                    "Hp : " + karmenHp + "\n" +
                                    "Ep : " + karmenEp + "\n" +
                                    "�⺻ ���ݷ� : " + clientCollisionManager.karmenAttackDamage + "\n" +
                                    "Q��ų ���ݷ� : " + clientCollisionManager.karmenQSkillDamage + "\n" +
                                    "W��ų ���ݷ� : " + clientCollisionManager.karmenWSkillDamage + "\n" +
                                    "E��ų ���ݷ� : " + clientCollisionManager.karmenESkillDamage + "\n" +
                                    "������� �κ��� óġ�ϰ��������? �������ּ���.\n";
    }
    public void JadeInfoMsg()
    {
        characterInfoMsgText.text = "���̵�� ���Ÿ� ���� ĳ�����Դϴ�." + "\n" +
                                    "Hp : " + jadeHp + "\n" +
                                    "Ep : " + jadeEp + "\n" +
                                    "�⺻ ���ݷ� : " + clientCollisionManager.jadeAttackDamage + "\n" +
                                    "Q��ų ���ݷ� : " + clientCollisionManager.jadeQSkillDamage + "\n" +
                                    "W��ų ���ݷ� : " + clientCollisionManager.jadeWSkillDamage + "\n" +
                                    "E��ų ���ݷ� : " + clientCollisionManager.jadeESkillDamage + "\n" +
                                    "������ �κ��� óġ�ϰ��������? �������ּ���.\n";
    }
    public void LeinaInfoMsg()
    {
        characterInfoMsgText.text = "���̳��� ���Ÿ� ���� ĳ�����Դϴ�." + "\n" +
                                    "Hp : " + leinaHp + "\n" +
                                    "Ep : " + leinaEp + "\n" +
                                    "�⺻ ���ݷ� : " + clientCollisionManager.leinaAttackDamage + "\n" +
                                    "Q��ų ���ݷ� : " + clientCollisionManager.leinaQSkillDamage + "\n" +
                                    "W��ų ���ݷ� : " + clientCollisionManager.leinaWSkillDamage + "\n" +
                                    "E��ų ���ݷ� : " + clientCollisionManager.leinaESkillDamage + "\n" +
                                    "ȭ��� �κ��� óġ�ϰ��������? �������ּ���.\n";
    }
    public void EvaInfoMsg()
    {
        characterInfoMsgText.text = "���ٴ� �ٰŸ� ���� ĳ�����Դϴ�." + "\n" +
                                    "Hp : " + evaHp + "\n" +
                                    "Ep : " + evaEp + "\n" +
                                    "�⺻ ���ݷ� : " + clientCollisionManager.evaAttackDamage + "\n" +
                                    "Q��ų ���ݷ� : " + clientCollisionManager.evaQSkillDamage + "\n" +
                                    "W��ų ���ݷ� : " + clientCollisionManager.evaWSkillDamage + "\n" +
                                    "E��ų ���ݷ� : " + clientCollisionManager.evaESkillDamage + "\n" +
                                    "�����ϰ� �κ��� óġ�ϰ��������? �������ּ���.\n";
    }
}