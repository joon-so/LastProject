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
        if (listPos == 1)
        {
            if (ServerLoginManager.playerList[0].selectMainCharacter == 1)
            {
                mainKarmenMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 2)
            {
                mainJadeMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 3)
            {
                mainLeinaMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[0].selectMainCharacter == 4)
            {
                mainEvaMask.SetActive(true);
            }
        }

        else if(listPos == 2)
        {
            if (ServerLoginManager.playerList[1].selectMainCharacter == 1)
            {
                mainKarmenMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[1].selectMainCharacter == 2)
            {
                mainJadeMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[1].selectMainCharacter == 3)
            {
                mainLeinaMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[1].selectMainCharacter == 4)
            {
                mainEvaMask.SetActive(true);
            }
        }

        else if(listPos == 3)
        {
            if (ServerLoginManager.playerList[2].selectMainCharacter == 1)
            {
                mainKarmenMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[2].selectMainCharacter == 2)
            {
                mainJadeMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[2].selectMainCharacter == 3)
            {
                mainLeinaMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[2].selectMainCharacter == 4)
            {
                mainEvaMask.SetActive(true);
            }
        }

        else if(listPos == 4)
        {
            if (ServerLoginManager.playerList[3].selectMainCharacter == 1)
            {
                mainKarmenMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[3].selectMainCharacter == 2)
            {
                mainJadeMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[3].selectMainCharacter == 3)
            {
                mainLeinaMask.SetActive(true);
            }
            else if (ServerLoginManager.playerList[3].selectMainCharacter == 4)
            {
                mainEvaMask.SetActive(true);
            }
        }
    }
}
