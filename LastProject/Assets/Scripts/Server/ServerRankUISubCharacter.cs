using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerRankUISubCharacter : ServerRankPlayerInfo
{
    [SerializeField] GameObject subKarmenMask;
    [SerializeField] GameObject subJadeMask;
    [SerializeField] GameObject subLeinaMask;
    [SerializeField] GameObject subEvaMask;

    void Start()
    {
        if (ServerLoginManager.playerList[playerIndex].selectSubCharacter == 1)
        {
            subKarmenMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[playerIndex].selectSubCharacter == 2)
        {
            subJadeMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[playerIndex].selectSubCharacter == 3)
        {
            subLeinaMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[playerIndex].selectSubCharacter == 4)
        {
            subEvaMask.SetActive(true);
        }
    }
}
