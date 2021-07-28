using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerIngameManager : MonoBehaviour
{
    public List<GameObject> player;

    // KDA
    void Start()
    {
        for (int i = 0; i < 4; ++i)
        {
            if (ServerLoginManager.playerList[i].playerID != null)
            {
                player[i].SetActive(true);
            }
            else
            {
                Destroy(player[i]);
            }
        }
    }
}
