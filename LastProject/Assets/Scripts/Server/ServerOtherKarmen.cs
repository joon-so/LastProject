using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherKarmen : MonoBehaviour
{
    [SerializeField] GameObject leftStaffEffect;
    [SerializeField] GameObject rightStaffEffect;

    private Animator otherAnimator;
    private int preBehavior;

    void Start()
    {
        otherAnimator = GetComponent<Animator>();
        preBehavior = 0;
        StartCoroutine(StartMotion());
    }

    void Update()
    {
        AnimationControl();
    }

    public void AnimationControl()
    {
        // Idle
        if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 0)
        {
            otherAnimator.SetBool("Run", false);
        }
        // run
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 1)
        {
            if (preBehavior != 1)
            {
                otherAnimator.SetBool("Run", true);
                preBehavior = 1;
            }
        }
        // dodge
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 2)
        {
            if (preBehavior != 2)
            {
                otherAnimator.SetTrigger("Dodge");
                preBehavior = 2;
            }
        }
        // attack
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 3)
        {
            if (preBehavior != 3)
            {
                otherAnimator.SetTrigger("Attack");
                preBehavior = 3;
            }
        }
    }
    IEnumerator StartMotion()
    {
        yield return new WaitForSeconds(0.5f);
        otherAnimator.SetTrigger("StartMotion");
        yield return new WaitForSeconds(1.5f);
        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);
    }
}
