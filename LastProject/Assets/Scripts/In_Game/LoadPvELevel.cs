using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPvELevel : MonoBehaviour
{
    [SerializeField] Image stage1;
    [SerializeField] Image stage2;
    [SerializeField] Image stage3;
    [SerializeField] Image stage4;
    [SerializeField] Image stage5;

    Color nonSelectColor = new Color(1f, 1f, 1f, 0.2f);
    void Start()
    {
        int i = DataBaseManager.PlayerPvELevel;
        if (i == 0)
        {
            stage2.color = nonSelectColor;
            stage3.color = nonSelectColor;
            stage4.color = nonSelectColor;
            stage5.color = nonSelectColor;
        }
        else if (i == 1)
        {
            stage3.color = nonSelectColor;
            stage4.color = nonSelectColor;
            stage5.color = nonSelectColor;
        }
        else if (i == 2)
        {
            stage4.color = nonSelectColor;
            stage5.color = nonSelectColor;
        }
        else if (i == 3)
        {
            stage5.color = nonSelectColor;
        }
    }

    void Update()
    {
        
    }
}
