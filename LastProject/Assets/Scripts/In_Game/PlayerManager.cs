using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera = null;
    //[SerializeField] GameObject clickEffect = null;

    public GameObject C_Karmen;
    public GameObject C_Jade;
    public GameObject C_Eva;

    private GameObject character1;
    private GameObject character2;
    private CameraController mainCameraControl;
    private NavMeshAgent C1_Nav;
    private NavMeshAgent C2_Nav;

    private bool isChange;
    void Start()
    {
        mainCameraControl = mainCamera.GetComponent<CameraController>();

        Debug.Log(GameManager.instance.isMainKarmen);
        Debug.Log(GameManager.instance.isSubJade);

        if (GameManager.instance.isMainKarmen)
        {
            Debug.Log("메인 카르멘");
            C_Karmen.SetActive(true);
            C_Karmen.gameObject.tag = "MainCharacter";
            C_Karmen.gameObject.layer = 6;
            character1 = C_Karmen;
        }
        else if (GameManager.instance.isMainJade)
        {
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "MainCharacter";
            C_Jade.gameObject.layer = 6;
            character1 = C_Jade;
        }
        else if (GameManager.instance.isMainEva)
        {
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "MainCharacter";
            C_Eva.gameObject.layer = 6;
            character1 = C_Eva;
        }

        if (GameManager.instance.isSubKarmen)
        {
            C_Karmen.SetActive(true);
            C_Karmen.gameObject.tag = "SubCharacter";
            C_Karmen.gameObject.layer = 7;
            character2 = C_Karmen;
        }
        else if (GameManager.instance.isSubJade)
        {
            Debug.Log("서브 제이드");
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "SubCharacter";
            C_Jade.gameObject.layer = 7;
            character2 = C_Jade;
        }
        else if (GameManager.instance.isSubEva)
        {
            Debug.Log("서브 제이드");
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "SubCharacter";
            C_Eva.gameObject.layer = 7;
            character2 = C_Eva;
        }

        //if (GameManager.instance.isMainKarmen)
        //{
        //    GameObject child = transform.Find("Karmen").gameObject;
        //    character1 = child;
        //    character1.gameObject.tag = "MainCharacter";
        //    character1.gameObject.layer = 6;
        //    character1.SetActive(true);
        //}
        //else if (GameManager.instance.isMainJade)
        //{
        //    GameObject child = transform.Find("Jade").gameObject;
        //    character1 = child;
        //    character1.gameObject.tag = "MainCharacter";
        //    character1.gameObject.layer = 6;
        //    character1.SetActive(true);
        //}

        //if (GameManager.instance.isSubKarmen)
        //{
        //    GameObject child = transform.Find("Karmen").gameObject;
        //    character2 = child;
        //    character2.gameObject.tag = "SubCharacter";
        //    character2.gameObject.layer = 7;
        //    character2.SetActive(true);
        //}
        //else if (GameManager.instance.isSubJade)
        //{
        //    GameObject child = transform.Find("Jade").gameObject;
        //    character2 = child;
        //    character2.gameObject.tag = "SubCharacter";
        //    character2.gameObject.layer = 7;
        //    character2.SetActive(true);
        //}

        //C1_Nav = character1.GetComponent<NavMeshAgent>();
        //C2_Nav = character2.GetComponent<NavMeshAgent>();

        //C1_Nav.enabled = false;
        //C2_Nav.enabled = true;

        isChange = false;
        //clickEffect.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            changeMainSub();
        }
        Zoom();
        //Click();
    }

    void changeMainSub()
    {
        // C1 : main -> sub
        if (!isChange)
        {
            mainCameraControl.focus = character2.transform;

            character1.gameObject.tag = "SubCharacter";
            character2.gameObject.tag = "MainCharacter";

            character1.gameObject.layer = 7;
            character2.gameObject.layer = 6;

            //C1_Nav.enabled = true;
            //C2_Nav.enabled = false;

            isChange = true;
        }
        // C1 : sub -> main
        else
        {
            mainCameraControl.focus = character1.transform;

            character1.gameObject.tag = "MainCharacter";
            character2.gameObject.tag = "SubCharacter";

            character1.gameObject.layer = 6;
            character2.gameObject.layer = 7;

            //C1_Nav.enabled = false;
            //C2_Nav.enabled = true;

            isChange = false;
        }
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