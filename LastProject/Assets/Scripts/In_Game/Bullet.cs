using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public float range = 11.0f;
    Vector3 startPoint;

    void Start()
    {
        startPoint = transform.position;
    }

    void Update()
    {
        if(Vector3.Distance(startPoint, transform.position) > range)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}