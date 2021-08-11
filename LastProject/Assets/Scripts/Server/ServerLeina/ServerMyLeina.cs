using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ServerMyLeina : ServerSubAIManager
{
    [SerializeField] GameObject arrow;
    [SerializeField] Transform arrowPos;

    [SerializeField] GameObject posionArrow;
    [SerializeField] Transform posionArrowPos;

    [SerializeField] GameObject wSkillArrow;

    public float moveSpeed = 5.0f;
    public float dodgeCoolTime = 7.0f;
    public float fireDelay = 1.1f;
    public float subFireDelay = 1.5f;
    public float followDistance = 5.0f;

    public float qSkillCoolTime = 5.0f;
    public float wSkillCoolTime = 5.0f;

    float curFireDelay;
    float curDodgeCoolTime;
    float curQSkillCoolTime;
    float curWSkillCoolTime;

    bool canAttack;
    bool canDodge;
    bool canMove;
    bool canSkill;

    bool onDodge;
    bool onQSkill;
    bool onWSkill;

    Vector3 vecTarget;
    Animator myAnimator;
    ServerCollisionManager collisionManager;
    ServerSkillEpManager skillEpManager;

    int characterIndex;

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
        FindPlayers();

        vecTarget = transform.position;
        curDodgeCoolTime = dodgeCoolTime;
        curQSkillCoolTime = qSkillCoolTime;
        curWSkillCoolTime = wSkillCoolTime;

        canMove = true;
        canDodge = true;
        canAttack = true;
        canSkill = true;

        onDodge = true;
        onQSkill = true;
        onWSkill = true;

        curFireDelay = fireDelay;

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
        }
    }
    void Update()
    {
        curFireDelay += Time.deltaTime;
        if (gameObject.transform.tag == "MainCharacter")
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
        else if (gameObject.transform.tag == "SubCharacter")
        {
            //distance = Vector3.Distance(tagCharacter.transform.position, transform.position);
            //if (currentState == characterState.trace)
            //{
            //    MainCharacterTrace(tagCharacter.transform.position);
            //    myAnimator.SetBool("Run", true);
            //    curFireDelay = 1f;
            //}
            //else if (currentState == characterState.attack)
            //{
            //    SubAttack();

            //    if (target)
            //    {
            //        Quaternion lookRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            //        Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            //        transform.rotation = Quaternion.Euler(0, euler.y, 0);
            //    }
            //    if (curFireDelay > subFireDelay && target != null)
            //    {
            //        GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
            //        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
            //        arrowRigid.velocity = arrowPos.forward;

            //        moveSpeed = 0f;
            //        myAnimator.SetBool("Run", false);
            //        vecTarget = transform.position;

            //        myAnimator.SetTrigger("Attack");
            //        curFireDelay = 0;

            //        StartCoroutine(AttackDelay());
            //    }
            //}
            //else if (currentState == characterState.idle)
            //{
            //    Idle();
            //    myAnimator.SetBool("Run", false);
            //    curFireDelay = 1f;
            //}
        }
        CoolTime();
        Tag();
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
            myAnimator.SetBool("Run", false);
            vecTarget = transform.position;
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
        if (myAnimator.GetCurrentAnimatorStateInfo(0).IsName("Dodge"))
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

                GameObject instantArrow = Instantiate(arrow, arrowPos.position, arrowPos.rotation);
                Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
                arrowRigid.velocity = arrowPos.forward;

                myAnimator.SetBool("Run", false);
                vecTarget = transform.position;

                myAnimator.SetTrigger("Attack");
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
            if(characterIndex == 1)
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
                ServerLoginManager.playerList[0].character1Ep -= skillEpManager.LeinaQSkill();
            else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                ServerLoginManager.playerList[0].character2Ep -= skillEpManager.LeinaQSkill();

            StartCoroutine(ChargingShot());
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
                ServerLoginManager.playerList[0].character1Ep -= skillEpManager.LeinaWSkill();
            else if (ServerLoginManager.playerList[0].is_Main_Character == 2)
                ServerLoginManager.playerList[0].character2Ep -= skillEpManager.LeinaWSkill();

            StartCoroutine(WideShot());
        }
    }

    void Tag()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            vecTarget = transform.position;
        }
    }

    IEnumerator AttackDelay()
    {
        yield return new WaitForSeconds(0.5f);
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

    IEnumerator ChargingShot()
    {
        vecTarget = transform.position;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit rayHit;
        if (Physics.Raycast(ray, out rayHit, Mathf.Infinity))
        {
            Vector3 nextVec = rayHit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
            transform.Rotate(0, transform.rotation.y + 90, 0);
        }

        ServerLoginManager.playerList[0].mainCharacterBehavior = 4;

        myAnimator.SetTrigger("QSkill");
        // Â÷Â¡
        yield return new WaitForSeconds(1.4f);
        GameObject instantArrow = Instantiate(posionArrow, posionArrowPos.position, posionArrowPos.rotation);
        LeinaPosionArrow.speed = 0;
        // ¼¦
        yield return new WaitForSeconds(1f);

        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = posionArrowPos.forward;
        LeinaPosionArrow.speed = 40;

        ServerLoginManager.playerList[0].mainCharacterBehavior = 0;
        yield return new WaitForSeconds(1.0f);

        canAttack = true;
        canMove = true;
        canDodge = true;
        canSkill = true;
    }

    IEnumerator WideShot()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Vector3 nextVec = hit.point - transform.position;
            nextVec.y = 0;
            transform.LookAt(transform.position + nextVec);
        }

        //SoundManager.instance.SFXPlay("Attack", attackClip);

        myAnimator.SetBool("Run", false);
        vecTarget = transform.position;

        ServerLoginManager.playerList[0].mainCharacterBehavior = 5;

        myAnimator.SetTrigger("Attack");
        // ¼¦
        Vector3 pos = arrowPos.position;
        GameObject instantArrow = Instantiate(wSkillArrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -25f, 0));
        Rigidbody arrowRigid = instantArrow.GetComponent<Rigidbody>();
        arrowRigid.velocity = arrowPos.forward;

        GameObject instantArrow2 = Instantiate(wSkillArrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -15f, 0));
        Rigidbody arrowRigid2 = instantArrow2.GetComponent<Rigidbody>();
        arrowRigid2.velocity = arrowPos.forward;

        GameObject instantArrow3 = Instantiate(wSkillArrow, pos, arrowPos.rotation * Quaternion.Euler(0f, -5f, 0));
        Rigidbody arrowRigid3 = instantArrow3.GetComponent<Rigidbody>();
        arrowRigid3.velocity = arrowPos.forward;

        GameObject instantArrow4 = Instantiate(wSkillArrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 5f, 0));
        Rigidbody arrowRigid4 = instantArrow4.GetComponent<Rigidbody>();
        arrowRigid4.velocity = arrowPos.forward;

        GameObject instantArrow5 = Instantiate(wSkillArrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 15f, 0));
        Rigidbody arrowRigid5 = instantArrow5.GetComponent<Rigidbody>();
        arrowRigid5.velocity = arrowPos.forward;

        GameObject instantArrow6 = Instantiate(wSkillArrow, pos, arrowPos.rotation * Quaternion.Euler(0f, 25f, 0));
        Rigidbody arrowRigid6 = instantArrow6.GetComponent<Rigidbody>();
        arrowRigid6.velocity = arrowPos.forward;

        yield return new WaitForSeconds(0.5f);
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
