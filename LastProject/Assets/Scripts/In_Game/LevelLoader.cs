using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator animator;

    public float transitionTime = 1f;

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevle(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevle(int levelIndex)
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }

}
