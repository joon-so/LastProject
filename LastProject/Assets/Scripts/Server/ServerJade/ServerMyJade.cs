using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ServerMyJade : ServerSubAIManager
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

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float fireDelay = 0.8f;
    public float subFireDelay = 1.5f;
    public float followDistance = 5.0f;
    public float grenadeDistance = 10.0f;

    public static float qSkillCoolTime = 5.0f;
    public static float wSkillCoolTime = 5.0f;

    float curFireDelay;
    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;

    bool canMove;
    bool canDodge;
    bool canAttack;
    bool canSkill;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;

    Vector3 vecTarget;
    Animator myAnimator;
    Rigidbody rigidbody; 
    ServerCollisionManager collisionManager;
    ServerSkillEpManager skillEpManager;

    int characterIndex;

    [SerializeField] AudioClip jadeAttackSound;
    [SerializeField] AudioClip jadeQSkillSound;
    [SerializeField] AudioClip jadeWSkillSound;

    void Awake()
    {
        myAnimator = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        collisionManager = GameObject.Find("ServerIngameManager").GetComponent<ServerCollisionManager>();
        skillEpManager = GameObject.Find("ServerIngameManager").GetComponent<ServerSkillEpManager>();
    }

    void Start()
    {
        curDodgeCoolTime = dodgeCoolTime;
        curQSkillCoolTime = qSkillCoolTime;
        curWSkillCoolTime = wSkillCoolTime;
        
        if (gameObject.transform.CompareTag("MainCharacter"))
        {
            tagCharacter = ServerMyPlayerManager.instance.character2;

            characterIndex = 1;

            ServerMyPlayerManager.instance.c1DodgeCoolTime = dodgeCoolTime;
            ServerMyPlayerManager.instance.c1QSkillCoolTime = qSkillCoolTime;
            ServerMyPlayerManager.instance.c1WSkillCoolTime = wSkillCoolTime;

            ServerMyPlayerManager.instance.curC1DodgeCoolTime = curDodgeCoolTime;
            ServerMyPlayerManager.instance.curC1QSkillCoolTime = curQSkillCoolTime;
            ServerMyPlayerManager.instance.curC1WSkillCoolTime = curWSkillCoolTime;
            nav.enabled = false;
        }
        else if (gameObject.transform.CompareTag("SubCharacter"))
        {
            tagCharacter = ServerMyPlayerManager.instance.character1;

            characterIndex = 2;

            ServerMyPlayerManager.instance.c2DodgeCoolTime = dodgeCoolTime;
            ServerMyPlayerManager.instance.c2QSkillCoolTime = qSkillCoolTime;
            ServerMyPlayerManager.instance.c2WSkillCoolTime = wSkillCoolTime;

            ServerMyPlayerManager.instance.curC2DodgeCoolTime = curDodgeCoolTime;
            ServerMyPlayerManager.instance.curC2QSkillCoolTime = curQSkillCoolTime;
            ServerMyPlayerManager.instance.curC2WSkillCoolTime = curWSkillCoolTime;
            nav.enabled = true;
        }
        FindPlayers();
        vecTarget = transform.position;

        canMove = false;
        canDodge = false;
        canAttack = false;
        canSkill = false;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;

        curFireDelay = fireDelay;
        
        StartCoroutine(DrawAssaultRifle());
    }

    void Update()
    {
        Tag();
        CoolTime();
        curFireDelay += Time.deltaTime;
        if (gameObject.transform.CompareTag("MainCharacter"))
        {
            if (canMove)
                Move();
            if (canAttack)
                Attack();
            if (canDodge)
                Dodge();
            if (canSkill)
            {
                Q_Skill();
                W_Skill();
            }
            Stop();
            Dead();
        }
        else if (gameObject.transform.CompareTag("SubCharacter"))
        {
            distance = Vector3.Distance(tagCharacter.transform.position, transform.position);

            if (currentState == characterState.trace)
            {
                MainCharacterTrace(tagCharacter.transform.position);
                myAnimator.SetBool("Run", true);
                ServerLoginManager.playerList[0].subCharacterBehavior = 1;
                curFireDelay = 1f;
            }
            else if (currentState == characterState.attack)
            {
                SubAttack();
                if (target)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
                    Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
                    transform.rotation = Quaternion.Euler(0, euler.y, 0);
                }
                if (curFireDelay > subFireDelay && target != null)
                {
                    GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
                    Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                    bulletRigid.velocity = assaultRifleBulletPos.forward;

                    moveSpeed = 0f;
                    myAnimator.SetBool("Run", false);
                    vecTarget = transform.position;

                    myAnimator.SetTrigger("shootAssaultRifle");
                    curFireDelay = 0;

                    ServerLoginManager.playerList[0].subCharacterBehavior = 3;
                    StartCoroutine(AttackDelay());
                }
            }
            else if (currentState == characterState.idle)
            {
                Idle();
                ServerLoginManager.playerList[0].subCharacterBehavior = 0;
                myAnimator.SetBool("Run", false);
                curFireDelay = 1f;
            }
        }
    }
    void FixedUpdate()
    {
        FindPlayers();
    }
    void Move()
    {
        if (Input.GetMouseButton(1))
        {
            moveSpeed = 5.0f;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                vecTarget = hit.point;
                vecTarget.y = transform.position.y;

                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, vecTarget, moveSpeed * Time.deltaTime);
        myAnimator.SetBool("Run", vecTarget != transform.position);

        ServerLoginManager.playerList[0].mainCharacterBehavior = 1;

        if (vecTarget == transform.position)
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }
    void Stop()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            vecTarget = transform.position;
            myAnimator.SetBool("Run", false);
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
        }
    }
    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.Space) && onDodge)
        {
            onDodge = false;

            canAttack = false;
            canMove = false;
            canSkill = false;

            curDodgeCoolTime = 0.0f;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Vector3 nextVec = hit.point - transform.position;
                nextVec.y = 0;
                transform.LookAt(transform.position + nextVec);
            }

            moveSpeed *= 2;
            myAnimator.SetTrigger("Dodge");
            ServerLoginManager.playerList[0].mainCharacterBehavior = 2;

            StartCoroutine(DodgeDelay());

        }
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("DodgeForward"))
        {
            transform.Translate(Vector3.forward * 5 * Time.deltaTime);
            vecTarget = transform.position;
            myAnimator.SetBool("Run", false);
        }
    }
    void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (curFireDelay > fireDelay)
            {
                canMove = false;
                canDodge = false;
                canSkill = false;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    Vector3 nextVec = hit.point - transform.position;
                    nextVec.y = 0;
                    transform.LookAt(transform.position + nextVec);
                }

                SoundManager.instance.SFXPlay("JadeAttack", jadeAttackSound);

                GameObject instantBullet = Instantiate(assaultRifleBullet, assaultRifleBulletPos.position, assaultRifleBulletPos.rotation);
                Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = assaultRifleBulletPos.forward;

                myAnimator.SetBool("Run", false);
                vecTarget = transform.position;

                myAnimator.SetTrigger("shootAssaultRifle");
                ServerLoginManager.playerList[0].mainCharacterBehavior = 3;
                curFireDelay = 0;

                StartCoroutine(AttackDelay());
            }
        }
        if (Input.GetMouseButtonUp(0))
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    void Dead()
    {
        if (characterIndex == 1)
        {
            if (ServerLoginManager.playerList[0].character1Hp <= 0)
            {
                StartCoroutine(Death());
            }
        }
        else if (characterIndex == 2)
        {
            if (ServerLoginManager.playerList[0].character2Hp <= 0)
            {
                StartCoroutine(Death());
            }
        }
    }

    void CoolTime()
    {
        if (curDodgeCoolTime < dodgeCoolTime)
        {
            curDodgeCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                ServerMyPlayerManager.instance.curC1DodgeCoolTime = curDodgeCoolTime;
            else if (characterIndex == 2)
                ServerMyPlayerManager.instance.curC2DodgeCoolTime = curDodgeCoolTime;
        }
        else
            onDodge = true;

        if (curQSkillCoolTime < qSkillCoolTime)
        {
            curQSkillCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                ServerMyPlayerManager.instance.curC1QSkillCoolTime = curQSkillCoolTime;
            else if (characterIndex == 2)
                ServerMyPlayerManager.instance.curC2QSkillCoolTime = curQSkillCoolTime;
        }
        else
            onQSkill = true;

        if (curWSkillCoolTime < wSkillCoolTime)
        {
            curWSkillCoolTime += Time.deltaTime;
            if (characterIndex == 1)
                ServerMyPlayerManager.instance.curC1WSkillCoolTime = curWSkillCoolTime;
            else if (characterIndex == 2)
                ServerMyPlayerManager.instance.curC2WSkillCoolTime = curWSkillCoolTime;
        }
        else
            onWSkill = true;
    }
    void Q_Skill()
    {
        if (Input.GetKeyDown(KeyCode.Q) && onQSkill)
        {
            onQSkill = false;
            curQSkillCoolTime = 0;
            myAnimator.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            if (ServerLoginManager.playerList[0].is_Main_Character == 1)
                ServerLoginManager.playerList[0].character1Ep -= skillEpManager.JadeQSkill();
            else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                ServerLoginManager.playerList[0].character2Ep -= skillEpManager.JadeQSkill();

            StartCoroutine(ShootMissile());
        }
    }

    void W_Skill()
    {
        if (Input.GetKeyDown(KeyCode.W) && onWSkill)
        {
            onWSkill = false;
            curWSkillCoolTime = 0;
            myAnimator.SetBool("Run", false);

            canAttack = false;
            canMove = false;
            canDodge = false;
            canSkill = false;

            if (ServerLoginManager.playerList[0].is_Main_Character == 1)
                ServerLoginManager.playerList[0].character1Ep -= skillEpManager.JadeWSkill();
            else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                ServerLoginManager.playerList[0].character2Ep -= skillEpManager.JadeWSkill();

            StartCoroutine(ShootGrenade());
        }
    }

    void Tag()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
                vecTarget = transform.position;
                Debug.Log("ÅÂ±×!!");
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.3f);
        canMove = true;
        canDodge = true;
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    IEnumerator DodgeDelay()
    {
        yield return new WaitForSeconds(1.0f);
        canAttack = true;
        canMove = true;
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    IEnumerator Death()
    {
        Debug.Log("my jade Á×À½");
        canMove = false;
        canAttack = false;
        canSkill = false;
        canDodge = false;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 6;
        myAnimator.SetTrigger("Dead");
        yield return new WaitForSeconds(1.9f);
    }

    IEnumerator DrawAssaultRifle()
    {
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetTrigger("drawAssaultRifle");
        yield return new WaitForSeconds(1.0f);
        backAssaultRifle.SetActive(false);
        useAssaultRifle.SetActive(true);
        canMove = true;
        canAttack = true;
        canDodge = true;
        canSkill = true;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
    }

    IEnumerator ShootMissile()
    {
        vecTarget = transform.position;

        ServerLoginManager.playerList[0].mainCharacterBehavior = 4;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            myAnimator.SetTrigger("drawMissileLauncher");
            yield return new WaitForSeconds(0.5f);
            useAssaultRifle.SetActive(false);
            useMissileLauncher.SetActive(true);

            myAnimator.SetBool("AimMissile", true);
            yield return new WaitForSeconds(0.5f);
            missileEffect.SetActive(true);
            SoundManager.instance.SFXPlay("JadeQSkill", jadeQSkillSound);

            yield return new WaitForSeconds(1.0f);
            myAnimator.SetBool("AimMissile", false);
            missileEffect.SetActive(false);

            myAnimator.SetTrigger("shootMissileLauncher");
            GameObject instantMissile = Instantiate(missileBullet, missileBulletPos.position, missileBulletPos.rotation);
            Rigidbody missileRigid = instantMissile.GetComponent<Rigidbody>();
            missileRigid.velocity = missileBulletPos.forward;

            yield return new WaitForSeconds(1.0f);

            myAnimator.SetTrigger("drawAssaultRifle");
            ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
            yield return new WaitForSeconds(0.5f);
            useMissileLauncher.SetActive(false);
            useAssaultRifle.SetActive(true);

            yield return new WaitForSeconds(0.3f);
            canAttack = true;
            canMove = true;
            canDodge = true;
            canSkill = true;
        }
    }
    IEnumerator ShootGrenade()
    {
        vecTarget = transform.position;
        ServerLoginManager.playerList[0].mainCharacterBehavior = 5;

        myAnimator.SetTrigger("shootGrenade");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);

            SoundManager.instance.SFXPlay("JadeWSkill", jadeWSkillSound);

            GameObject instantGrenade = Instantiate(Grenade, grenadePos.position, grenadePos.rotation);
            Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
            rigidGrenade.AddForce(transform.localRotation * Vector3.forward * 10.0f, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.3f);
        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
            return;

        if (ServerLoginManager.playerList[0].character1Hp <= 0 || ServerLoginManager.playerList[0].character2Hp <= 0)
            return;

        if (gameObject.CompareTag("MainCharacter"))
        {
            if (collision.gameObject.CompareTag("KarmenAttack"))
                collisionManager.KarmenBasicAttack();
            if (collision.gameObject.CompareTag("KarmenQSkill"))
                collisionManager.KarmenQSkillAttack();
            if (collision.gameObject.CompareTag("KarmenWSkill"))
                collisionManager.KarmenWSkillAttack();
            if (collision.gameObject.CompareTag("JadeAttack"))
                collisionManager.JadeBasicAttack();
            if (collision.gameObject.CompareTag("JadeQSkill"))
                collisionManager.JadeQSkillAttack();
            if (collision.gameObject.CompareTag("JadeWSkill"))
                collisionManager.JadeWSkillAttack();
            if (collision.gameObject.CompareTag("LeinaAttack"))
                collisionManager.LeinaBasicAttack();
            if (collision.gameObject.CompareTag("LeinaQSkill"))
                collisionManager.LeinaQSkillAttack();
            if (collision.gameObject.CompareTag("LeinaWSkill"))
                collisionManager.LeinaWSkillAttack();
            if (collision.gameObject.CompareTag("EvaAttack"))
                collisionManager.EvaBasicAttack();
            if (collision.gameObject.CompareTag("EvaWSkill"))
                collisionManager.EvaWSkillAttack();
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == 10)
            return;

        if (ServerLoginManager.playerList[0].character1Hp <= 0 || ServerLoginManager.playerList[0].character2Hp <= 0)
            return;

        if (gameObject.CompareTag("MainCharacter"))
        {
            if (other.gameObject.CompareTag("EvaQSkill"))
                collisionManager.EvaQSkillAttack();
        }
    }
}