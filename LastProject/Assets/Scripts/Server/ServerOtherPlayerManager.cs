using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherPlayerManager : MonoBehaviour
{
    public static ServerOtherPlayerManager instance;

    [SerializeField] GameObject KarmenObj;
    [SerializeField] GameObject JadeObj;
    [SerializeField] GameObject LeinaObj;
    [SerializeField] GameObject EvaObj;

    [SerializeField] GameObject character1;
    [SerializeField] GameObject character2;

    public string ID = null;

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
            character1 = KarmenObj;
            //KarmenObj.tag = "MainCharacter";
            KarmenObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 2)
        {
            character1 = JadeObj;
            //JadeObj.tag = "MainCharacter";
            JadeObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 3)
        {
            character1 = LeinaObj;
            //LeinaObj.tag = "MainCharacter";
            LeinaObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectMainCharacter == 4)
        {
            character1 = EvaObj;
            //EvaObj.tag = "MainCharacter";
            EvaObj.SetActive(true);
        }

        if (ServerLoginManager.playerList[index].selectSubCharacter == 1)
        {
            character2 = KarmenObj;
            //KarmenObj.tag = "SubCharacter";
            KarmenObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 2)
        {
            character2 = JadeObj;
            //JadeObj.tag = "SubCharacter";
            JadeObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 3)
        {
            character2 = LeinaObj;
            //LeinaObj.tag = "SubCharacter";
            LeinaObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[index].selectSubCharacter == 4)
        {
            character2 = EvaObj;
            //EvaObj.tag = "SubCharacter";
            EvaObj.SetActive(true);
        }
    }
    void Update()
    {
        UpdatePos();
    }

    public void UpdatePos()
    {
        //Debug.Log("----------------" + 0);
        //Debug.Log(ServerLoginManager.playerList[0].playerID);
        //Debug.Log(ServerLoginManager.playerList[0].mainCharacterPos);

        //Debug.Log("----------------" + 1);
        //Debug.Log(ServerLoginManager.playerList[1].playerID);
        //Debug.Log(ServerLoginManager.playerList[1].mainCharacterPos);

        //Debug.Log("----------------" + 2);
        //Debug.Log(ServerLoginManager.playerList[2].playerID);
        //Debug.Log(ServerLoginManager.playerList[2].mainCharacterPos);

        //Debug.Log("----------------" + 3);
        //Debug.Log(ServerLoginManager.playerList[3].playerID);
        //Debug.Log(ServerLoginManager.playerList[3].mainCharacterPos);
        //Debug.Log(ServerLoginManager.playerList[index].mainCharacterRot);

        character1.transform.position = ServerLoginManager.playerList[index].mainCharacterPos;
        character1.transform.rotation = ServerLoginManager.playerList[index].mainCharacterRot;
    }
}
