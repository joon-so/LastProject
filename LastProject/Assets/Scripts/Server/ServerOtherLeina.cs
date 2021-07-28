using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherLeina : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPos;

    private Animator otherAnimator;

    private int preBehavior;

    void Start()
    {
        otherAnimator = GetComponent<Animator>();
        preBehavior = 0;
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
                StartCoroutine(ShootArrow());
                preBehavior = 3;
            }
        }
    }

    IEnumerator ShootArrow()
    {
        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward;
        yield return null;
    }
}
