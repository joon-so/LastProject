using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

    [SerializeField] GameObject mainCharacterEffect;
    [SerializeField] GameObject subCharacterEffect;

    public GameObject character1;
    public GameObject character2;

    public string ID;

    // item
    public int myHpPotionCount;
    public int myEpPotionCount;

    // skill coolTime
    public float c1DodgeCoolTime;
    public float c1QSkillCoolTime;
    public float c1WSkillCoolTime;
                 
    public float c2DodgeCoolTime;
    public float c2QSkillCoolTime;
    public float c2WSkillCoolTime;

    public float curC1DodgeCoolTime;
    public float curC1QSkillCoolTime;
    public float curC1WSkillCoolTime;

    public float curC2DodgeCoolTime;
    public float curC2QSkillCoolTime;
    public float curC2WSkillCoolTime;

    private short c1MaxHp;
    private short c1MaxEp;
    private short c2MaxHp;
    private short c2MaxEp;

    public float hpCoolTime;
    public float epCoolTime;

    public float curHpCoolTime;
    public float curEpCoolTime;

    private bool onHpPotion;
    private bool onEpPotion;

    // tag
    public float tagCoolTime;

    public bool onTag;
    private bool isTag;
    public float curTagCoolTime;

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
            character1.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            character1.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            serverKarmenObj.tag = "MainCharacter";
            c1MaxHp = 500;
            c1MaxEp = 100;
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
        {
            serverJadeObj.SetActive(true);
            character1 = serverJadeObj;
            character1.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            character1.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            serverJadeObj.tag = "MainCharacter";
            c1MaxHp = 400;
            c1MaxEp = 200;
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
        {
            serverLeinaObj.SetActive(true);
            character1 = serverLeinaObj;
            character1.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            character1.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            serverLeinaObj.tag = "MainCharacter";
            c1MaxHp = 400;
            c1MaxEp = 200;
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
        {
            serverEvaObj.SetActive(true);
            character1 = serverEvaObj;
            character1.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            character1.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            serverEvaObj.tag = "MainCharacter";
            c1MaxHp = 500;
            c1MaxEp = 100;
        }

        if (ServerLoginManager.playerList[0].selectSubCharacter == 1)
        {
            serverKarmenObj.SetActive(true);
            character2 = serverKarmenObj;
            character2.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            serverKarmenObj.tag = "SubCharacter";
            c2MaxHp = 500;
            c2MaxEp = 100;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 2)
        {
            serverJadeObj.SetActive(true);
            character2 = serverJadeObj;
            character2.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            serverJadeObj.tag = "SubCharacter";
            c2MaxHp = 400;
            c2MaxEp = 200;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 3)
        {
            serverLeinaObj.SetActive(true);
            character2 = serverLeinaObj;
            character2.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            serverLeinaObj.tag = "SubCharacter";
            c2MaxHp = 400;
            c2MaxEp = 200;
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 4)
        {
            serverEvaObj.SetActive(true);
            character2 = serverEvaObj;
            character2.gameObject.GetComponent<NavMeshAgent>().enabled = true;
            character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;
            serverEvaObj.tag = "SubCharacter";
            c2MaxHp = 500;
            c2MaxEp = 100;
        }

        ID = ServerLoginManager.playerList[0].playerID;
        ServerLoginManager.playerList[0].is_Main_Character = 1;

        character1.transform.position = ServerLoginManager.playerList[0].mainCharacterPos;
        character1.transform.rotation = ServerLoginManager.playerList[0].mainCharacterRot;
        character2.transform.position = ServerLoginManager.playerList[0].subCharacterPos;
        character2.transform.rotation = ServerLoginManager.playerList[0].subCharacterRot;

        mainCameraControl.focus = character1.transform;

        myHpPotionCount = 2;
        myEpPotionCount = 2;

        hpCoolTime = 3;
        epCoolTime = 3;

        curHpCoolTime = hpCoolTime;
        curEpCoolTime = epCoolTime;

        onHpPotion = true;
        onEpPotion = true;

        tagCoolTime = 5.0f;
        curTagCoolTime = tagCoolTime;
        isTag = true;

        StartCoroutine("CoSendPacket");
    }

    void Update()
    {
        if (ServerLoginManager.playerList[0].character1Hp <= 0 || ServerLoginManager.playerList[0].character2Hp <= 0)
        {
            // 플레이어 사망
            //ServerIngameManager.instance.TimeSclaeSetZero();
        }
        else
        {
            if (curTagCoolTime < tagCoolTime)
                curTagCoolTime += Time.deltaTime;
            else
                onTag = true;

            if (Input.GetKeyDown(KeyCode.F))
            {
                ServerMainSubTag();
            }
            if(Input.GetKeyDown(KeyCode.F1))
            {
                ChangeHpEp();
            }
            if (Input.GetKeyDown(KeyCode.F2))
            {
                ChangeTime();
            }

            Click();
            MainSubEffect();
            PotionCount();
        }
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
        if (onTag)
        {
            curTagCoolTime = 0.0f;
            onTag = false;

            // main->sub
            if (isTag)
            {
                mainCameraControl.focus = character2.transform;
                character1.gameObject.tag = "SubCharacter";
                character2.gameObject.tag = "MainCharacter";

                character1.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                character2.gameObject.GetComponent<NavMeshAgent>().enabled = false;

                character1.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                character2.gameObject.GetComponent<Rigidbody>().isKinematic = false;

                ServerLoginManager.playerList[0].is_Main_Character = 2;
                isTag = false;
            }
            else
            {
                mainCameraControl.focus = character1.transform;
                character1.gameObject.tag = "MainCharacter";
                character2.gameObject.tag = "SubCharacter";

                character1.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                character2.gameObject.GetComponent<NavMeshAgent>().enabled = true;

                character1.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                ServerLoginManager.playerList[0].is_Main_Character = 1;
                isTag = true;
            }
        }
    }

    public void MainSubEffect()
    {
        if (isTag)
        {
            mainCharacterEffect.transform.position = new Vector3(character1.transform.position.x, 0.2f, character1.transform.position.z);
            subCharacterEffect.transform.position = new Vector3(character2.transform.position.x, 0.2f, character2.transform.position.z);
        }
        else
        {
            mainCharacterEffect.transform.position = new Vector3(character2.transform.position.x, 0.2f, character2.transform.position.z);
            subCharacterEffect.transform.position = new Vector3(character1.transform.position.x, 0.2f, character1.transform.position.z);
        }
    }

    void ChangeHpEp()
    {
        ServerLoginManager.playerList[0].character1Hp = 100;
        ServerLoginManager.playerList[0].character2Hp = 100;
        ServerLoginManager.playerList[0].character1Ep = 50;
        ServerLoginManager.playerList[0].character2Ep = 50;
    }

    void ChangeTime()
    {
        cs_SetTime settimePacket = new cs_SetTime();
        settimePacket.time = 10.0f;

        NetworkManager.instance.Send(settimePacket.Write());
    }

    void PotionCount()
    {
        if (curHpCoolTime < hpCoolTime)
            curHpCoolTime += Time.deltaTime;
        else
            onHpPotion = true;
        if (curEpCoolTime < epCoolTime)
            curEpCoolTime += Time.deltaTime;
        else
            onEpPotion = true;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(onHpPotion)
            {
                if (ServerLoginManager.playerList[0].is_Main_Character == 1)
                {
                    if (ServerLoginManager.playerList[0].character1Hp >= c1MaxHp)
                        return;

                    if (myHpPotionCount > 0)
                    {
                        myHpPotionCount -= 1;
                        curHpCoolTime = 0;
                        onHpPotion = false;
                        ServerLoginManager.playerList[0].character1Hp += ServerItemManager.instance.hpValue;
                        if (ServerLoginManager.playerList[0].character1Hp > c1MaxHp)
                            ServerLoginManager.playerList[0].character1Hp = c1MaxHp;
                    }
                }
                else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                {
                    if (ServerLoginManager.playerList[0].character2Hp >= c2MaxHp)
                        return;

                    if (myHpPotionCount > 0)
                    {
                        myHpPotionCount -= 1;
                        curHpCoolTime = 0;
                        onHpPotion = false;
                        ServerLoginManager.playerList[0].character2Hp += ServerItemManager.instance.hpValue;
                        if (ServerLoginManager.playerList[0].character2Hp > c2MaxHp)
                            ServerLoginManager.playerList[0].character2Hp = c2MaxHp;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(onEpPotion)
            {
                if (ServerLoginManager.playerList[0].is_Main_Character == 1)
                {
                    if (ServerLoginManager.playerList[0].character1Ep >= c1MaxEp)
                        return;

                    if (myEpPotionCount > 0)
                    {
                        myEpPotionCount -= 1;
                        curEpCoolTime = 0;
                        onEpPotion = false;
                        ServerLoginManager.playerList[0].character1Ep += ServerItemManager.instance.epValue;
                        if (ServerLoginManager.playerList[0].character1Ep > c1MaxEp)
                            ServerLoginManager.playerList[0].character1Ep = c1MaxEp;
                    }
                }
                else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                {
                    if (ServerLoginManager.playerList[0].character2Ep >= c2MaxEp)
                        return;

                    if (myEpPotionCount > 0)
                    {
                        myEpPotionCount -= 1;
                        curEpCoolTime = 0;
                        onEpPotion = false;
                        ServerLoginManager.playerList[0].character2Ep += ServerItemManager.instance.epValue;
                        if (ServerLoginManager.playerList[0].character2Ep > c2MaxEp)
                            ServerLoginManager.playerList[0].character2Ep = c2MaxEp;
                    }
                }
            }
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

            if(character1.CompareTag("MainCharacter"))
            {
                movePacket.is_Main_Ch = 1;

                movePacket.mainPlayer_Behavior = ServerLoginManager.playerList[0].mainCharacterBehavior;
                movePacket.mainPlayer_Pos_X = character1.gameObject.transform.position.x;
                movePacket.mainPlayer_Pos_Z = character1.gameObject.transform.position.z;
                movePacket.mainPlayer_Rot_Y = character1.gameObject.transform.rotation.eulerAngles.y;

                movePacket.mainPlayer_Hp = ServerLoginManager.playerList[0].character1Hp;
                movePacket.mainPlayer_Mp = ServerLoginManager.playerList[0].character1Ep;

                movePacket.subPlayer_Behavior = ServerLoginManager.playerList[0].subCharacterBehavior;
                movePacket.subPlayer_Pos_X = character2.gameObject.transform.position.x;
                movePacket.subPlayer_Pos_Z = character2.gameObject.transform.position.z;
                movePacket.subPlayer_Rot_Y = character2.gameObject.transform.rotation.eulerAngles.y;

                movePacket.subPlayer_Hp = ServerLoginManager.playerList[0].character2Hp;
                movePacket.subPlayer_Mp = ServerLoginManager.playerList[0].character2Ep;
            }
            else if (character2.CompareTag("MainCharacter"))
            {
                movePacket.is_Main_Ch = 2;

                movePacket.mainPlayer_Behavior = ServerLoginManager.playerList[0].mainCharacterBehavior;
                movePacket.mainPlayer_Pos_X = character2.gameObject.transform.position.x;
                movePacket.mainPlayer_Pos_Z = character2.gameObject.transform.position.z;
                movePacket.mainPlayer_Rot_Y = character2.gameObject.transform.rotation.eulerAngles.y;

                movePacket.mainPlayer_Hp = ServerLoginManager.playerList[0].character2Hp;
                movePacket.mainPlayer_Mp = ServerLoginManager.playerList[0].character2Ep;

                movePacket.subPlayer_Behavior = ServerLoginManager.playerList[0].subCharacterBehavior;
                movePacket.subPlayer_Pos_X = character1.gameObject.transform.position.x;
                movePacket.subPlayer_Pos_Z = character1.gameObject.transform.position.z;
                movePacket.subPlayer_Rot_Y = character1.gameObject.transform.rotation.eulerAngles.y;

                movePacket.subPlayer_Hp = ServerLoginManager.playerList[0].character1Hp;
                movePacket.subPlayer_Mp = ServerLoginManager.playerList[0].character1Ep;
            }

            NetworkManager.instance.Send(movePacket.Write());
        }
    }
}
