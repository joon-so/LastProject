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

    void Start()
    {
        listPos -= 1;
    }

    void Update()
    {
        if (ServerLoginManager.playerList[listPos].selectSubCharacter == 1)
        {
            subKarmenMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[listPos].selectSubCharacter == 2)
        {
            subJadeMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[listPos].selectSubCharacter == 3)
        {
            subLeinaMask.SetActive(true);
        }
        else if (ServerLoginManager.playerList[listPos].selectSubCharacter == 4)
        {
            subEvaMask.SetActive(true);
        }
    }
}
