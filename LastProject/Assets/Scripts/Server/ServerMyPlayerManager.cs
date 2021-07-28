using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerMyPlayerManager : MonoBehaviour
{
    [SerializeField] GameObject KarmenObj;
    [SerializeField] GameObject JadeObj;
    [SerializeField] GameObject LeinaObj;
    [SerializeField] GameObject EvaObj;

    public GameObject character1;
    public GameObject character2;

    public string ID;
    private bool isTag;

    void Start()
    {
        if (ServerLoginManager.playerList[0].selectMainCharacter == 1)
        {
            character1 = KarmenObj;
            KarmenObj.tag = "MainCharacter";
            KarmenObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
        {
            character1 = JadeObj;
            JadeObj.tag = "MainCharacter";
            JadeObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
        {
            character1 = LeinaObj;
            LeinaObj.tag = "MainCharacter";
            LeinaObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
        {
            character1 = EvaObj;
            EvaObj.tag = "MainCharacter";
            EvaObj.SetActive(true);
        }

        if (ServerLoginManager.playerList[0].selectSubCharacter == 1)
        {
            character2 = KarmenObj;
            KarmenObj.tag = "SubCharacter";
            KarmenObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 2)
        {
            character2 = JadeObj;
            JadeObj.tag = "SubCharacter";
            JadeObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 3)
        {
            character2 = LeinaObj;
            LeinaObj.tag = "SubCharacter";
            LeinaObj.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 4)
        {
            character2 = EvaObj;
            EvaObj.tag = "SubCharacter";
            EvaObj.SetActive(true);
        }
        ID = ServerLoginManager.playerList[0].playerID;
        StartCoroutine("CoSendPacket");

        isTag = true;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            ServerMainSubTag();
        }
    }

    public void ServerMainSubTag()
    {
        // main->sub
        //if (isTag)
        //{
        //    character1.gameObject.tag = "SubCharacter";
        //    character2.gameObject.tag = "MainCharacter";
        //    isTag = false;
        //}
        //else
        //{
        //    character1.gameObject.tag = "MainCharacter";
        //    character2.gameObject.tag = "SubCharacter";
        //    isTag = true;
        //}
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
                movePacket.mainPlayer_Behavior = ServerLoginManager.playerList[0].mainCharacterBehavior;
                movePacket.mainPlayer_Pos_X = character1.gameObject.transform.position.x;
                movePacket.mainPlayer_Pos_Z = character1.gameObject.transform.position.z;
                movePacket.mainPlayer_Rot_Y = character1.gameObject.transform.rotation.eulerAngles.y;

                movePacket.mainPlayer_Hp = 0;
                movePacket.mainPlayer_Mp = 0;

                movePacket.subPlayer_Behavior = ServerLoginManager.playerList[0].subCharacterBehavior;
                movePacket.subPlayer_Pos_X = 0;
                movePacket.subPlayer_Pos_Z = 0;
                movePacket.subPlayer_Rot_Y = 0;
                movePacket.subPlayer_Hp = 0;
                movePacket.subPlayer_Mp = 0;

                //Debug.Log(character1.gameObject.transform.position);

                //movePacket.subPlayer_Pos_X = character1.gameObject.transform.position.x;
                //movePacket.subPlayer_Pos_Z = character1.gameObject.transform.position.z;
                //movePacket.subPlayer_Rot_Y = character1.gameObject.transform.rotation.y;
            }
            else if (character2.gameObject.tag == "MainCharacter")
            {
                movePacket.mainPlayer_Behavior = ServerLoginManager.playerList[0].mainCharacterBehavior;
                movePacket.mainPlayer_Pos_X = character2.gameObject.transform.position.x;
                movePacket.mainPlayer_Pos_Z = character2.gameObject.transform.position.z;
                movePacket.mainPlayer_Rot_Y = character1.gameObject.transform.rotation.eulerAngles.y;

                movePacket.mainPlayer_Hp = 0;
                movePacket.mainPlayer_Mp = 0;

                movePacket.subPlayer_Behavior = ServerLoginManager.playerList[0].subCharacterBehavior;
                movePacket.subPlayer_Pos_X = 0;
                movePacket.subPlayer_Pos_Z = 0;
                movePacket.subPlayer_Rot_Y = 0;
                movePacket.subPlayer_Hp = 0;
                movePacket.subPlayer_Mp = 0;
                //movePacket.subPlayer_Pos_X = character2.gameObject.transform.position.x;
                //movePacket.subPlayer_Pos_Z = character2.gameObject.transform.position.z;
                //movePacket.subPlayer_Rot_Y = character2.gameObject.transform.rotation.y;
            }

            //Debug.Log("Send Packet"+ movePacket.ID+ " " + movePacket.mainPlayer_Pos_X + " " + movePacket.mainPlayer_Pos_Z);

            NetworkManager.instance.Send(movePacket.Write());
        }
    }
}
