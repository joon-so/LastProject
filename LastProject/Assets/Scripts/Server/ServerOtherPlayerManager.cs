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
            character1 = serverKarmenObj;
            serverKarmenObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 2)
        {
            character1 = serverJadeObj;
            serverJadeObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 3)
        {
            character1 = serverLeinaObj;
            serverLeinaObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 4)
        {
            character1 = serverEvaObj;
            serverEvaObj.SetActive(true);
        }

        if (ServerLoginManager.playerList[index].selectSubCharacter == 1)
        {
            character2 = serverKarmenObj;
            serverKarmenObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 2)
        {
            character2 = serverJadeObj;
            serverJadeObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 3)
        {
            character2 = serverLeinaObj;
            serverLeinaObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 4)
        {
            character2 = serverEvaObj;
            serverEvaObj.SetActive(true);
        }
    }
    void Update()
    {
        UpdatePos();
    }

    public void UpdatePos()
    {
        if (ServerLoginManager.playerList[index].is_Main_Character == 1)
        {
            character1.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
            character1.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;

            character2.transform.position = ServerLoginManager.playerList[index].subCharacterPos;
            character2.transform.rotation = ServerLoginManager.playerList[index].subCharacterRot;
        }
        else if (ServerLoginManager.playerList[index].is_Main_Character == 2)
        {
            character2.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
            character2.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;

            character1.transform.position = ServerLoginManager.playerList[index].subCharacterPos;
            character1.transform.rotation = ServerLoginManager.playerList[index].subCharacterRot;
        }
    }
}
