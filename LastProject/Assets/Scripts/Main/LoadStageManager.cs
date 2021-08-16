using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStageManager : MonoBehaviour
{
    [SerializeField] AudioClip uiButtonSound;
    public void Stage1Click()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("Stage1", 1f);
    }
    void Stage1()
    {
        GameManager.instance.ChangeSceneStage0();
    }

    public void Stage2Click()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("Stage2", 1f);
    }
    void Stage2()
    {
        GameManager.instance.ChangeSceneStage0();
    }
    public void Stage3Click()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("Stage3", 1f);
    }
    void Stage3()
    {
        GameManager.instance.ChangeSceneStage0();
    }
    public void Stage4Click()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("Stage4", 1f);
    }
    void Stage4()
    {
        GameManager.instance.ChangeSceneStage0();
    }
    public void Stage5Click()
    {
        SoundManager.instance.SFXPlay("UIButtonClik", uiButtonSound);
        Invoke("Stage5", 1f);
    }
    void Stage5()
    {
        GameManager.instance.ChangeSceneStage0();
    }
}