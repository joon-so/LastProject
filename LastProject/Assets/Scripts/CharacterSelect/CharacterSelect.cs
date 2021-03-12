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

    private int mainOrsub;  // main: 1 sub: 2

    void Start()
    {
        GameManager.instance.isMainKarmen = false;
        GameManager.instance.isMainJade = false;
        GameManager.instance.isMainLeina = false;
        GameManager.instance.isMainEva = false;

        GameManager.instance.isSubKarmen = false;
        GameManager.instance.isSubJade = false;
        GameManager.instance.isSubLeina = false;
        GameManager.instance.isSubEva = false;

        mainOrsub = 1;
    }

    public void OnClickSelectKarmen()
    {
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
            if (mainKarmen.activeSelf)
            {
                GameManager.instance.isMainKarmen = true;
            }
            if (mainJade.activeSelf)
            {
                GameManager.instance.isMainJade = true;
            }
            if (mainLeina.activeSelf)
            {
                GameManager.instance.isMainLeina = true;
            }
            if (mainEva.activeSelf)
            {
                GameManager.instance.isMainEva = true;
            }
            mainOrsub = 2;
        }
        else if (mainOrsub == 2)
        {
            if (subKarmen.activeSelf)
            {
                GameManager.instance.isSubKarmen = true;
            }
            if (subJade.activeSelf)
            {
                GameManager.instance.isSubJade = true;
            }
            if (subLeina.activeSelf)
            {
                GameManager.instance.isSubLeina = true;
            }
            if (subEva.activeSelf)
            {
                GameManager.instance.isSubEva = true;
            }
        }
    }

    public void OnClickStart()
    {
        SceneManager.LoadScene("TagProtoType 1");
    }
    public void OnClickExit()
    {
        SceneManager.LoadScene("Main");
    }
}