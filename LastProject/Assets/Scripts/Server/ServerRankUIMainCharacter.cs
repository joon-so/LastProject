using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerRankUIMainCharacter : ServerRankPlayerInfo
{
    [SerializeField] GameObject mainKarmenMask;
    [SerializeField] GameObject mainJadeMask;
    [SerializeField] GameObject mainLeinaMask;
    [SerializeField] GameObject mainEvaMask;

    void Start()
    {
        if (ServerLoginManager.playerList[playerIndex].selectMainCharacter == 1)
        {
            mainKarmenMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[playerIndex].selectMainCharacter == 2)
        {
            mainJadeMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[playerIndex].selectMainCharacter == 3)
        {
            mainLeinaMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[playerIndex].selectMainCharacter == 4)
        {
            mainEvaMask.SetActive(true);
        }
    }
}
