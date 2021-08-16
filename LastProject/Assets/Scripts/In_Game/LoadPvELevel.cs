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

    void Start()
    {
        Color nonSelectColor = new Color(1f, 1f, 1f, 0.2f);
        stage1.color = nonSelectColor;
        stage2.color = nonSelectColor;
        stage3.color = nonSelectColor;
        stage4.color = nonSelectColor;
        stage5.color = nonSelectColor;

    }

    void Update()
    {
        
    }
}
