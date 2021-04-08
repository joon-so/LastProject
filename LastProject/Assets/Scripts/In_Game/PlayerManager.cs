using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera = null;

    public GameObject C_Karmen;
    public GameObject C_Jade;
    public GameObject C_Leina;
    public GameObject C_Eva;

    private CameraController mainCameraControl;

    private Karmen _Karmen;
    private Jade _Jade;
    private Leina _Leina;
    private Eva _Eva;

    private bool isChange;

     void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        mainCameraControl = mainCamera.GetComponent<CameraController>();

        InitMainSub();

        isChange = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Tag();
        }
        Zoom();
        //Click();
    }

    void InitMainSub()
    {
        if (GameManager.instance.isMainKarmen)
        {
            C_Karmen.SetActive(true);
            C_Karmen.gameObject.tag = "MainCharacter";
            C_Karmen.gameObject.layer = 6;
            GameManager.instance.character1 = C_Karmen;
        }
        else if (GameManager.instance.isMainJade)
        {
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "MainCharacter";
            C_Jade.gameObject.layer = 6;
            GameManager.instance.character1 = C_Jade;
        }
        else if (GameManager.instance.isMainLeina)
        {
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "MainCharacter";
            C_Eva.gameObject.layer = 6;
            GameManager.instance.character1 = C_Leina;
        }
        else if (GameManager.instance.isMainEva)
        {
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "MainCharacter";
            C_Eva.gameObject.layer = 6;
            GameManager.instance.character1 = C_Eva;
        }

        if (GameManager.instance.isSubKarmen)
        {
            C_Karmen.SetActive(true);
            C_Karmen.gameObject.tag = "SubCharacter";
            C_Karmen.gameObject.layer = 7;
            GameManager.instance.character2 = C_Karmen;
        }
        else if (GameManager.instance.isSubJade)
        {
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "SubCharacter";
            C_Jade.gameObject.layer = 7;
            GameManager.instance.character2 = C_Jade;
        }
        else if (GameManager.instance.isSubLeina)
        {
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "SubCharacter";
            C_Jade.gameObject.layer = 7;
            GameManager.instance.character2 = C_Leina;
        }
        else if (GameManager.instance.isSubEva)
        {
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "SubCharacter";
            C_Eva.gameObject.layer = 7;
            GameManager.instance.character2 = C_Eva;
        }
    }

    void Tag()
    {
        // C1 : main -> sub
        if (!isChange)
        {
            mainCameraControl.focus = GameManager.instance.character2.transform;
            GameManager.instance.TagObject1();
            GameManager.instance.TagSkillSlot1();
            GameManager.instance.TagMask1();

            isChange = true;
        }
        // C1 : sub -> main
        else
        {
            mainCameraControl.focus = GameManager.instance.character1.transform;
            GameManager.instance.TagObject2();
            GameManager.instance.TagSkillSlot2();
            GameManager.instance.TagMask2();

            isChange = false;
        }
        // ½ºÅ³

        // hp
        GameManager.instance.TagHpEp();
        GameManager.instance.EffectFillBar();
    }

    void Zoom()
    {
        var scroll = Input.mouseScrollDelta;
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - scroll.y, 30f, 70f);
    }

    //void Click()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
    //        Physics.Raycast(ray, out hit);

    //        clickEffect.transform.position = hit.point;
    //        StartCoroutine(ActiveEffect());
    //    }
    //}

    //IEnumerator ActiveEffect()
    //{
    //    clickEffect.SetActive(true);
    //    yield return new WaitForSeconds(1f);
    //    clickEffect.SetActive(false);
    //}
}