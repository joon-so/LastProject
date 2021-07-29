using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherEva : MonoBehaviour
{
    [SerializeField] GameObject qSkill;
    [SerializeField] GameObject wSkillEffect;
    [SerializeField] GameObject wSkillShockEffect;
    public Transform wSkillPos = null;

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
        if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 0)
        {
            otherAnimator.SetBool("Run", false);
            preBehavior = 0;
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
                StartCoroutine(AttackDelay());
        }
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 4)
        {
            if (preBehavior != 4)
            {
                StartCoroutine(FireGun());
                preBehavior = 4;
            }
        }
        else if (ServerLoginManager.playerList[ServerOtherPlayerManager.instance.index].mainCharacterBehavior == 5)
        {
            if (preBehavior != 5)
            {
                StartCoroutine(ShockWave());
                preBehavior = 5;
            }
        }
    }
    IEnumerator StartMotion()
    {
        yield return new WaitForSeconds(0.5f);
        otherAnimator.SetTrigger("StartMotion");
        preBehavior = 0;
    }
    IEnumerator DodgeDelay()
    {
        preBehavior = 2; 
        otherAnimator.SetTrigger("Dodge");
        yield return new WaitForSeconds(1.0f);
        preBehavior = 0;
    }
    IEnumerator AttackDelay()
    {
        preBehavior = 3;
        otherAnimator.SetTrigger("Attack"); 
        yield return new WaitForSeconds(1.0f);
        preBehavior = 0;
    }
    IEnumerator FireGun()
    {
        preBehavior = 4;

        qSkill.SetActive(true);
        otherAnimator.SetTrigger("QSkill");
  
        yield return new WaitForSeconds(5.0f);
        qSkill.SetActive(false);

        otherAnimator.SetBool("Run", false);
        preBehavior = 0;
    }

    IEnumerator ShockWave()
    {
        preBehavior = 5;

        otherAnimator.SetTrigger("WSkill");
        Instantiate(wSkillEffect, wSkillPos.position, wSkillPos.rotation);
        yield return new WaitForSeconds(0.5f);
        otherAnimator.SetFloat("Speed", 0.0f);
        yield return new WaitForSeconds(0.5f);
        otherAnimator.SetFloat("Speed", 1.0f);

        yield return new WaitForSeconds(0.3f);
        // ����
        wSkillShockEffect.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 1.5f, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        wSkillShockEffect.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 3f + transform.right, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 3f + -transform.right, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        wSkillShockEffect.transform.localScale = new Vector3(0.55f, 0.55f, 0.55f);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 5f + transform.right * 2.0f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 5f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 5f + -transform.right * 2.0f, transform.rotation);
        yield return new WaitForSeconds(0.4f);
        wSkillShockEffect.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 7f - transform.right * 3f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 7f - transform.right * 1.2f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 7f + transform.right * 1.2f, transform.rotation);
        Instantiate(wSkillShockEffect, transform.position + transform.forward * 7f + transform.right * 3f, transform.rotation);

        yield return new WaitForSeconds(0.5f);
        otherAnimator.SetFloat("Speed", 1.0f);
        preBehavior = 0;
    }
}
