using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInGamePlayerManager : MonoBehaviour
{
    [SerializeField] GameObject KarmenObj;
    [SerializeField] GameObject JadeObj;
    [SerializeField] GameObject LeinaObj;
    [SerializeField] GameObject EvaObj;

    public int plist;

    void Start()
    {
        plist -= 1;

        if (ServerLoginManager.playerList[plist].selectMainCharacter == 1)
        {
            KarmenObj.SetActive(true);
            KarmenObj.tag = "MainCharacter";
            KarmenObj.layer = 6;
        }
        else if (ServerLoginManager.playerList[plist].selectMainCharacter == 2)
        {
            JadeObj.SetActive(true);
            JadeObj.tag = "MainCharacter";
            JadeObj.layer = 6;
        }
        else if (ServerLoginManager.playerList[plist].selectMainCharacter == 3)
        {
            LeinaObj.SetActive(true);
            LeinaObj.tag = "MainCharacter";
            LeinaObj.layer = 6;
        }
        else if (ServerLoginManager.playerList[plist].selectMainCharacter == 4)
        {
            EvaObj.SetActive(true);
            EvaObj.tag = "MainCharacter";
            EvaObj.layer = 6;
        }

        if (ServerLoginManager.playerList[plist].selectSubCharacter == 1)
        {
            KarmenObj.SetActive(true);
            KarmenObj.tag = "SubCharacter";
            KarmenObj.layer = 7;
        }
        else if (ServerLoginManager.playerList[plist].selectSubCharacter == 2)
        {
            JadeObj.SetActive(true);
            JadeObj.tag = "SubCharacter";
            JadeObj.layer = 7;
        }
        else if (ServerLoginManager.playerList[plist].selectSubCharacter == 3)
        {
            LeinaObj.SetActive(true);
            LeinaObj.tag = "SubCharacter";
            LeinaObj.layer = 7;
        }
        else if (ServerLoginManager.playerList[plist].selectSubCharacter == 4)
        {
            EvaObj.SetActive(true);
            EvaObj.tag = "SubCharacter";
            EvaObj.layer = 7;
        }
    }
}
