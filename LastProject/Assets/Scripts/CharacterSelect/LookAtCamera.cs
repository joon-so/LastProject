using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] Camera camera;

    void Update()
    {
        Vector3 pos = camera.transform.position - transform.position;
        pos.x = pos.z = 0;
        transform.LookAt(camera.transform.position - pos);
    }
}
