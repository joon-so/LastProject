using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMyPlayerManager : MonoBehaviour
{
    public static ServerMyPlayerManager instance;

    [SerializeField] Camera mainCamera;
    private ServerCameraController mainCameraControl;

    [SerializeField] GameObject clickEffect;

    [SerializeField] GameObject serverKarmenObj;
    [SerializeField] GameObject serverJadeObj;
    [SerializeField] GameObject serverLeinaObj;
    [SerializeField] GameObject serverEvaObj;

    public GameObject character1;
    public GameObject character2;

    public string ID;
    private bool isTag;

    void Awake()
    {
        if (instance == null)
            instance = this;

        mainCameraControl = mainCamera.GetComponent<ServerCameraController>();
    }

    void Start()
    {

        if (ServerLoginManager.playerList[0].selectMainCharacter == 1)
        {
            serverKarmenObj.SetActive(true);
            character1 = serverKarmenObj;
            serverKarmenObj.tag = "MainCharacter";
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
        {
            serverJadeObj.SetActive(true);
            character1 = serverJadeObj;
            serverJadeObj.tag = "MainCharacter";
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
        {
            serverLeinaObj.SetActive(true);
            character1 = serverLeinaObj;
            serverLeinaObj.tag = "MainCharacter";
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
        {
            serverEvaObj.SetActive(true);
            character1 = serverEvaObj;
            serverEvaObj.tag = "MainCharacter";
        }

        if (ServerLoginManager.playerList[0].selectSubCharacter == 1)
        {
            serverKarmenObj.SetActive(true);
            character2 = serverKarmenObj;
            serverKarmenObj.tag = "SubCharacter";
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 2)
        {
            serverJadeObj.SetActive(true);
            character2 = serverJadeObj;
            serverJadeObj.tag = "SubCharacter";
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 3)
        {
            serverLeinaObj.SetActive(true);
            character2 = serverLeinaObj;
            serverLeinaObj.tag = "SubCharacter";
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 4)
        {
            serverEvaObj.SetActive(true);
            character2 = serverEvaObj;
            serverEvaObj.tag = "SubCharacter";
        }

        ID = ServerLoginManager.playerList[0].playerID;
        ServerLoginManager.playerList[0].is_Main_Character = 1;

        character1.transform.position = ServerLoginManager.playerList[0].mainCharacterPos;
        character1.transform.rotation = ServerLoginManager.playerList[0].mainCharacterRot;
        character2.transform.position = ServerLoginManager.playerList[0].subCharacterPos;
        character2.transform.rotation = ServerLoginManager.playerList[0].subCharacterRot;

        isTag = true;

        mainCameraControl.focus = character1.transform;

        StartCoroutine("CoSendPacket");
    }

    void Update()
    {
        Zoom();
        Click();
        if (Input.GetKeyDown(KeyCode.F))
        {
            ServerMainSubTag();
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
            RaycastHit hit;
            Physics.Raycast(ray, out hit);

            clickEffect.transform.position = hit.point;
            StartCoroutine(ActiveEffect()); 
        }
    }

    public void ServerMainSubTag()
    {
        // main->sub
        if (isTag)
        {
            mainCameraControl.focus = character2.transform;
            character1.gameObject.tag = "SubCharacter";
            character2.gameObject.tag = "MainCharacter";
            ServerLoginManager.playerList[0].is_Main_Character = 2;
            isTag = false;
        }
        else
        {
            mainCameraControl.focus = character1.transform;
            character1.gameObject.tag = "MainCharacter";
            character2.gameObject.tag = "SubCharacter";
            ServerLoginManager.playerList[0].is_Main_Character = 1;
            isTag = true;
        }
    }

    IEnumerator ActiveEffect()
    {
        clickEffect.SetActive(true);
        yield return new WaitForSeconds(1f);
        clickEffect.SetActive(false);
    }


    IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);
            cs_PlayerData movePacket = new cs_PlayerData();

            movePacket.ID = ID;

            if (character1.gameObject.tag == "MainCharacter")
            {
                movePacket.is_Main_Ch = 1;
                movePacket.mainPlayer_Behavior = ServerLoginManager.playerList[0].mainCharacterBehavior;
                movePacket.mainPlayer_Pos_X = character1.gameObject.transform.position.x;
                movePacket.mainPlayer_Pos_Z = character1.gameObject.transform.position.z;
                movePacket.mainPlayer_Rot_Y = character1.gameObject.transform.rotation.eulerAngles.y;

                movePacket.mainPlayer_Hp = 0;
                movePacket.mainPlayer_Mp = 0;

                movePacket.subPlayer_Behavior = ServerLoginManager.playerList[0].subCharacterBehavior;
                movePacket.subPlayer_Pos_X = character2.gameObject.transform.position.x;
                movePacket.subPlayer_Pos_Z = character2.gameObject.transform.position.z;
                movePacket.subPlayer_Rot_Y = character2.gameObject.transform.rotation.eulerAngles.y;
                movePacket.subPlayer_Hp = 0;
                movePacket.subPlayer_Mp = 0;
            }
            else if (character2.gameObject.tag == "MainCharacter")
            {
                movePacket.is_Main_Ch = 2;
                movePacket.mainPlayer_Behavior = ServerLoginManager.playerList[0].mainCharacterBehavior;
                movePacket.mainPlayer_Pos_X = character2.gameObject.transform.position.x;
                movePacket.mainPlayer_Pos_Z = character2.gameObject.transform.position.z;
                movePacket.mainPlayer_Rot_Y = character2.gameObject.transform.rotation.eulerAngles.y;

                movePacket.mainPlayer_Hp = 0;
                movePacket.mainPlayer_Mp = 0;

                movePacket.subPlayer_Behavior = ServerLoginManager.playerList[0].subCharacterBehavior;
                movePacket.subPlayer_Pos_X = character1.gameObject.transform.position.x;
                movePacket.subPlayer_Pos_Z = character1.gameObject.transform.position.z;
                movePacket.subPlayer_Rot_Y = character1.gameObject.transform.rotation.eulerAngles.y;

                movePacket.subPlayer_Hp = 0;
                movePacket.subPlayer_Mp = 0;
            }

            NetworkManager.instance.Send(movePacket.Write());
        }
    }
}
