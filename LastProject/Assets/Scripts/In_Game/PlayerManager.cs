using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;
    [SerializeField] Camera mainCamera = default;
    [SerializeField] GameObject clickEffect = default;

    public GameObject C_Karmen;
    public GameObject C_Jade;
    public GameObject C_Leina;
    public GameObject C_Eva;

    private CameraController mainCameraControl;

    private bool isChange;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

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
        Click();
    }

    void InitMainSub()
    {
        if (GameManager.instance.isMainKarmen)
        {
            C_Karmen.SetActive(true);
            C_Karmen.gameObject.tag = "MainCharacter";
            C_Karmen.gameObject.layer = 6;
            GameManager.instance.character1 = C_Karmen;
            GameManager.instance.c1_QSkillCoolTime = Karmen.qSkillCoolTime;
        }
        else if (GameManager.instance.isMainJade)
        {
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "MainCharacter";
            C_Jade.gameObject.layer = 6;
            GameManager.instance.character1 = C_Jade;
            GameManager.instance.c1_QSkillCoolTime = Jade.qSkillCoolTime;
        }
        else if (GameManager.instance.isMainLeina)
        {
            C_Leina.SetActive(true);
            C_Leina.gameObject.tag = "MainCharacter";
            C_Leina.gameObject.layer = 6;
            GameManager.instance.character1 = C_Leina;
            GameManager.instance.c1_QSkillCoolTime = Leina.qSkillCoolTime;
        }
        else if (GameManager.instance.isMainEva)
        {
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "MainCharacter";
            C_Eva.gameObject.layer = 6;
            GameManager.instance.character1 = C_Eva;
            GameManager.instance.c1_QSkillCoolTime = Eva.qSkillCoolTime;
        }

        if (GameManager.instance.isSubKarmen)
        {
            C_Karmen.SetActive(true);
            C_Karmen.gameObject.tag = "SubCharacter";
            C_Karmen.gameObject.layer = 7;
            GameManager.instance.character2 = C_Karmen;
            GameManager.instance.c2_QSkillCoolTime = Karmen.qSkillCoolTime;
        }
        else if (GameManager.instance.isSubJade)
        {
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "SubCharacter";
            C_Jade.gameObject.layer = 7;
            GameManager.instance.character2 = C_Jade;
            GameManager.instance.c2_QSkillCoolTime = Jade.qSkillCoolTime;
        }
        else if (GameManager.instance.isSubLeina)
        {
            C_Leina.SetActive(true);
            C_Leina.gameObject.tag = "SubCharacter";
            C_Leina.gameObject.layer = 7;
            GameManager.instance.character2 = C_Leina;
            GameManager.instance.c2_QSkillCoolTime = Leina.qSkillCoolTime;
        }
        else if (GameManager.instance.isSubEva)
        {
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "SubCharacter";
            C_Eva.gameObject.layer = 7;
            GameManager.instance.character2 = C_Eva;
            GameManager.instance.c2_QSkillCoolTime = Eva.qSkillCoolTime;
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
            GameManager.instance.character1.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            GameManager.instance.character2.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            isChange = true;
        }
        // C1 : sub -> main
        else
        {
            mainCameraControl.focus = GameManager.instance.character1.transform;
            GameManager.instance.TagObject2();
            GameManager.instance.TagSkillSlot2();
            GameManager.instance.TagMask2();
            GameManager.instance.character1.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            GameManager.instance.character2.gameObject.GetComponent<NavMeshAgent>().enabled = true;


            isChange = false;
        }
        // ½ºÅ³

        // hp
        GameManager.instance.TagHpEp();
        //GameManager.instance.EffectFillBar();
    }
    
    void Zoom()
    {
        var scroll = Input.mouseScrollDelta;
        mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - scroll.y, 30f, 70f);
    }

    void Click()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            clickEffect.transform.position = hit.point;
            if (clickEffect.transform.position.y > 0.0f)
            {
                clickEffect.transform.position = new Vector3(clickEffect.transform.position.x, 0.0f, clickEffect.transform.position.z);
            }
            StartCoroutine(ActiveEffect());
        }
    }

    IEnumerator ActiveEffect()
    {
        clickEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        clickEffect.SetActive(false);
    }
}