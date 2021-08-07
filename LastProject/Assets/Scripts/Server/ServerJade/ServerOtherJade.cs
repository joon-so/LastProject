using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerOtherJade : MonoBehaviour
{
    [SerializeField] GameObject useAssaultRifle;
    [SerializeField] GameObject backAssaultRifle;

    [SerializeField] Transform assaultRifleBulletPos;
    [SerializeField] GameObject assaultRifleBullet;

    [SerializeField] GameObject useMissileLauncher;
    [SerializeField] GameObject backMissileLauncher;

    [SerializeField] Transform missileBulletPos;
    [SerializeField] GameObject missileBullet;
    [SerializeField] GameObject missileRange;
    [SerializeField] GameObject missileEffect;

    [SerializeField] Transform grenadePos;
    [SerializeField] GameObject Grenade;

    [SerializeField] GameObject parentObject;

    public int isMainCharacter;

    private Animator otherAnimator;

    private int preBehavior;
    private int index;

    void Start()
    {
        otherAnimator = GetComponent<Animator>();
        index = parentObject.GetComponent<ServerOtherPlayerManager>().index; 
        preBehavior = 0;
        StartCoroutine(DrawAssaultRifle());
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
        if (ServerLoginManager.playerList[index].mainCharacterBehavior == 0)
        {
            otherAnimator.SetBool("Run", false);
            preBehavior = 0;
        }
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 1)
        {
            otherAnimator.SetBool("Run", true);
            preBehavior = 1;
        }
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 2)
        {
            if (preBehavior != 2)
                StartCoroutine(DodgeDelay());
        }
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 3)
        {
            if (preBehavior != 3)
                StartCoroutine(ShootAssaultRifle());
        }
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 4)
        {
            if (preBehavior != 4)
                StartCoroutine(ShootMissile());
        }
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 5)
        {
            if (preBehavior != 5)
                StartCoroutine(ShootGrenade());
        }
        else if (ServerLoginManager.playerList[index].mainCharacterBehavior == 6)
        {
            if (preBehavior != 6)
                StartCoroutine(Death());
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
    IEnumerator DodgeDelay()
    {
        preBehavior = 2;
        otherAnimator.SetTrigger("Dodge");
        yield return new WaitForSeconds(1.0f);
        preBehavior = 0;
    }
    IEnumerator ShootAssaultRifle()
    {
        preBehavior = 3;
        otherAnimator.SetTrigger("shootAssaultRifle");
        GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = assaultRifleBulletPos.forward;
        yield return new WaitForSeconds(0.3f);
        preBehavior = 0;
    }
    IEnumerator ShootMissile()
    {
        preBehavior = 4;
        otherAnimator.SetTrigger("drawMissileLauncher");
        yield return new WaitForSeconds(0.5f);
        useAssaultRifle.SetActive(false);
        useMissileLauncher.SetActive(true);

        otherAnimator.SetBool("Run", false);
        otherAnimator.SetBool("AimMissile", true);
        yield return new WaitForSeconds(0.5f);
        missileEffect.SetActive(true);

        //SoundManager.instance.SFXPlay("Attack", qSkillClip);

        yield return new WaitForSeconds(1.0f);
        otherAnimator.SetBool("AimMissile", false);
        missileEffect.SetActive(false);

        otherAnimator.SetTrigger("shootMissileLauncher");
        GameObject instantMissile = Instantiate(missileBullet, missileBulletPos.position, missileBulletPos.rotation);
        Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
        missileRigid.velocity = missileBulletPos.forward;

        yield return new WaitForSeconds(1.0f);

        otherAnimator.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(0.5f);
        useMissileLauncher.SetActive(false);
        useAssaultRifle.SetActive(true);

        yield return new WaitForSeconds(0.3f);
        preBehavior = 0;
    }
    IEnumerator ShootGrenade()
    {
        preBehavior = 5;
        otherAnimator.SetTrigger("shootGrenade");

        //SoundManager.instance.SFXPlay("Grenade", wSkillClip);
        GameObject instantGrenade = Instantiate(Grenade, grenadePos.position, grenadePos.rotation);
        Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
        rigidGrenade.AddForce(transform.position, ForceMode.Impulse);
        rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(0.3f);
        preBehavior = 0;
    }

    IEnumerator Death()
    {
        otherAnimator.SetTrigger("Dead");
        yield return null;
    }
}
