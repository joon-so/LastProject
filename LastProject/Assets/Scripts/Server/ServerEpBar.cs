using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerEpBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxEp(short Ep)
    {
        slider.maxValue = Ep;
        slider.value = Ep;
    }

    public void SetEp(short Ep)
    {
        slider.value = Ep;
    }
}