using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private short c1MaxHp;
    private short c1MaxEp;
    private short c2MaxHp;
    private short c2MaxEp;

    //[Header("C1")]
    //public Image c1_QSkillImg;
    //public float c1_QSkillcoolDown;
    //bool c1_QSkillCoolDown = false;
    //public KeyCode c1_QSkillkey;

    //public Image c1_WSkillImg;
    //public float c1_WSkillcoolDown;
    //bool c1_WSkillCoolDown = false;
    //public KeyCode c1_WSkillkey;

    //public Image c1_ESkillImg;
    //public float c1_ESkillcoolDown;
    //bool c1_ESkillCoolDown = false;
    //public KeyCode c1_ESkillkey;

    //[Header("C2")]
    //public Image c2_QSkillImg;
    //public float c2_QSkillcoolDown;
    //bool c2_QSkillCoolDown = false;
    //public KeyCode c2_QSkillkey;

    //public Image c2_WSkillImg;
    //public float c2_WSkillcoolDown;
    //bool c2_WSkillCoolDown = false;
    //public KeyCode c2_WSkillkey;

    //public Image c2_ESkillImg;
    //public float c2_ESkillcoolDown;
    //bool c2_ESkillCoolDown = false;
    //public KeyCode c2_ESkillkey;

    void Start()
    {
        if (ServerLoginManager.playerList[0].selectMainCharacter == 1)
        {
            mainKarmenMask.SetActive(true);
            c1MaxHp = 500;
            c1MaxEp = 100;
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
        {
            mainJadeMask.SetActive(true);
            c1MaxHp = 400;
            c1MaxEp = 200;
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
        {
            mainLeinaMask.SetActive(true);
            c1MaxHp = 400;
            c1MaxEp = 200;
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
        {
            mainEvaMask.SetActive(true);
            c1MaxHp = 500;
            c1MaxEp = 100;
        }

        if (ServerLoginManager.playerList[0].selectSubCharacter == 1)
        {
            subKarmenMask.SetActive(true);
            c2MaxHp = 500;
            c2MaxEp = 100;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 2)
        {
            subJadeMask.SetActive(true);
            c2MaxHp = 400;
            c2MaxEp = 200;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 3)
        {
            subLeinaMask.SetActive(true);
            c2MaxHp = 400;
            c2MaxEp = 200;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 4)
        {
            subEvaMask.SetActive(true);
            c2MaxHp = 500;
            c2MaxEp = 100;
        }

        //c1_QSkillImg.fillAmount = 0;
        //c1_WSkillImg.fillAmount = 0;
        //c1_ESkillImg.fillAmount = 0;

        //c2_QSkillImg.fillAmount = 0;
        //c2_WSkillImg.fillAmount = 0;
        //c2_ESkillImg.fillAmount = 0;
    }
    void Update()
    {
        UpdateHp();
        //C1_QSkillCoolDownUI();
        //C1_WSkillCoolDownUI();

        //C2_QSkillCoolDownUI();
        //C2_WSkillCoolDownUI();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameMenu.activeSelf)
                gameMenu.SetActive(false);
            else
                gameMenu.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            SetCharacterMask();
        }
    }

    void SetCharacterMask()
    {
        if (ServerLoginManager.playerList[0].is_Main_Character == 1)
        {
            if (ServerLoginManager.playerList[0].selectMainCharacter == 1)
            {
                mainKarmenMask.SetActive(true);
                subKarmenMask.SetActive(false);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
            {
                mainJadeMask.SetActive(true);
                subJadeMask.SetActive(false);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
            {
                mainLeinaMask.SetActive(true);
                subLeinaMask.SetActive(false);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
            {
                mainEvaMask.SetActive(true);
                subEvaMask.SetActive(false);
            }
            if (ServerLoginManager.playerList[0].selectSubCharacter == 1)
            {
                mainKarmenMask.SetActive(false);
                subKarmenMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectSubCharacter == 2)
            {
                mainJadeMask.SetActive(false);
                subJadeMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectSubCharacter == 3)
            {
                mainLeinaMask.SetActive(false);
                subLeinaMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectSubCharacter == 4)
            {
                mainEvaMask.SetActive(false);
                subEvaMask.SetActive(true);
            }        }
        else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
        {
            if (ServerLoginManager.playerList[0].selectMainCharacter == 1)
            {
                mainKarmenMask.SetActive(false);
                subKarmenMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
            {
                mainJadeMask.SetActive(false);
                subJadeMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
            {
                mainLeinaMask.SetActive(false);
                subLeinaMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
            {
                mainEvaMask.SetActive(false);
                subEvaMask.SetActive(true);
            }

            if (ServerLoginManager.playerList[0].selectSubCharacter == 1)
            {
                mainKarmenMask.SetActive(true);
                subKarmenMask.SetActive(false);
            }
            else if (ServerLoginManager.playerList[0].selectSubCharacter == 2)
            {
                mainJadeMask.SetActive(true);
                subJadeMask.SetActive(false);
            }
            else if (ServerLoginManager.playerList[0].selectSubCharacter == 3)
            {
                mainLeinaMask.SetActive(true);
                subLeinaMask.SetActive(false);
            }
            else if (ServerLoginManager.playerList[0].selectSubCharacter == 4)
            {
                mainEvaMask.SetActive(true);
                subEvaMask.SetActive(false);
            }        }
    }

    void UpdateHp()
    {
        Debug.Log(ServerLoginManager.playerList[0].character1Hp);
        Debug.Log(ServerLoginManager.playerList[0].character1Ep);
        Debug.Log(ServerLoginManager.playerList[0].character2Hp);
        Debug.Log(ServerLoginManager.playerList[0].character2Ep);

        imageMainHpFill.fillAmount = ServerLoginManager.playerList[0].character1Hp / c1MaxHp;
        imageMainEpFill.fillAmount = ServerLoginManager.playerList[0].character1Ep / c1MaxEp;
        imageSubHpFill.fillAmount = ServerLoginManager.playerList[0].character2Hp / c2MaxHp;
        imageSubEpFill.fillAmount = ServerLoginManager.playerList[0].character2Ep / c2MaxEp;

        textMainHp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character1Hp, c1MaxHp);
        textMainEp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character1Ep, c1MaxEp);
        textSubHp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character2Hp, c2MaxHp);
        textSubEp.text = string.Format("{0} / {1}", ServerLoginManager.playerList[0].character2Ep, c2MaxEp);
    }

    //void ResetHp()
    //{
    //    GameManager.instance.mainPlayerHp = GameManager.instance.mainPlayerMaxHp;
    //    GameManager.instance.subPlayerHp = GameManager.instance.subPlayerMaxHp;
    //}
    //void ResetEp()
    //{
    //    GameManager.instance.mainPlayerEp = GameManager.instance.mainPlayerMaxEp;
    //    GameManager.instance.subPlayerEp = GameManager.instance.subPlayerMaxEp;
    //}

    //void C1_QSkillCoolDownUI()
    //{
    //    if (Input.GetKey(c1_QSkillkey) && c1_QSkillCoolDown == false)
    //    {
    //        c1_QSkillCoolDown = true;
    //        c1_QSkillImg.fillAmount = 1;
    //    }

    //    if (c1_QSkillCoolDown)
    //    {
    //        c1_QSkillImg.fillAmount -= 1 / c1_QSkillcoolDown * Time.deltaTime;

    //        if (c1_QSkillImg.fillAmount <= 0)
    //        {
    //            c1_QSkillImg.fillAmount = 0;
    //            c1_QSkillCoolDown = false;
    //        }
    //    }
    //}
    //void C1_WSkillCoolDownUI()
    //{
    //    if (Input.GetKey(c1_WSkillkey) && c1_WSkillCoolDown == false)
    //    {
    //        c1_WSkillCoolDown = true;
    //        c1_WSkillImg.fillAmount = 1;
    //    }

    //    if (c1_WSkillCoolDown)
    //    {
    //        c1_WSkillImg.fillAmount -= 1 / c1_WSkillcoolDown * Time.deltaTime;

    //        if (c1_WSkillImg.fillAmount <= 0)
    //        {
    //            c1_WSkillImg.fillAmount = 0;
    //            c1_WSkillCoolDown = false;
    //        }
    //    }
    //}
    //void C2_QSkillCoolDownUI()
    //{
    //    if (Input.GetKey(c2_QSkillkey) && c2_QSkillCoolDown == false)
    //    {
    //        c2_QSkillCoolDown = true;
    //        c2_QSkillImg.fillAmount = 1;
    //    }

    //    if (c2_QSkillCoolDown)
    //    {
    //        c2_QSkillImg.fillAmount -= 1 / c2_QSkillcoolDown * Time.deltaTime;

    //        if (c2_QSkillImg.fillAmount <= 0)
    //        {
    //            c2_QSkillImg.fillAmount = 0;
    //            c2_QSkillCoolDown = false;
    //        }
    //    }
    //}
    //void C2_WSkillCoolDownUI()
    //{
    //    if (Input.GetKey(c2_WSkillkey) && c2_WSkillCoolDown == false)
    //    {
    //        c2_WSkillCoolDown = true;
    //        c2_WSkillImg.fillAmount = 1;
    //    }

    //    if (c2_WSkillCoolDown)
    //    {
    //        c2_WSkillImg.fillAmount -= 1 / c2_WSkillcoolDown * Time.deltaTime;

    //        if (c2_WSkillImg.fillAmount <= 0)
    //        {
    //            c2_WSkillImg.fillAmount = 0;
    //            c2_WSkillCoolDown = false;
    //        }
    //    }
    //}
    public void OnClickOptionButton()
    {
    }
    public void OnClickTagButton()
    {
    }

    public void ClickResumButton()
    {

    }
}