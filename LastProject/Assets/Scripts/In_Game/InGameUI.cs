using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] Image imageMainHpFill;
    [SerializeField] Image imageMainEpFill;
    [SerializeField] Image imageSubHpFill;
    [SerializeField] Image imageSubEpFill;
    
    [SerializeField] Text textMainHp;
    [SerializeField] Text textMainEp;
    [SerializeField] Text textSubHp;
    [SerializeField] Text textSubEp;

    [SerializeField] Text playerKill;
    [SerializeField] Text playerDeath;
    [SerializeField] Text playerScore;

    [SerializeField] Text textOtherHp;
    [SerializeField] Text textOtherEp;

    [SerializeField] GameObject gameMenu;
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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
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
        UpdatePlayerScore();
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
    }

    void UpdateHp()
    {
        imageMainHpFill.fillAmount = GameManager.instance.mainPlayerHp / GameManager.instance.mainPlayerMaxHp;
        imageMainEpFill.fillAmount = GameManager.instance.mainPlayerEp / GameManager.instance.mainPlayerMaxEp;
        imageSubHpFill.fillAmount = GameManager.instance.subPlayerHp / GameManager.instance.subPlayerMaxHp;
        imageSubEpFill.fillAmount = GameManager.instance.subPlayerEp / GameManager.instance.subPlayerMaxEp;

        textMainHp.text = string.Format("{0}/{1}", GameManager.instance.mainPlayerHp, GameManager.instance.mainPlayerMaxHp);
        textMainEp.text = string.Format("{0}/{1}", GameManager.instance.mainPlayerEp, GameManager.instance.mainPlayerMaxEp);
        textSubHp.text = string.Format("{0}/{1}", GameManager.instance.subPlayerHp, GameManager.instance.subPlayerMaxHp);
        textSubEp.text = string.Format("{0}/{1}", GameManager.instance.subPlayerEp, GameManager.instance.subPlayerMaxEp);
    }

    void UpdatePlayerKD()
    {
        playerKill.text = string.Format("{0}", GameManager.instance.playerKill);
        playerDeath.text = string.Format("{0}", GameManager.instance.playerDeath);
    }

    void UpdatePlayerScore()
    {
        playerScore.text = string.Format("{0}", GameManager.instance.playerScore);
    }

    void ResetHp()
    {
        GameManager.instance.mainPlayerHp = GameManager.instance.mainPlayerMaxHp;
        GameManager.instance.subPlayerHp = GameManager.instance.subPlayerMaxHp;
    }
    void ResetEp()
    {
        GameManager.instance.mainPlayerEp = GameManager.instance.mainPlayerMaxEp;
        GameManager.instance.subPlayerEp = GameManager.instance.subPlayerMaxEp;
    }
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