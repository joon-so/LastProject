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
        if (ServerLoginManager.playerList[plist].selectMainCharacter == 1)
        {
            KarmenObj.SetActive(true);
            KarmenObj.tag = "MainCharacter";
        }
        else if (ServerLoginManager.playerList[plist].selectMainCharacter == 2)
        {
            JadeObj.SetActive(true);
            KarmenObj.tag = "MainCharacter";
        }
        else if (ServerLoginManager.playerList[plist].selectMainCharacter == 3)
        {
            LeinaObj.SetActive(true);
            KarmenObj.tag = "MainCharacter";
        }
        else if (ServerLoginManager.playerList[plist].selectMainCharacter == 4)
        {
            EvaObj.SetActive(true);
            KarmenObj.tag = "MainCharacter";
        }

        if (ServerLoginManager.playerList[plist].selectSubCharacter == 1)
        {
            KarmenObj.SetActive(true);
            KarmenObj.tag = "SubCharacter";
        }
        else if (ServerLoginManager.playerList[plist].selectSubCharacter == 2)
        {
            JadeObj.SetActive(true);
            KarmenObj.tag = "SubCharacter";
        }
        else if (ServerLoginManager.playerList[plist].selectSubCharacter == 3)
        {
            LeinaObj.SetActive(true);
            KarmenObj.tag = "SubCharacter";
        }
        else if (ServerLoginManager.playerList[plist].selectSubCharacter == 4)
        {
            EvaObj.SetActive(true);
            KarmenObj.tag = "SubCharacter";
        }
    }

    void Update()
    {
        
    }
}
