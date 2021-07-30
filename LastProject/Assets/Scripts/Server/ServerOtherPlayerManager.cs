using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherPlayerManager : MonoBehaviour
{
    [SerializeField] GameObject serverKarmenObj;
    [SerializeField] GameObject serverJadeObj;
    [SerializeField] GameObject serverLeinaObj;
    [SerializeField] GameObject serverEvaObj;

    [SerializeField] GameObject character1;
    [SerializeField] GameObject character2;

    public string ID;

    public int index;

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
            if (ServerLoginManager.playerList[index].selectMainCharacter == 1)
                character1.GetComponent<ServerOtherKarmen>().isMainCharacter = 1;
            else if (ServerLoginManager.playerList[index].selectMainCharacter == 2)
                character1.GetComponent<ServerOtherJade>().isMainCharacter = 1;
            else if (ServerLoginManager.playerList[index].selectMainCharacter == 3)
                character1.GetComponent<ServerOtherLeina>().isMainCharacter = 1;
            else if (ServerLoginManager.playerList[index].selectMainCharacter == 4)
                character1.GetComponent<ServerOtherEva>().isMainCharacter = 1;

            if (ServerLoginManager.playerList[index].selectSubCharacter == 1)
                character2.GetComponent<ServerOtherKarmen>().isMainCharacter = 2;
            else if (ServerLoginManager.playerList[index].selectSubCharacter == 2)
                character2.GetComponent<ServerOtherJade>().isMainCharacter = 2;
            else if (ServerLoginManager.playerList[index].selectSubCharacter == 3)
                character2.GetComponent<ServerOtherLeina>().isMainCharacter = 2;
            else if (ServerLoginManager.playerList[index].selectSubCharacter == 4)
                character2.GetComponent<ServerOtherEva>().isMainCharacter = 2;

            character1.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
            character1.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;

            character2.transform.position = ServerLoginManager.playerList[index].subCharacterPos;
            character2.transform.rotation = ServerLoginManager.playerList[index].subCharacterRot;
        }
        else if (ServerLoginManager.playerList[index].is_Main_Character == 2)
        {
            if (ServerLoginManager.playerList[index].selectMainCharacter == 1)
                character1.GetComponent<ServerOtherKarmen>().isMainCharacter = 2;
            else if (ServerLoginManager.playerList[index].selectMainCharacter == 2)
                character1.GetComponent<ServerOtherJade>().isMainCharacter = 2;
            else if (ServerLoginManager.playerList[index].selectMainCharacter == 3)
                character1.GetComponent<ServerOtherLeina>().isMainCharacter = 2;
            else if (ServerLoginManager.playerList[index].selectMainCharacter == 4)
                character1.GetComponent<ServerOtherEva>().isMainCharacter = 2;

            if (ServerLoginManager.playerList[index].selectSubCharacter == 1)
                character2.GetComponent<ServerOtherKarmen>().isMainCharacter = 1;
            else if (ServerLoginManager.playerList[index].selectSubCharacter == 2)
                character2.GetComponent<ServerOtherJade>().isMainCharacter = 1;
            else if (ServerLoginManager.playerList[index].selectSubCharacter == 3)
                character2.GetComponent<ServerOtherLeina>().isMainCharacter = 1;
            else if (ServerLoginManager.playerList[index].selectSubCharacter == 4)
                character2.GetComponent<ServerOtherEva>().isMainCharacter = 1;

            character2.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
            character2.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;

            character1.transform.position = ServerLoginManager.playerList[index].subCharacterPos;
            character1.transform.rotation = ServerLoginManager.playerList[index].subCharacterRot;
        }
    }
}
