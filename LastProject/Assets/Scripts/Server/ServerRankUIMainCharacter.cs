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
        if (ServerIngameManager.instance.resultPlayerInfo[playerIndex].character1 == 1)
        {
            mainKarmenMask.SetActive(true);
        }
        else if (ServerIngameManager.instance.resultPlayerInfo[playerIndex].character1 == 2)
        {
            mainJadeMask.SetActive(true);
        }
        else if (ServerIngameManager.instance.resultPlayerInfo[playerIndex].character1 == 3)
        {
            mainLeinaMask.SetActive(true);
        }
        else if (ServerIngameManager.instance.resultPlayerInfo[playerIndex].character1 == 4)
        {
            mainEvaMask.SetActive(true);
        }
    }
}