using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerHpBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHp(short hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
    }

    public void SetHp(short hp)
    {
        slider.value = hp;
    }
}