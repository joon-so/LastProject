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

    public void LoadBossPage2()
    {
        StartCoroutine(BpssPage2());
    }
    IEnumerator BpssPage2()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("StageBoss2PageEnter");
    }

    public void LoadBossPage3()
    {
        StartCoroutine(BpssPage3());
    }
    IEnumerator BpssPage3()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("StageBoss3PageEnter");
    }

    public void LoadBossStage()
    {
        StartCoroutine(BpssStage());
    }
    IEnumerator BpssStage()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Stage5");
    }

    public void LoadMain()
    {
        StartCoroutine(Main());
    }
    IEnumerator Main()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Main");
    }
}