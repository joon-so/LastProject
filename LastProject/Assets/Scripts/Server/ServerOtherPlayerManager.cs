using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerOtherPlayerManager : MonoBehaviour
{
    [SerializeField] GameObject serverKarmenObj;
    [SerializeField] GameObject serverJadeObj;
    [SerializeField] GameObject serverLeinaObj;
    [SerializeField] GameObject serverEvaObj;

    [SerializeField] GameObject mainCharacterEffect;
    [SerializeField] GameObject subCharacterEffect;

    [SerializeField] GameObject character1;
    [SerializeField] GameObject character2;

    [SerializeField] Canvas playerInfoCanvas;
    [SerializeField] ServerHpBar hpBar;
    [SerializeField] ServerEpBar epBar;
    [SerializeField] Text otherPlayerID;

    public string ID;

    public int index;
    public GameObject mainObj;

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

        playerInfoCanvas.transform.position = character1.transform.position;
        otherPlayerID.text = ServerLoginManager.playerList[index].playerID;
    }
    void Update()
    {
        UpdateOtherPlayerInfo();
    }

    public void UpdateOtherPlayerInfo()
    {
        if (ServerLoginManager.playerList[index].is_Main_Character == 1)
        {
            //-------------------------------------------------------------------------------------
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

            //-------------------------------------------------------------------------------------
            mainCharacterEffect.transform.position = new Vector3(character1.transform.position.x, 0.2f, character1.transform.position.z);
            subCharacterEffect.transform.position = new Vector3(character2.transform.position.x, 0.2f, character2.transform.position.z);

            //-------------------------------------------------------------------------------------
            playerInfoCanvas.transform.position = new Vector3(character1.transform.position.x, 1.0f, character1.transform.position.z + 2.0f);

            //-------------------------------------------------------------------------------------

            //Debug.Log(index + " / C1------------------------------");
            //Debug.Log(ServerLoginManager.playerList[index].character1Hp);
            //Debug.Log(ServerLoginManager.playerList[index].character1Ep);
            hpBar.SetHp(ServerLoginManager.playerList[index].character1Hp);
            epBar.SetEp(ServerLoginManager.playerList[index].character1Ep);

            //-------------------------------------------------------------------------------------
            character1.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
            character1.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;

            character2.transform.position = ServerLoginManager.playerList[index].subCharacterPos;
            character2.transform.rotation = ServerLoginManager.playerList[index].subCharacterRot;

            //-------------------------------------------------------------------------------------
            mainObj = character1;
        }
        else if (ServerLoginManager.playerList[index].is_Main_Character == 2)
        {
            //-------------------------------------------------------------------------------------
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

            //-------------------------------------------------------------------------------------
            mainCharacterEffect.transform.position = new Vector3(character2.transform.position.x, 0.2f, character2.transform.position.z);
            subCharacterEffect.transform.position = new Vector3(character1.transform.position.x, 0.2f, character1.transform.position.z);

            //-------------------------------------------------------------------------------------
            playerInfoCanvas.transform.position = new Vector3(character2.transform.position.x, 1.0f, character2.transform.position.z + 2.0f);

            //-------------------------------------------------------------------------------------
            //Debug.Log(index + " / C2------------------------------");
            //Debug.Log(ServerLoginManager.playerList[index].character1Hp);
            //Debug.Log(ServerLoginManager.playerList[index].character1Ep);
            hpBar.SetHp(ServerLoginManager.playerList[index].character2Hp);
            epBar.SetEp(ServerLoginManager.playerList[index].character2Ep);

            //-------------------------------------------------------------------------------------
            character2.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
            character2.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;

            character1.transform.position = ServerLoginManager.playerList[index].subCharacterPos;
            character1.transform.rotation = ServerLoginManager.playerList[index].subCharacterRot;

            //-------------------------------------------------------------------------------------
            mainObj = character2;
        }
    }
}