using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy6 : MonoBehaviour
{
    [SerializeField] [Range(10f, 30f)] float detectDistance = 15f;
    [SerializeField] [Range(10f, 20f)] float shootDistance = 10f;
    [SerializeField] GameObject targetMark = null;
    [SerializeField] GameObject bullet = null;
    [SerializeField] GameObject explosion = null;
    [SerializeField] GameObject dropEffect = null;
    [SerializeField] Transform LeftBulletPoint;
    [SerializeField] Transform RightBulletPoint;
    [SerializeField] AudioClip deadSound;
    protected List<GameObject> targets;

    public float shootCooltime = 1.0f;
    public float spinSpeed = 200.0f;

    bool shootable = true;
    bool movable = true;
    bool alive = true;

    NavMeshAgent nav;
    float playerDistance;
    GameObject mainCharacter;
    Vector3 startPoint;

    public static int damage = 40;

    // Ã¼·Â
    public int maxHp = 200;
    public int currentHp;
    public HpBar hpBar;

    void Start()
    {
        nav = GetComponent<NavMeshAgent>();

        targets = GameObject.Find("Enemys").GetComponent<EnemyList>().Enemys;

        targetMark.SetActive(false);
        nav.enabled = false;

        startPoint = new Vector3(transform.position.x, 0f, transform.position.z);

        currentHp = maxHp;
        hpBar.SetMaxHp(maxHp);

        detectDistance = 20f;
        shootDistance = 15f;
        shootable = true;
        movable = true;
    }
                                                                                                   
    void FixedUpdate()
    {
        if (mainCharacter == null)
        {
            mainCharacter = GameObject.FindGameObjectWithTag("MainCharacter");
        }
        if (currentHp <= 0 && alive)
        {
            alive = false;
            StartCoroutine(ExploseAndDistroy());
        }
        Find();
        if(transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        //Change Target
        if (Input.GetKeyDown(KeyCode.F))
        {
            mainCharacter = GameObject.FindGameObjectWithTag("SubCharacter");
        }
    }

    void Find()
    {
        if (mainCharacter == null)
        {
            return;
        }
        else
        {
            playerDistance = Vector3.Distance(mainCharacter.transform.position, transform.position);

            //Attack
            if(playerDistance < shootDistance)
            {
                Quaternion lookRotation = Quaternion.LookRotation(mainCharacter.transform.position - transform.position);
                Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
                transform.rotation = Quaternion.Euler(0, euler.y, 0);

                nav.enabled = false;
                if (shootable)
                {
                    shootable = false;
                    StartCoroutine(Attack());
                }
            }
            //Detect
            else if (playerDistance < detectDistance)
            {
                Quaternion lookRotation = Quaternion.LookRotation(mainCharacter.transform.position - transform.position);
                Vector3 euler = Quaternion.RotateTowards(transform.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
                transform.rotation = Quaternion.Euler(0, euler.y, 0);

                if (movable)
                {
                    nav.enabled = true;
                    nav.SetDestination(mainCharacter.transform.position);
                }
            }
            else
            {
                if (movable)
                    nav.SetDestination(startPoint);
            }
        }
    }

    IEnumerator Attack()
    {
        movable = false;
        //yield return new WaitForSeconds(0.5f);
        Instantiate(bullet, LeftBulletPoint.transform.position, LeftBulletPoint.transform.rotation);
        yield return new WaitForSeconds(0.3f);
        Instantiate(bullet, RightBulletPoint.transform.position, RightBulletPoint.transform.rotation);
        yield return new WaitForSeconds(0.9f);
        shootable = true;
        movable = true;
    }

    IEnumerator ExploseAndDistroy()
    {
        SoundManager.instance.SFXPlay("Explosion", deadSound);
        shootable = false;
        movable = false;
        Instantiate(explosion, transform.position, transform.rotation);
        targets.Remove(gameObject);
        GetComponent<TriangleExplosion>().ExplosionMesh();
        yield return new WaitForSeconds(1f);
        //Destroy(gameObject);
    }

    public void HitJadeGrenade()
    {
        currentHp -= Jade.wSkillDamage;
        hpBar.SetHp(currentHp);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "KarmenAttack")
        {
            currentHp -= Karmen.attackDamage;
            hpBar.SetHp(currentHp);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Karmen
        if (collision.gameObject.tag == "KarmenAttack")
        {
            currentHp -= Karmen.attackDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "KarmenQSkill")
        {
            currentHp -= Karmen.qSkillDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "KarmenWSkill")
        {
            currentHp -= Karmen.wSkillDamage;
            hpBar.SetHp(currentHp);
        }
        // Jade
        if (collision.gameObject.tag == "JadeAttack")
        {
            currentHp -= Jade.attackDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "JadeQSkill")
        {
            currentHp -= Jade.qSkillDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "JadeWSkill")
        {
            currentHp -= Jade.wSkillDamage;
            hpBar.SetHp(currentHp);
        }
        // Leina
        if (collision.gameObject.tag == "LeinaAttack")
        {
            currentHp -= Leina.attackDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "LeinaQSkill")
        {
            currentHp -= Leina.qSkillDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "LeinaWSkill")
        {
            currentHp -= Leina.wSkillDamage;
            hpBar.SetHp(currentHp);
        }
        // Eva
        if (collision.gameObject.tag == "EvaAttack")
        {
            currentHp -= Eva.attackDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "EvaQSkill")
        {
            currentHp -= Eva.qSkillDamage;
            hpBar.SetHp(currentHp);
        }
        if (collision.gameObject.tag == "EvaWSkill")
        {
            currentHp -= Eva.wSkillDamage;
            hpBar.SetHp(currentHp);
        }
    }

}