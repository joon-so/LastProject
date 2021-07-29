using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherLeina : MonoBehaviour
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPos;

    [SerializeField] GameObject posionArrow;
    [SerializeField] Transform posionArrowPos;

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
        if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 0)
        {
            otherAnimator.SetBool("Run", false);
        }
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 1)
        {
            otherAnimator.SetBool("Run", true);
            preBehavior = 1;
        }
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 2)
        {
            if (preBehavior != 2)
                StartCoroutine(DodgeDelay());
        }
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 3)
        {
            if (preBehavior != 3)
                StartCoroutine(ShootArrow());
        }
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 4)
        {
            if (preBehavior != 4)
            {
                StartCoroutine(ChargingShot());
                preBehavior = 4;
            }
        }
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 5)
        {
            if (preBehavior != 5)
            {
                StartCoroutine(WideShot());
                preBehavior = 5;
            }
        }
    }

    IEnumerator DodgeDelay()
    {
        preBehavior = 2;
        otherAnimator.SetTrigger("Dodge");
        yield return new WaitForSeconds(1.0f);
        preBehavior = 0;
    }
    IEnumerator ShootArrow()
    {
        preBehavior = 3; 
        otherAnimator.SetTrigger("Attack");
        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward;
        yield return new WaitForSeconds(0.5f);
        preBehavior = 0;
    }
    IEnumerator ChargingShot()
    {
        otherAnimator.SetTrigger("QSkill");
        // Â÷Â¡
        yield return new WaitForSeconds(1.4f);
        GameObject instantArrow = Instantiate(posionArrow, posionArrowPos.position, posionArrowPos.rotation);
        LeinaPosionArrow.speed = 0;
        // ¼¦
        yield return new WaitForSeconds(1f);

        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = posionArrowPos.forward;
        LeinaPosionArrow.speed = 40;
        yield return new WaitForSeconds(1.0f);
    }

    IEnumerator WideShot()
    {
        //SoundManager.instance.SFXPlay("Attack", attackClip);

        otherAnimator.SetBool("Run", false);

        otherAnimator.SetTrigger("Attack");
        // ¼¦
        Vector3 pos = arrowPos.position;
        GameObject instantArrow = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -25f, 0));
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward;

        GameObject instantArrow2 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -15f, 0));
        Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
        arrowRigid2.velocity = arrowPos.forward;

        GameObject instantArrow3 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -5f, 0));
        Rigidbody arrowRigid3 = instantArrow3.GetComponent<Rigidbody>();
        arrowRigid3.velocity = arrowPos.forward;

        GameObject instantArrow4 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 5f, 0));
        Rigidbody arrowRigid4 = instantArrow4.GetComponent<Rigidbody>();
        arrowRigid4.velocity = arrowPos.forward;

        GameObject instantArrow5 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 15f, 0));
        Rigidbody arrowRigid5 = instantArrow5.GetComponent<Rigidbody>();
        arrowRigid5.velocity = arrowPos.forward;

        GameObject instantArrow6 = Instantiate(arrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 25f, 0));
        Rigidbody arrowRigid6 = instantArrow6.GetComponent<Rigidbody>();
        arrowRigid6.velocity = arrowPos.forward;

        yield return new WaitForSeconds(0.5f);
    }
}
