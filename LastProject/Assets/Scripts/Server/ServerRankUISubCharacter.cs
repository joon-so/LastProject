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
        if (ServerIngameManager.instance.resultPlayerInfo[playerIndex].character2 == 1)
        {
            subKarmenMask.SetActive(true);
        }
        else if (ServerIngameManager.instance.resultPlayerInfo[playerIndex].character2 == 2)
        {
            subJadeMask.SetActive(true);
        }
        else if (ServerIngameManager.instance.resultPlayerInfo[playerIndex].character2 == 3)
        {
            subLeinaMask.SetActive(true);
        }
        else if (ServerIngameManager.instance.resultPlayerInfo[playerIndex].character2 == 4)
        {
            subEvaMask.SetActive(true);
        }
    }
}