using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherKarmen : MonoBehaviour
{
    [SerializeField] GameObject leftStaffEffect;
    [SerializeField] GameObject rightStaffEffect;

    [SerializeField] GameObject qSkill;
    public Transform qSkillPos;

    [SerializeField] GameObject wLeftEffect = null;
    [SerializeField] GameObject wRightEffect = null;

    private Animator otherAnimator;
    private int preBehavior;

    public int isMainCharacter;

    void Start()
    {
        otherAnimator = GetComponent<Animator>();
        preBehavior = 0;
        StartCoroutine(StartMotion());
    }

    void Update()
    {
        if (isMainCharacter == 1)
            AnimationControl();
    }

    public void AnimationControl()
    {
        // Idle
        if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 0)
        {
            otherAnimator.SetBool("Run", false);
            preBehavior = 0;
        }
        // Run
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 1)
        {
            otherAnimator.SetBool("Run", true);
            preBehavior = 1;
        }
        // Dodge
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 2)
        {
            if (preBehavior != 2)
                StartCoroutine(DodgeDelay());
        }
        // Attack
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 3)
        {
            if (preBehavior != 3)
                StartCoroutine(AttackDelay());
        }
        // QSkill
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 4)
        {
            if (preBehavior != 4)
            {
                StartCoroutine(BigAttack());
                preBehavior = 4;
            }
        }
        // WSkill
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 5)
        {
            if (preBehavior != 5)
            {
                StartCoroutine(StraightAttack());
                preBehavior = 5;
            }
        }
    }

    IEnumerator AttackDelay()
    {
        preBehavior = 3;
        otherAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(0.6f);
        preBehavior = 0;
    }
    IEnumerator DodgeDelay()
    {
        preBehavior = 2;
        otherAnimator.SetTrigger("Dodge");
        yield return new WaitForSeconds(1.0f);
        preBehavior = 0;
    }
    IEnumerator StartMotion()
    {
        yield return new WaitForSeconds(0.5f);
        otherAnimator.SetTrigger("StartMotion");
        yield return new WaitForSeconds(1.5f);
        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);
    }
    IEnumerator BigAttack()
    {
        leftStaffEffect.SetActive(false);
        rightStaffEffect.SetActive(false);

        otherAnimator.SetTrigger("QSkill");
        otherAnimator.SetFloat("Speed", 0.2f);
        yield return new WaitForSeconds(0.5f);
        Instantiate(qSkill, qSkillPos.position, qSkillPos.rotation);
        otherAnimator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(1.0f);
        otherAnimator.SetFloat("Speed", 1.0f);
        yield return new WaitForSeconds(1.0f);

        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        otherAnimator.SetBool("Run", false);
    }
    IEnumerator StraightAttack()
    {
        leftStaffEffect.SetActive(false);
        rightStaffEffect.SetActive(false);

        otherAnimator.SetTrigger("WSkill");
        wLeftEffect.SetActive(true);
        wRightEffect.SetActive(true);
        yield return new WaitForSeconds(2.8f);

        wLeftEffect.SetActive(false);
        wRightEffect.SetActive(false);

        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        otherAnimator.SetBool("Run", false);
    }
}
