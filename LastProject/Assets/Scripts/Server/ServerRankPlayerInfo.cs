using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerRankPlayerInfo : MonoBehaviour
{
    public int playerIndex;
    public Text nameText;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnEnable()
    {
        nameText.text = ServerIngameManager.instance.resultPlayerInfo[playerIndex].playerID;
    }
}