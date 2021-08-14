using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossCinemachine2PageEnter : MonoBehaviour
{
    [SerializeField] GameObject GroundPattern3Effect;
    [SerializeField] GameObject Pilot;

    Vector3 startPosition;

    Rigidbody rigidbody;

    public int maxHp = 2000;
    public int currentHp;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        //startPosition = new Vector3(22.02f, 25f, -0.1000023f);
        //transform.position = startPosition;
        StartCoroutine(StartEffect());
    }

    IEnumerator StartEffect()
    {
        Pilot.SetActive(false);
        yield return new WaitForSeconds(1.7f);
        rigidbody.useGravity = true;
        yield return new WaitForSeconds(2.2f);
        Instantiate(GroundPattern3Effect, transform.position + transform.up * 0.5f, Quaternion.Euler(90f, 0f, 0));
        yield return new WaitForSeconds(1f);

        //Pilot.SetActive(true);
        //motion start
        yield return new WaitForSeconds(9.05f);
        Pilot.SetActive(false);
        yield return new WaitForSeconds(10.65f);
        Instantiate(GroundPattern3Effect, transform.position + transform.up * 0.5f, Quaternion.Euler(90f, 0f, 0));
        //canAttack = true;
    }
}
