using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttack : MonoBehaviour
{
    public Animator anim;
    bool canCombo;
    int comboStep;

    public void Attack()
    {
        if (comboStep ==0)
        {
            anim.Play("attack2SS");
            comboStep = 1;
            return;
        }
        if (comboStep!= 0)
        {
            canCombo = false;
            comboStep += 1;
        }
        
    }

    public void CanCombo()
    {
        canCombo = true;
    }

    public void Combo()
    {
        if(comboStep == 2)
            anim.Play("attack3SS");
        if (comboStep == 3)
            anim.Play("attack4SS");
        if (comboStep == 4)
            anim.Play("attack5SS");
    }

    public void ComboReset()
    {
        canCombo = false;
        comboStep = 0;
    }

}
