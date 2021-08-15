using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2To3CharacterSelect : MonoBehaviour
{
    [SerializeField] GameObject karmen;
    [SerializeField] GameObject jade;
    [SerializeField] GameObject leina;
    [SerializeField] GameObject eva;

    void Start()
    {
        if(GameManager.instance.clientPlayer.curMainCharacter == 1)
        {
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
        }

        if (GameManager.instance.clientPlayer.curMainCharacter == 2)
        {
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
        }
    }
}