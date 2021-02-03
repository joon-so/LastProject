using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera = default;

    [SerializeField]
    GameObject character1 = default;

    [SerializeField]
    GameObject character2 = default;

    private CameraController mainCameraControl;

    private MainCharacter C1_mainScript;
    private SubCharacter C1_subScript;
    private MainCharacter C2_mainScript;
    private SubCharacter C2_subScript;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            changeMainSub();
        }
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
}
