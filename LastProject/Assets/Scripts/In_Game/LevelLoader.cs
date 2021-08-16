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
        StartCoroutine(BossPage2());
    }
    IEnumerator BossPage2()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("StageBoss2PageEnter");
    }

    public void LoadBossPage3()
    {
        StartCoroutine(BossPage3());
    }
    IEnumerator BossPage3()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("StageBoss3PageEnter");
    }

    public void LoadBossStage()
    {
        StartCoroutine(BossStage());
    }
    IEnumerator BossStage()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Stage5");
    }

    public void LoadBossEnding()
    {
        StartCoroutine(BossEnding());
    }
    IEnumerator BossEnding()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("StageBossEnding");
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

    public void LoadLogin()
    {
        StartCoroutine(Login());
    }
    IEnumerator Login()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene("Login");
    }
}