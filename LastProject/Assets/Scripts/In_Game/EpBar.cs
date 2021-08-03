using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EpBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxEp(int Ep)
    {
        slider.maxValue = Ep;
        slider.value = Ep;
    }

    public void SetEp(int Ep)
    {
        slider.value = Ep;
    }
}