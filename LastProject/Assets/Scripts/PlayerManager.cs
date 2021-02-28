using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera = null;
    [SerializeField] GameObject character1 = null;
    [SerializeField] GameObject character2 = null;
    [SerializeField] GameObject clickEffect = null;

    private CameraController mainCameraControl;
    private MainCharacter C1_mainScript;
    private SubCharacter C1_subScript;
    private MainCharacter C2_mainScript;
    private SubCharacter C2_subScript;
    
    RaycastHit hit;

    private bool isChange;

    void Start()
    {
        mainCameraControl = mainCamera.GetComponent<CameraController>();

        C1_mainScript = character1.GetComponent<MainCharacter>();
        C1_subScript = character1.GetComponent<SubCharacter>();
        C2_mainScript = character2.GetComponent<MainCharacter>();
        C2_subScript = character2.GetComponent<SubCharacter>();

        C1_mainScript.enabled = true;
        C1_subScript.enabled = false;
        C2_mainScript.enabled = false;
        C2_subScript.enabled = true;

        isChange = false;
        clickEffect.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            changeMainSub();
        }
        Zoom();
        Click();
    }

    void changeMainSub()
    {
        // C1 : main -> sub
        if (!isChange)
        {
            mainCameraControl.focus = character2.transform;

            C1_mainScript.enabled = false;
            C1_subScript.enabled = true;
            C2_mainScript.enabled = true;
            C2_subScript.enabled = false;
            isChange = true;
        }
        else
        {
            mainCameraControl.focus = character1.transform;

            C1_mainScript.enabled = true;
            C1_subScript.enabled = false;
            C2_mainScript.enabled = false;
            C2_subScript.enabled = true;
            isChange = false;
        }
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
            Physics.Raycast(ray, out hit);

            clickEffect.transform.position = hit.point;
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