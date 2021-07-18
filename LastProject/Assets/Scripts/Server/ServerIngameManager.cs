using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerIngameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> player;

    private int plist;

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
