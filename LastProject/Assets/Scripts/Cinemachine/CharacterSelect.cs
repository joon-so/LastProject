using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelect : MonoBehaviour
{
    LevelLoader levelLoader;

    [SerializeField] GameObject karmen;
    [SerializeField] GameObject jade;
    [SerializeField] GameObject leina;
    [SerializeField] GameObject eva;

    Vector3 movePoint1;
    Vector3 movePoint2;
    Vector3 movePoint3;

    // Start is called before the first frame update
    void Start()
    {
        SetActiveManager.instance.SetActiveFalse();

        levelLoader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();

        movePoint1 = new Vector3(1.97f, transform.position.y, 20.3f);
        movePoint2 = new Vector3(1.94f, transform.position.y, 15.15f);
        movePoint3 = new Vector3(6.35f, transform.position.y, 9.48f);

        if (GameManager.instance.clientPlayer.curMainCharacter == 1)
        {
            // Krammen
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

        else if (GameManager.instance.clientPlayer.curMainCharacter == 2)
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
        StartCoroutine(Motion());
    }

    IEnumerator Motion()
    {
        transform.LookAt(movePoint1);
        while (Vector3.Distance(transform.position, movePoint1) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint1, 4f * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.1f);
        transform.LookAt(movePoint2);
        yield return new WaitForSeconds(0.1f);

        yield return new WaitForSeconds(0.8f);
        transform.LookAt(movePoint3);
        while (Vector3.Distance(transform.position, movePoint3) > 0.001f)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint3, 4f * Time.deltaTime);
            yield return null;
        }
        levelLoader.LoadNextLevel();
    }
}