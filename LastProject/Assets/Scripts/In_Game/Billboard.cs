using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform cam;
    private GameObject mainCamera;

    void LateUpdate()
    {
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            cam = mainCamera.transform;
        }

        transform.LookAt(transform.position + cam.forward);    
    }
}
