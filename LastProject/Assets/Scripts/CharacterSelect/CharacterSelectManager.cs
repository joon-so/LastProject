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

    [SerializeField] AudioClip uiButtonSound;
    [SerializeField] AudioClip uiStartButtonSound;
    [SerializeField] AudioClip uiCharacterClickSound;

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
    void Start()
    {
        clientCollisionManager = GameObject.Find("GameManager").GetComponent<ClientCollisionManager>();
        mainOrsub = 1;
    }

    public void OnClickSelectKarmen()
    {
        SoundManager.instance.SFXPlay("CharacterListClick", uiCharacterClickSound);

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
        SoundManager.instance.SFXPlay("CharacterListClick", uiCharacterClickSound);

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
        SoundManager.instance.SFXPlay("CharacterListClick", uiCharacterClickSound);

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
        SoundManager.instance.SFXPlay("CharacterListClick", uiCharacterClickSound);

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
        SoundManager.instance.SFXPlay("UIButtonClick", uiButtonSound);

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
        if (GameManager.instance.clientPlayer.selectCharacter1 == 0 || GameManager.instance.clientPlayer.selectCharacter2 == 0)
            return;

        GameManager.instance.clientPlayer.curMainCharacter = 1;

        if (DataBaseManager.PlayerPvELevel ==0)
        {
            SoundManager.instance.SFXPlay("UIStartButtonClick", uiStartButtonSound);
            Invoke("LoadStage0", 1f);
        }
        else
        {
            SoundManager.instance.SFXPlay("UIStartButtonClick", uiStartButtonSound);
            Invoke("LoadPvELevel", 1f);
        }
    }
    void LoadStage0()
    {
        SceneManager.LoadScene("Stage0");
    }
    void LoadPvELevel()
    {
        SceneManager.LoadScene("LoadPvELevel");
    }

    public void OnClickExit()
    {
        SoundManager.instance.SFXPlay("UIButtonClick", uiButtonSound);
        GameManager.instance.DestroyGameManager();
        Invoke("LoadSelectPvEMode", 1f);
    }
    void LoadSelectPvEMode()
    {
        SceneManager.LoadScene("SelectPvEMode");
    }

    public void KarmenInfoMsg()
    {
        characterInfoMsgText.text = "카르멘은 근거리 전투 캐릭터입니다." + "\n" +
                                    "Hp : " + karmenHp + "\n" +
                                    "Ep : " + karmenEp + "\n" +
                                    "기본공격(곤봉을 휘두름)\t\t\t\t\t\t공격력 : " + clientCollisionManager.karmenAttackDamage + "\n" +
                                    "Q스킬(기를 모아 한번 공격)\t\t\t\t\t공격력 : " + clientCollisionManager.karmenQSkillDamage + "\n" +
                                    "W스킬(빠른속도로 연속 공격)\t\t\t\t공격력 : " + clientCollisionManager.karmenWSkillDamage + "\n" +
                                    "E스킬(커다란 곤봉을 휘두름)\t\t\t\t\t공격력 : " + clientCollisionManager.karmenESkillDamage + "\n" +
                                    "곤봉으로 로봇을 처치하고싶으세요? 선택해주세요.\n";
    }
    public void JadeInfoMsg()
    {
        characterInfoMsgText.text = "제이드는 원거리 전투 캐릭터입니다." + "\n" +
                                    "Hp : " + jadeHp + "\n" +
                                    "Ep : " + jadeEp + "\n" +
                                    "기본공격(총알 발사)\t\t\t\t\t\t\t\t공격력 : " + clientCollisionManager.jadeAttackDamage + "\n" +
                                    "Q스킬(미사일 발사)\t\t\t\t\t\t\t\t공격력 : " + clientCollisionManager.jadeQSkillDamage + "\n" +
                                    "W스킬(수료탄 발사)\t\t\t\t\t\t\t\t공격력 : " + clientCollisionManager.jadeWSkillDamage + "\n" +
                                    "E스킬(10M반경으로 유도탄 발사)\t\t\t공격력 : " + clientCollisionManager.jadeESkillDamage + "\n" +
                                    "총으로 로봇을 처치하고싶으세요? 선택해주세요.\n";
    }
    public void LeinaInfoMsg()
    {
        characterInfoMsgText.text = "레이나는 원거리 전투 캐릭터입니다." + "\n" +
                                    "Hp : " + leinaHp + "\n" +
                                    "Ep : " + leinaEp + "\n" +
                                    "기본공격(화살 발사)\t\t\t\t\t\t\t\t공격력 : " + clientCollisionManager.leinaAttackDamage + "\n" +
                                    "Q스킬(큰 화살 발사)\t\t\t\t\t\t\t\t공격력 : " + clientCollisionManager.leinaQSkillDamage + "\n" +
                                    "W스킬(여러 방향으로 화살 발사)\t\t\t공격력 : " + clientCollisionManager.leinaWSkillDamage + "\n" +
                                    "E스킬(하늘에서 수많은 화살발사)\t\t\t공격력 : " + clientCollisionManager.leinaESkillDamage + "\n" +
                                    "화살로 로봇을 처치하고싶으세요? 선택해주세요.\n";
    }
    public void EvaInfoMsg()
    {
        characterInfoMsgText.text = "에바는 근거리 전투 캐릭터입니다." + "\n" +
                                    "Hp : " + evaHp + "\n" +
                                    "Ep : " + evaEp + "\n" +
                                    "기본공격(칼을 휘두름)\t\t\t\t\t\t\t공격력 : " + clientCollisionManager.evaAttackDamage + "\n" +
                                    "Q스킬(화염방사기 발사)\t\t\t\t\t\t공격력 : " + clientCollisionManager.evaQSkillDamage + "\n" +
                                    "W스킬(땅을 내려쳐 충격파 생성)\t\t\t공격력 : " + clientCollisionManager.evaWSkillDamage + "\n" +
                                    "E스킬(10M반경으로 적을 공격)\t\t\t\t공격력 : " + clientCollisionManager.evaESkillDamage + "\n" +
                                    "무식하게 로봇을 처치하고싶으세요? 선택해주세요.\n";
    }
}