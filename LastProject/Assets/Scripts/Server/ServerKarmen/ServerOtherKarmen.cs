using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherKarmen : MonoBehaviour
{
    [SerializeField] GameObject leftStaffEffect;
    [SerializeField] GameObject rightStaffEffect;

    [SerializeField] CapsuleCollider leftStaff;
    [SerializeField] CapsuleCollider rightStaff;

    [SerializeField] GameObject qSkill;
    public Transform qSkillPos;

    [SerializeField] GameObject wLeftEffect = null;
    [SerializeField] GameObject wRightEffect = null;

    [SerializeField] GameObject parentObject;

    public int isMainCharacter;

    private Animator otherAnimator;
    
    private int preBehavior;
    private int index;

    void Start()
    {
        otherAnimator = GetComponent<Animator>();
        preBehavior = 0;
        index = parentObject.GetComponent<ServerOtherPlayerManager>().index;
        StartCoroutine(StartMotion());
    }

    void Update()
    {
        if (isMainCharacter == 1)
            AnimationControl();
        else if (isMainCharacter == 2)
            otherAnimator.SetBool("Run", false);
    }

    public void AnimationControl()
    {
        // Idle
        if (ServerLoginManager.playerList[index].mainCharacterBehavior == 0)
        {
            otherAnimator.SetBool("Run", false);
            preBehavior = 0;
        }
        // Run
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 1)
        {
            otherAnimator.SetBool("Run", true);
            preBehavior = 1;
        }
        // Dodge
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 2)
        {
            if (preBehavior != 2)
                StartCoroutine(DodgeDelay());
        }
        // Attack
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 3)
        {
            if (preBehavior != 3)
                StartCoroutine(AttackDelay());
        }
        // QSkill
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 4)
        {
            if (preBehavior != 4)
                StartCoroutine(BigAttack());
        }
        // WSkill
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 5)
        {
            if (preBehavior != 5)
                StartCoroutine(StraightAttack());
        }
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 6)
        {
            if (preBehavior != 6)
                StartCoroutine(Death());
        }
    }

    IEnumerator AttackDelay()
    {
        preBehavior = 3;
        otherAnimator.SetTrigger("Attack");
        leftStaff.enabled = true;
        rightStaff.enabled = true;
        yield return new WaitForSeconds(0.6f);
        leftStaff.enabled = false;
        rightStaff.enabled = false;
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
        preBehavior = 4;
        leftStaffEffect.SetActive(false);
        rightStaffEffect.SetActive(false);

        otherAnimator.SetTrigger("QSkill");
        otherAnimator.SetFloat("Speed", 0.2f);
        yield return new WaitForSeconds(0.5f);
        qSkill.SetActive(true);
        otherAnimator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(1.0f);
        otherAnimator.SetFloat("Speed", 1.0f);
        yield return new WaitForSeconds(1.0f);
        preBehavior = 0;
        qSkill.SetActive(false);
        leftStaffEffect.SetActive(true);
        rightStaffEffect.SetActive(true);

        otherAnimator.SetBool("Run", false);
    }
    IEnumerator StraightAttack()
    {
        preBehavior = 5;
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
        preBehavior = 0;
    }

    IEnumerator Death()
    {
        Debug.Log("other karmen Á×À½");
        preBehavior = 6;
        otherAnimator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.9f);
    }
}
