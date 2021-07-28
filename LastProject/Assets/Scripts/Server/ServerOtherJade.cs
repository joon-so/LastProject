using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherJade : MonoBehaviour
{
    [SerializeField] GameObject useAssaultRifle;
    [SerializeField] GameObject backAssaultRifle;

    [SerializeField] Transform assaultRifleBulletPos;
    [SerializeField] GameObject assaultRifleBullet;

    private Animator otherAnimator;
    private int preBehavior;

    void Start()
    {
        otherAnimator = GetComponent<Animator>();
        preBehavior = 0;
        StartCoroutine(DrawAssaultRifle());
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
                otherAnimator.SetTrigger("shootAssaultRifle");
                StartCoroutine(ShootAssaultRifle());
                preBehavior = 3;
            }

        }
    }

    IEnumerator DrawAssaultRifle()
    {
        yield return new WaitForSeconds(0.5f);
        otherAnimator.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(1.0f);
        backAssaultRifle.SetActive(false);
        useAssaultRifle.SetActive(true);
    }

    IEnumerator ShootAssaultRifle()
    {
        GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = assaultRifleBulletPos.forward;
        yield return null;
    }
}
