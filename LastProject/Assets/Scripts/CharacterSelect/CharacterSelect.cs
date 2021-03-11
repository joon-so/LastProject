using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    private bool selectKarmen;
    private bool selectJade;
    private bool selectLeina;
    private bool selectEva;

    private int mainOrsub;  // main: 1 sub: 2

    void Start()
    {
        selectKarmen = false;
        selectJade = false;
        selectLeina = false;
        selectEva = false;

        mainOrsub = 1;
    }

    public void OnClickSelectKarmen()
    {
        selectKarmen = true;
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
        selectJade = true;
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
        selectLeina = true;
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
        selectEva = true;
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
        if (mainOrsub == 1)
        {
            if (selectKarmen)
            {
                //GameManager.instance.isKarmen = true;
            }
            if (selectJade)
            {
               // GameManager.instance.isJade = true;
            }
            if (selectLeina)
            {
                //GameManager.instance.isLeina = true;
            }
            if (selectEva)
            {
               // GameManager.instance.isEva = true;
            }
            mainOrsub = 2;
        }
        else if (mainOrsub == 2)
        {
            if (selectKarmen)
            {
                //GameManager.instance.isKarmen = true;
            }
            if (selectJade)
            {
                //GameManager.instance.isJade = true;
            }
            if (selectLeina)
            {
                //GameManager.instance.isLeina = true;
            }
            if (selectEva)
            {
               // GameManager.instance.isEva = true;
            }
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("ProtoType 1");
    }
    public void OnClickExit()
    {
        SceneManager.LoadScene("Main");
    }
}