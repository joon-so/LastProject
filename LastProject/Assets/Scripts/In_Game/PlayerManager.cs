using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    [SerializeField] Camera mainCamera;
    private CameraController mainCameraControl;

    [SerializeField] GameObject clickEffect;

    public GameObject C_Karmen;
    public GameObject C_Jade;
    public GameObject C_Leina;
    public GameObject C_Eva;

    [SerializeField] GameObject mainCharacterEffect;
    [SerializeField] GameObject subCharacterEffect;

    // Skill CoolTime
    public float c1DodgeCoolTime;
    public float c1QSkillCoolTime;
    public float c1WSkillCoolTime;
    public float c1ESkillCoolTime;

    public float c2DodgeCoolTime;
    public float c2QSkillCoolTime;
    public float c2WSkillCoolTime;
    public float c2ESkillCoolTime;

    public float curC1DodgeCoolTime;
    public float curC1QSkillCoolTime;
    public float curC1WSkillCoolTime;
    public float curC1ESkillCoolTime;

    public float curC2DodgeCoolTime;
    public float curC2QSkillCoolTime;
    public float curC2WSkillCoolTime;
    public float curC2ESkillCoolTime;

    // Item CoolTime
    public float hpCoolTime;
    public float epCoolTime;

    public float curHpCoolTime;
    public float curEpCoolTime;

    private bool onHpPotion;
    private bool onEpPotion;

    // tag
    public bool onTag;
    private bool isTag;
    public float curTagCoolTime;

    public bool initTargetVec;

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
        Initialize();
        isTag = true;
        onTag = true;

        hpCoolTime = 3;
        epCoolTime = 3;

        curHpCoolTime = hpCoolTime;
        curEpCoolTime = epCoolTime;

        curTagCoolTime = GameManager.instance.tagCoolTime;
    }
    void Update()
    {
        if (GameManager.instance.clientPlayer.character1Hp <= 0 || GameManager.instance.clientPlayer.character2Hp <= 0)
        {
            // 플레이어 사망 

        }
        if (GameManager.instance.clientPlayer.character1Ep <= 0)
            GameManager.instance.clientPlayer.character1Ep = 0;
        if (GameManager.instance.clientPlayer.character2Ep <= 0)
            GameManager.instance.clientPlayer.character2Ep = 0;

        else
        {
            if (curTagCoolTime < GameManager.instance.tagCoolTime)
                curTagCoolTime += Time.deltaTime;
            else
                onTag = true;

            CheatMode();
            Zoom();
            Click();
            MainSubEffect();
            PotionCount();
        }
    }

    void Initialize()
    {
        if (GameManager.instance.clientPlayer.selectCharacter1 == 1)
        {
            C_Karmen.SetActive(true);
            C_Karmen.gameObject.tag = "MainCharacter";
            C_Karmen.gameObject.layer = 6;

            GameManager.instance.character1 = C_Karmen;

        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 2)
        {
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "MainCharacter";
            C_Jade.gameObject.layer = 6;

            GameManager.instance.character1 = C_Jade;
        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 3)
        {
            C_Leina.SetActive(true);
            C_Leina.gameObject.tag = "MainCharacter";
            C_Leina.gameObject.layer = 6;
            GameManager.instance.character1 = C_Leina;
        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 4)
        {
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "MainCharacter";
            C_Eva.gameObject.layer = 6;
            GameManager.instance.character1 = C_Eva;
        }

        if (GameManager.instance.clientPlayer.selectCharacter2 == 1)
        {
            C_Karmen.SetActive(true);
            C_Karmen.gameObject.tag = "SubCharacter";
            C_Karmen.gameObject.layer = 7;
            GameManager.instance.character2 = C_Karmen;
            GameManager.instance.character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 2)
        {
            C_Jade.SetActive(true);
            C_Jade.gameObject.tag = "SubCharacter";
            C_Jade.gameObject.layer = 7;
            GameManager.instance.character2 = C_Jade;
            GameManager.instance.character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 3)
        {
            C_Leina.SetActive(true);
            C_Leina.gameObject.tag = "SubCharacter";
            C_Leina.gameObject.layer = 7;
            GameManager.instance.character2 = C_Leina;
            GameManager.instance.character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 4)
        {
            C_Eva.SetActive(true);
            C_Eva.gameObject.tag = "SubCharacter";
            C_Eva.gameObject.layer = 7;
            GameManager.instance.character2 = C_Eva;
            GameManager.instance.character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    void CheatMode()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            MainSubTag();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            GameManager.instance.ChangeHpEp();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            GameManager.instance.ChangeSceneStage1();
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            GameManager.instance.ChangeSceneStage2();
        }
        if (Input.GetKeyDown(KeyCode.F3))
        {
            GameManager.instance.ChangeSceneStage3();
        }
        if (Input.GetKeyDown(KeyCode.F4))
        {
            GameManager.instance.ChangeSceneStage4();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            GameManager.instance.ChangeSceneStage1To2();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            GameManager.instance.ChangeSceneStage2To3();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GameManager.instance.ChangeSceneStage3To4();
        }
        if (Input.GetKeyDown(KeyCode.F9))
        {
            GameManager.instance.ChangeSceneBoss1PageEnter();
        }
        if (Input.GetKeyDown(KeyCode.F10))
        {
            GameManager.instance.ChangeSceneBoss2PageEnter();
        }
        if (Input.GetKeyDown(KeyCode.F11))
        {
            GameManager.instance.ChangeSceneBoss3PageEnter();
        }
    }
    void MainSubTag()
    {
        if (onTag)
        {
            curTagCoolTime = 0.0f;
            onTag = false;

            // C1 : main -> sub
            if (isTag)
            {
                mainCameraControl.focus = GameManager.instance.character2.transform;
                GameManager.instance.character1.gameObject.tag = "SubCharacter";
                GameManager.instance.character1.gameObject.layer = 7;
                GameManager.instance.character2.gameObject.tag = "MainCharacter";
                GameManager.instance.character2.gameObject.layer = 6;

                GameManager.instance.character1.gameObject.GetComponent<NavMeshAgent>().enabled = true;
                GameManager.instance.character2.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                
                GameManager.instance.character1.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                GameManager.instance.character2.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                
                GameManager.instance.clientPlayer.curMainCharacter = 2;
                isTag = false;
            }
            // C1 : sub -> main
            else
            {
                mainCameraControl.focus = GameManager.instance.character1.transform;
                GameManager.instance.character1.gameObject.tag = "MainCharacter";
                GameManager.instance.character1.gameObject.layer = 6;
                GameManager.instance.character2.gameObject.tag = "SubCharacter";
                GameManager.instance.character2.gameObject.layer = 7;

                GameManager.instance.character1.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                GameManager.instance.character2.gameObject.GetComponent<NavMeshAgent>().enabled = true;

                GameManager.instance.character1.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                GameManager.instance.character2.gameObject.GetComponent<Rigidbody>().isKinematic = true;

                GameManager.instance.clientPlayer.curMainCharacter = 1;
                isTag = true;
            }
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
            if (clickEffect.transform.position.y > 0.0f)
            {
                clickEffect.transform.position = new Vector3(clickEffect.transform.position.x, 0.0f, clickEffect.transform.position.z);
            }
            StartCoroutine(ActiveEffect());
        }
    }
    public void MainSubEffect()
    {
        if (isTag)
        {
            mainCharacterEffect.transform.position = new Vector3(GameManager.instance.character1.transform.position.x, GameManager.instance.character1.transform.position.y + 0.2f, GameManager.instance.character1.transform.position.z);
            subCharacterEffect.transform.position = new Vector3(GameManager.instance.character2.transform.position.x, GameManager.instance.character2.transform.position.y + 0.2f, GameManager.instance.character2.transform.position.z);
        }
        else
        {
            mainCharacterEffect.transform.position = new Vector3(GameManager.instance.character2.transform.position.x, GameManager.instance.character2.transform.position.y + 0.2f, GameManager.instance.character2.transform.position.z);
            subCharacterEffect.transform.position = new Vector3(GameManager.instance.character1.transform.position.x, GameManager.instance.character1.transform.position.y + 0.2f, GameManager.instance.character1.transform.position.z);
        }
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
            if (onHpPotion)
            {
                if (GameManager.instance.clientPlayer.curMainCharacter == 1)
                {
                    if (GameManager.instance.clientPlayer.character1Hp >= GameManager.instance.character1MaxHp)
                        return;

                    if (GameManager.instance.curHpPotionCount > 0)
                    {
                        GameManager.instance.curHpPotionCount -= 1;
                        curHpCoolTime = 0;
                        onHpPotion = false;

                        GameManager.instance.clientPlayer.character1Hp += GameManager.instance.hpPotionValue;
                        if (GameManager.instance.clientPlayer.character1Hp > GameManager.instance.character1MaxHp)
                            GameManager.instance.clientPlayer.character1Hp = GameManager.instance.character1MaxHp;
                    }
                }
                else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                {
                    if (GameManager.instance.clientPlayer.character2Hp >= GameManager.instance.character2MaxHp)
                        return;

                    if (GameManager.instance.curHpPotionCount > 0)
                    {
                        GameManager.instance.curHpPotionCount -= 1;
                        curHpCoolTime = 0;
                        onHpPotion = false;

                        GameManager.instance.clientPlayer.character2Hp += GameManager.instance.hpPotionValue;
                        if (GameManager.instance.clientPlayer.character2Hp > GameManager.instance.character2MaxHp)
                            GameManager.instance.clientPlayer.character2Hp = GameManager.instance.character2MaxHp;
                    }
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (onEpPotion)
            {
                if (GameManager.instance.clientPlayer.curMainCharacter == 1)
                {
                    if (GameManager.instance.clientPlayer.character1Ep >= GameManager.instance.character1MaxEp)
                        return;

                    if (GameManager.instance.curEpPotionCount > 0)
                    {
                        GameManager.instance.curEpPotionCount -= 1;
                        curEpCoolTime = 0;
                        onEpPotion = false;

                        GameManager.instance.clientPlayer.character1Ep += GameManager.instance.epPotionValue;
                        if (GameManager.instance.clientPlayer.character1Ep > GameManager.instance.character1MaxEp)
                            GameManager.instance.clientPlayer.character1Ep = GameManager.instance.character1MaxEp;
                    }
                }
                else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
                {
                    if (GameManager.instance.clientPlayer.character2Ep >= GameManager.instance.character2MaxEp)
                        return;

                    if (GameManager.instance.curEpPotionCount > 0)
                    {
                        GameManager.instance.curEpPotionCount -= 1;
                        curEpCoolTime = 0;
                        onEpPotion = false;

                        GameManager.instance.clientPlayer.character2Ep += GameManager.instance.epPotionValue;
                        if (GameManager.instance.clientPlayer.character2Ep > GameManager.instance.character2MaxEp)
                            GameManager.instance.clientPlayer.character2Ep = GameManager.instance.character2MaxEp;
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

    private void OnEnable()
    {
        initTargetVec = true;
    }
}