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

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }
    void Update()
    {
        UpdateHp();
        UpdatePlayerScore();
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

    //    Debug.Log(textMainHp.text);
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

    public void OnClickOptionButton()
    {

    }
    public void OnClickTagButton()
    {
        
    }
}