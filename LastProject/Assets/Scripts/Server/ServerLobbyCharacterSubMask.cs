using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerLobbyCharacterSubMask : MonoBehaviour
{
    [SerializeField] GameObject subKarmenMask;
    [SerializeField] GameObject subJadeMask;
    [SerializeField] GameObject subLeinaMask;
    [SerializeField] GameObject subEvaMask;

    public int listPos;

    void Update()
    {
        if (ServerLoginManager.playerList[0].selectSubCharacter == 1)
        {
            subKarmenMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 2)
        {
            subJadeMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 3)
        {
            subLeinaMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[0].selectSubCharacter == 4)
        {
            subEvaMask.SetActive(true);
        }

        if (ServerLoginManager.playerList[1].selectSubCharacter == 1)
        {
            subKarmenMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[1].selectSubCharacter == 2)
        {
            subJadeMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[1].selectSubCharacter == 3)
        {
            subLeinaMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[1].selectSubCharacter == 4)
        {
            subEvaMask.SetActive(true);
        }

        if (ServerLoginManager.playerList[2].selectSubCharacter == 1)
        {
            subKarmenMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[2].selectSubCharacter == 2)
        {
            subJadeMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[2].selectSubCharacter == 3)
        {
            subLeinaMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[2].selectSubCharacter == 4)
        {
            subEvaMask.SetActive(true);
        }

        if (ServerLoginManager.playerList[3].selectSubCharacter == 1)
        {
            subKarmenMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[3].selectSubCharacter == 2)
        {
            subJadeMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[3].selectSubCharacter == 3)
        {
            subLeinaMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[3].selectSubCharacter == 4)
        {
            subEvaMask.SetActive(true);
        }
    }
}
