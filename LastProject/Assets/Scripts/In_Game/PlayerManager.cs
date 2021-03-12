using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] Camera mainCamera = null;
    [SerializeField] GameObject clickEffect = null;

    private GameObject character1;
    private GameObject character2;
    private CameraController mainCameraControl;
    private NavMeshAgent C1_Nav;
    private NavMeshAgent C2_Nav;

    GameManager instance;

    RaycastHit hit;

    private bool isChange;

    void Start()
    {
        mainCameraControl = mainCamera.GetComponent<CameraController>();

        character1 = GameObject.FindWithTag("MainCharacter");
        character2 = GameObject.FindWithTag("SubCharacter");

        C1_Nav = character1.GetComponent<NavMeshAgent>();
        C2_Nav = character2.GetComponent<NavMeshAgent>();

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

            character1.gameObject.tag = "SubCharacter";
            character2.gameObject.tag = "MainCharacter";

            character1.gameObject.layer = 7;
            character2.gameObject.layer = 6;

            C1_Nav.enabled = true;
            C2_Nav.enabled = false;

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

            C1_Nav.enabled = false;
            C2_Nav.enabled = true;

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