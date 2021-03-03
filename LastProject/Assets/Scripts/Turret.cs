using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform turretGun = null;
    [SerializeField] float range = 0f;
    [SerializeField] LayerMask layerMask = 0;
    [SerializeField] float spinSpeed = 0f;
    [SerializeField] float fireRate = 0f;

    float currentFireRate;
    Transform target = null;

    void Start()
    {
        currentFireRate = fireRate;
        InvokeRepeating("Search", 0f, 0.5f);
    }

    void Update()
    {
        if (target == null)
        {
            turretGun.Rotate(new Vector3(0, 45, 0) * Time.deltaTime);
        }
        else
        {
            Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
            Vector3 euler = Quaternion.RotateTowards(turretGun.rotation, lookRotation, spinSpeed * Time.deltaTime).eulerAngles;
            turretGun.rotation = Quaternion.Euler(0, euler.y, 0);

            Quaternion fireRotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
            if (Quaternion.Angle(turretGun.rotation, fireRotation) < 5f)
            {
                currentFireRate -= Time.deltaTime;
                if (currentFireRate <= 0)
                {
                    currentFireRate = fireRate;
                    Debug.Log("น฿ป็");
                }
            }
        }
    }

    void Search()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range, layerMask);
        Transform closestTarget = null;

        if (colliders.Length > 0)
        {
            float closestDistance = Mathf.Infinity;
            foreach (Collider colliderTarget in colliders)
            {
                float distance = Vector3.SqrMagnitude(transform.position - colliderTarget.transform.position);
                if (closestDistance > distance)
                {
                    closestDistance = distance;
                    closestTarget = colliderTarget.transform;
                }
            }
        }
        target = closestTarget;
    }
}