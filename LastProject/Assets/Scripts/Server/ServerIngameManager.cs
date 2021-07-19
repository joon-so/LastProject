using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerInGameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> player;

    void Start()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (ServerLoginManager.playerList[i].isContainPlayerInfo)
            {
                player[i].SetActive(true);
            }
        }
    }

    void Update()
    {
        
    }
}
