using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] GameObject GroundPattern1Bullet;
    public GameObject BulletPos;
    [SerializeField] GameObject GroundPattern2Effect;
    [SerializeField] GameObject GroundPattern2Gage;
    [SerializeField] GameObject GroundPattern3Effect;
    [SerializeField] GameObject GroundPattern3Gage;

    [SerializeField] GameObject FlyPattern1Effect;
    public GameObject Frame;
    [SerializeField] GameObject FlyPattern2Effect;
    //[SerializeField] GameObject FlyPattern3Effect;

    public GameObject FlyEffect;

    GameObject targetCharacter;

    Rigidbody rigidbody;
    Animator anim;
    NavMeshAgent nav;
    BoxCollider boxCollider;

    bool canMove;
    bool canAttack;
    bool canFollow;
    bool canDash;

    float skillCooltime;
    float playerDistance;
    float shootDistance;
    float detectDistance;
    float moveSpeed;
    float GroundPattern2Distance;
    float spinSpeed;

    int page;
    int pattern;

    ClientCollisionManager collisionManager;
    BossManager bossManager;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        boxCollider = GetComponent<BoxCollider>();

        collisionManager = GameObject.Find("GameManager").GetComponent<ClientCollisionManager>();
        bossManager = GameObject.Find("BossManager").GetComponent<BossManager>();

        targetCharacter = GameObject.FindGameObjectWithTag("SubCharacter");
        FlyEffect.SetActive(false);
        Frame.SetActive(false);

        nav.enabled = true;
        canMove = true;
        canAttack = true;
        canFollow = false;
        canDash = false;
        shootDistance = 15f;
        detectDistance = 30f;

        page = 1;
        pattern = 0;
        skillCooltime = 2f;
        moveSpeed = 10f;
        spinSpeed = 300f;
        GroundPattern2Distance = 20f;
        //StartCoroutine(StartEffect());
    }

    void FixedUpdate()
    {
        page = GameManager.instance.bossPage - 1;
        if (targetCharacter == null)
        {
            targetCharacter = GameObject.FindGameObjectWithTag("MainCharacter");
        }
        //Change Target
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (PlayerManager.instance.onTag)
                targetCharacter = GameObject.FindGameObjectWithTag("SubCharacter");
        }

        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            pattern = 0;
            canAttack = false;
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            pattern = 1;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            pattern = 2;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            pattern = 3;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            pattern = 4;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            pattern = 5;
            canAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            pattern = 6;
            canAttack = true;
        }

        Movement();
    }
    void Movement()
    {
        if (targetCharacter == null)
        {
            return;
        }
        else
        {
            playerDistance = Vector3.Distance(targetCharacter.transform.position, transform.position);

            if (canMove)
            {
                Quaternion lookRotation = Quaternion.LookRotation(targetCharacter.transform.position - transform.position);
                Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
                transform.rotation = Quaternion.Euler(0, euler.y, 0);
            }

            //Attack
            if (playerDistance < shootDistance)
            {
                nav.enabled = false;
                if (canAttack)
                {
                    canAttack = false;
                    if(page == 1)
                    {
                        pattern = (int)Random.Range(1, 4);
                    }
                    if(page == 2)
                    {
                        if(pattern == 5)
                        {
                            pattern = 6;
                        }
                        else
                        {
                            pattern = (int)Random.Range(1, 7);
                            if (pattern > 4)
                                pattern = 4;
                        }
                    }
                    Pattern(pattern);
                }
            }
            //Detect
            else if (playerDistance < detectDistance)
            {
                if (canMove)
                {
                    if (pattern != 5)
                        anim.SetBool("Run", true);
                    nav.enabled = true;
                    nav.SetDestination(targetCharacter.transform.position);
                }
            }
        }
    }

    IEnumerator StartEffect()
    {
        yield return new WaitForSeconds(22.8f);
        Instantiate(GroundPattern3Effect, transform.position + transform.up * 0.5f, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(27f);
        canAttack = true;
    }
    void Pattern(int BossPattern)
    {
        anim.SetBool("Run", false);
        if (BossPattern == 1)
        {
            StartCoroutine(Pattern1());
        }
        else if(BossPattern == 2)
        {
            StartCoroutine(Pattern2());
        }
        else if (BossPattern == 3)
        {
            StartCoroutine(Pattern3());
        }
        else if (BossPattern == 4)
        {
            StartCoroutine(Pattern4());
        }
        else if (BossPattern == 6)
        {
            StartCoroutine(Pattern6());
        }
    }
    IEnumerator Pattern1()
    {
        canMove = false;
        anim.SetInteger("Pattern", pattern);
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i< 20; i++)
        {
            Instantiate(GroundPattern1Bullet, BulletPos.transform.position, transform.rotation);
            yield return new WaitForSeconds(0.16f);
        }
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canMove = true;
        yield return new WaitForSeconds(skillCooltime);
        canAttack = true;
    }
    IEnumerator Pattern2()
    {
        canMove = false;
        anim.SetInteger("Pattern", pattern);
        Instantiate(GroundPattern2Gage, transform.position + transform.up  * 0.5f, transform.rotation * Quaternion.Euler(0f, 90f, 0));

        yield return new WaitForSeconds(1.2f);
        canDash = true;
        Instantiate(GroundPattern2Effect, transform.position + transform.up * 3f + transform.forward * 2.5f, transform.rotation);
        float skillTime = 0f;
        while(skillTime < 1.5f) {
            skillTime += Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward * GroundPattern2Distance, moveSpeed * Time.deltaTime);
            yield return null;
        }

        anim.SetTrigger("SprintEnd");
        yield return new WaitForSeconds(0.2f);
        canDash = false;
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canMove = true;
        yield return new WaitForSeconds(skillCooltime);
        canAttack = true;
    }
    IEnumerator Pattern3()
    {
        canMove = false;
        anim.SetInteger("Pattern", pattern);
        Vector3 pos = transform.position + transform.forward * 2.2f - transform.right * 0.9f + transform.up * 0.5f;
        Instantiate(GroundPattern3Gage, pos, Quaternion.Euler(0f, 90f, 0));
        yield return new WaitForSeconds(0.6f);
        Instantiate(GroundPattern3Effect, pos, Quaternion.Euler(90f, 0f, 0));
        if (Vector3.Distance(transform.position, targetCharacter.transform.position) < 11f)
        {
            collisionManager.BossAttack3();
        }
        yield return new WaitForSeconds(0.72f);
        Instantiate(GroundPattern3Effect, pos, Quaternion.Euler(90f, 0f, 0));
        if (Vector3.Distance(transform.position, targetCharacter.transform.position) < 11f)
        {
            collisionManager.BossAttack3();
        }
        yield return new WaitForSeconds(0.72f);
        Instantiate(GroundPattern3Effect, pos, Quaternion.Euler(90f, 0f, 0));
        if (Vector3.Distance(transform.position, targetCharacter.transform.position) < 11f)
        {
            collisionManager.BossAttack3();
        }
        yield return new WaitForSeconds(1f);
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canMove = true;
        yield return new WaitForSeconds(skillCooltime);
        canAttack = true;
    }
    IEnumerator Pattern4()
    {
        canMove = false;
        FlyEffect.SetActive(true);
        anim.SetInteger("Pattern", pattern);
        Instantiate(GroundPattern2Gage, transform.position + transform.up * 0.5f, transform.rotation * Quaternion.Euler(0f, 90f, 0));
        Vector3 P1 = transform.position;
        Vector3 P2 = targetCharacter.transform.position + new Vector3(0, -5f, 0);
        Vector3 P3 = targetCharacter.transform.position + transform.forward * 9f;
        Vector3 bezier;

        yield return new WaitForSeconds(1f);
        canDash = true;
        rigidbody.useGravity = false;
        float bezierValue = 0f;
        float shootTime = 0.8f;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y/4, boxCollider.size.z);
        while (bezierValue < shootTime)
        {
            bezierValue += Time.deltaTime;

            bezier = Bezier(P1, P2, P3, bezierValue * 1 / shootTime);
            transform.position = bezier;
            yield return null;
        }
        rigidbody.useGravity = true;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y * 4, boxCollider.size.z);
        //yield return new WaitForSeconds(0.1f);
        float rotateTime = 0;
        while(rotateTime < 0.7f)
        {
            rotateTime += Time.deltaTime;
            Quaternion lookRotation = Quaternion.LookRotation(targetCharacter.transform.position - transform.position);
            Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            transform.rotation = Quaternion.Euler(0, euler.y, 0);

            yield return null;
        }
        Frame.SetActive(true);
        canMove = true;
        canDash = false;
        yield return new WaitForSeconds(2.7f);
        Frame.SetActive(false);
        //yield return new WaitForSeconds(f);
        pattern = 5;
        anim.SetInteger("Pattern", pattern);
        yield return new WaitForSeconds(skillCooltime);
        canAttack = true;
    }
    IEnumerator Pattern6()
    {
        canMove = false;
        anim.SetInteger("Pattern", pattern);
        //transform.LookAt(targetCharacter.transform);
        Instantiate(GroundPattern2Gage, transform.position + transform.up * 0.5f, transform.rotation * Quaternion.Euler(0f, 90f, 0));
        Vector3 P1 = transform.position;
        Vector3 P2 = targetCharacter.transform.position + new Vector3(0, -5f, 0);
        Vector3 P3 = targetCharacter.transform.position + transform.forward * 9f;
        Vector3 bezier;
        yield return new WaitForSeconds(0.7f);
        canDash = true;
        rigidbody.useGravity = false;
        float bezierValue = 0f;
        float shootTime = 0.7f;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y / 4, boxCollider.size.z);
        while (bezierValue < shootTime)
        {
            bezierValue += Time.deltaTime;

            bezier = Bezier(P1, P2, P3, bezierValue * 1 / shootTime);
            //transform.LookAt(bezier);
            transform.position = bezier;
            yield return null;
        }
        canDash = false;
        rigidbody.useGravity = true;
        boxCollider.size = new Vector3(boxCollider.size.x, boxCollider.size.y * 4, boxCollider.size.z);

        yield return new WaitForSeconds(0.7f);
        Instantiate(GroundPattern3Effect, transform.position + transform.up * 0.5f, Quaternion.Euler(90f, 0f, 0));
        if (Vector3.Distance(transform.position, targetCharacter.transform.position) < 11f)
        {
            collisionManager.BossAttack3();
        }
        FlyEffect.SetActive(false);
        pattern = 0;
        anim.SetInteger("Pattern", pattern);
        canMove = true;
        yield return new WaitForSeconds(skillCooltime);
        canAttack = true;
    }

    public void HitJadeGrenade()
    {
        if(GameManager.instance.bossPage == 2)
            bossManager.curBoss2PageHp -= collisionManager.jadeWSkillDamage;
        if (GameManager.instance.bossPage == 3)
            bossManager.curBoss3PageHp -= collisionManager.jadeWSkillDamage;
    }
    public void HitEvaQSkill()
    {
        if (GameManager.instance.bossPage == 2)
            bossManager.curBoss2PageHp -= collisionManager.evaQSkillDamage;
        if (GameManager.instance.bossPage == 3)
            bossManager.curBoss3PageHp -= collisionManager.evaQSkillDamage;
    }
    void OnCollisionEnter(Collision collision)
    {
        // Karmen
        if (collision.gameObject.tag == "KarmenAttack")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.karmenAttackDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.karmenAttackDamage;
        }
        if (collision.gameObject.tag == "KarmenQSkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.karmenQSkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.karmenQSkillDamage;
        }
        if (collision.gameObject.tag == "KarmenWSkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.karmenWSkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.karmenWSkillDamage;
        }
        if (collision.gameObject.tag == "KarmenESkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.karmenESkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.karmenESkillDamage;
        }
        // Jade
        if (collision.gameObject.tag == "JadeAttack")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.jadeAttackDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.jadeAttackDamage;
        }
        if (collision.gameObject.tag == "JadeQSkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.jadeQSkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.jadeQSkillDamage;
        }
        if (collision.gameObject.tag == "JadeESkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.jadeESkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.jadeESkillDamage;
        }
        // Leina
        if (collision.gameObject.tag == "LeinaAttack")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.leinaAttackDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.leinaAttackDamage;
        }
        if (collision.gameObject.tag == "LeinaQSkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.leinaQSkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.leinaQSkillDamage;
        }
        if (collision.gameObject.tag == "LeinaWSkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.leinaWSkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.leinaWSkillDamage;
        }
        if (collision.gameObject.tag == "LeinaESkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.leinaESkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.leinaESkillDamage;
        }
        // Eva
        if (collision.gameObject.tag == "EvaAttack")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.evaAttackDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.evaAttackDamage;
        }
        if (collision.gameObject.tag == "EvaQSkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.evaQSkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.evaQSkillDamage;
        }
        if (collision.gameObject.tag == "EvaWSkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.evaWSkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.evaWSkillDamage;
        }
        if (collision.gameObject.tag == "EvaESkill")
        {
            if (GameManager.instance.bossPage == 2)
                bossManager.curBoss2PageHp -= collisionManager.evaESkillDamage;
            if (GameManager.instance.bossPage == 3)
                bossManager.curBoss3PageHp -= collisionManager.evaESkillDamage;
        }
    }

    Vector3 Bezier(Vector3 P_1, Vector3 P_2, Vector3 P_3, float value)
    {
        Vector3 A = Vector3.Lerp(P_1, P_2, value);
        Vector3 B = Vector3.Lerp(P_2, P_3, value);
        Vector3 D = Vector3.Lerp(A, B, value);

        return D;
    }
}
