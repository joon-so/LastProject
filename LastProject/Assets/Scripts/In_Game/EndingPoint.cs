using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingPoint : MonoBehaviour
{
    [SerializeField] AudioClip mainClip;

    private void OnCollisionEnter(Collision collision)
    {
        SceneManager.LoadScene("Stage1-2");
    }
}