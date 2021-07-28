using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerLobbyCharacterMainMask : MonoBehaviour
{
    [SerializeField] GameObject mainKarmenMask;
    [SerializeField] GameObject mainJadeMask;
    [SerializeField] GameObject mainLeinaMask;
    [SerializeField] GameObject mainEvaMask;

    public int listPos;

    void Update()
    {
        if (ServerLoginManager.playerList[listPos].selectMainCharacter == 1)
        {
            mainKarmenMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[listPos].selectMainCharacter == 2)
        {
            mainJadeMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[listPos].selectMainCharacter == 3)
        {
            mainLeinaMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[listPos].selectMainCharacter == 4)
        {
            mainEvaMask.SetActive(true);
        }
    }
}
