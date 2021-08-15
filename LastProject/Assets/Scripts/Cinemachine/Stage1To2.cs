using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1To2 : MonoBehaviour
{
    LevelLoader levelLoader;

    [SerializeField] GameObject karmen;
    [SerializeField] GameObject jade;
    [SerializeField] GameObject leina;
    [SerializeField] GameObject eva;
    void Start()
    {
        SetActiveManager.instance.SetActiveFalse();

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        if (GameManager.instance.clientPlayer.selectCharacter1 == 1)
        {
            karmen.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 2)
        {
            jade.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 3)
        {
            leina.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter1 == 4)
        {
            eva.SetActive(true);
        }

        if (GameManager.instance.clientPlayer.selectCharacter2 == 1)
        {
            karmen.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 2)
        {
            jade.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 3)
        {
            leina.SetActive(true);
        }
        else if (GameManager.instance.clientPlayer.selectCharacter2 == 4)
        {
            eva.SetActive(true);
        }

        StartCoroutine(NextScene());
    }
    IEnumerator NextScene()
    {
        yield return new WaitForSeconds(6.5f);
        levelLoader.LoadNextLevel();
    }
}