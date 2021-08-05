using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherPlayerManager : MonoBehaviour
{
    public static ServerOtherPlayerManager instance;

    [SerializeField] GameObject serverKarmenObj;
    [SerializeField] GameObject serverJadeObj;
    [SerializeField] GameObject serverLeinaObj;
    [SerializeField] GameObject serverEvaObj;

    [SerializeField] GameObject character1;
    [SerializeField] GameObject character2;

    public string ID;

    public int index;
    private int kindOfCharacter1;
    private int kindOfCharacter2;

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void Start()
    {
        ID = ServerLoginManager.playerList[index].playerID;

        if (ServerLoginManager.playerList[index].selectMainCharacter == 1)
        {
            kindOfCharacter1 = 1;
            character1 = serverKarmenObj;
            //character1.GetComponent<ServerOtherKarmen>().isMainCharacter = 1;
            serverKarmenObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 2)
        {
            kindOfCharacter1 = 2;
            character1 = serverJadeObj;
            //character1.GetComponent<ServerOtherJade>().isMainCharacter = 1;
            serverJadeObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 3)
        {
            kindOfCharacter1 = 3;
            character1 = serverLeinaObj;
            //character1.GetComponent<ServerOtherLeina>().isMainCharacter = 1;
            serverLeinaObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 4)
        {
            kindOfCharacter1 = 4;
            character1 = serverEvaObj;
            //character1.GetComponent<ServerOtherEva>().isMainCharacter = 1;
            serverEvaObj.SetActive(true);
        }

        if (ServerLoginManager.playerList[index].selectSubCharacter == 1)
        {
            kindOfCharacter2 = 1; 
            character2 = serverKarmenObj;
            //character2.GetComponent<ServerOtherKarmen>().isMainCharacter = 2;
            serverKarmenObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 2)
        {
            kindOfCharacter2 = 2;
            character2 = serverJadeObj;
            //character2.GetComponent<ServerOtherJade>().isMainCharacter = 2;
            serverJadeObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 3)
        {
            kindOfCharacter2 = 3;
            character2 = serverLeinaObj;
            //character2.GetComponent<ServerOtherLeina>().isMainCharacter = 2;
            serverLeinaObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 4)
        {
            kindOfCharacter2 = 4;
            character2 = serverEvaObj;
            //character2.GetComponent<ServerOtherEva>().isMainCharacter = 2;
            serverEvaObj.SetActive(true);
        }
    }
    void Update()
    {
        UpdatePos();
        Debug.Log("------ServerOtherClient------");
        Debug.Log("INDEX" + index);
        Debug.Log("BeHav" + ServerLoginManager.playerList[index].mainCharacterBehavior);
        Debug.Log("main" + ServerLoginManager.playerList[index].is_Main_Character);

    }

    public void UpdatePos()
    {
        if (ServerLoginManager.playerList[index].is_Main_Character == 1)
        {
            if (kindOfCharacter1 == 1)
                character1.GetComponent<ServerOtherKarmen>().isMainCharacter = 1;
            else if (kindOfCharacter1 == 2)
                character1.GetComponent<ServerOtherJade>().isMainCharacter = 1;
            else if (kindOfCharacter1 == 3)
                character1.GetComponent<ServerOtherLeina>().isMainCharacter = 1;
            else if (kindOfCharacter1 == 4)
                character1.GetComponent<ServerOtherEva>().isMainCharacter = 1;

            if (kindOfCharacter2 == 1)
                character2.GetComponent<ServerOtherKarmen>().isMainCharacter = 2;
            else if (kindOfCharacter2 == 2)
                character2.GetComponent<ServerOtherJade>().isMainCharacter = 2;
            else if (kindOfCharacter2 == 3)
                character2.GetComponent<ServerOtherLeina>().isMainCharacter = 2;
            else if (kindOfCharacter2 == 4)
                character2.GetComponent<ServerOtherEva>().isMainCharacter = 2;

            character1.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
            character1.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;

            character2.transform.position = ServerLoginManager.playerList[index].subCharacterPos;
            character2.transform.rotation = ServerLoginManager.playerList[index].subCharacterRot;
        }
        else if (ServerLoginManager.playerList[index].is_Main_Character == 2)
        {
            if (kindOfCharacter1 == 1)
                character1.GetComponent<ServerOtherKarmen>().isMainCharacter = 2;
            else if (kindOfCharacter1 == 2)
                character1.GetComponent<ServerOtherJade>().isMainCharacter = 2;
            else if (kindOfCharacter1 == 3)
                character1.GetComponent<ServerOtherLeina>().isMainCharacter = 2;
            else if (kindOfCharacter1 == 4)
                character1.GetComponent<ServerOtherEva>().isMainCharacter = 2;

            if (kindOfCharacter2 == 1)
                character2.GetComponent<ServerOtherKarmen>().isMainCharacter = 1;
            else if (kindOfCharacter2 == 2)
                character2.GetComponent<ServerOtherJade>().isMainCharacter = 1;
            else if (kindOfCharacter2 == 3)
                character2.GetComponent<ServerOtherLeina>().isMainCharacter = 1;
            else if (kindOfCharacter2 == 4)
                character2.GetComponent<ServerOtherEva>().isMainCharacter = 1;

            character2.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
            character2.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;

            character1.transform.position = ServerLoginManager.playerList[index].subCharacterPos;
            character1.transform.rotation = ServerLoginManager.playerList[index].subCharacterRot;
        }
    }
}
