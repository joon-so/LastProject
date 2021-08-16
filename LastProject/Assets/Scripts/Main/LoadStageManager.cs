using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadStageManager : MonoBehaviour
{
    public void Stage1Click()
    {
        GameManager.instance.ChangeSceneStage0();
    }
    public void Stage2Click()
    {
        GameManager.instance.ChangeSceneStage1To2();
    }
    public void Stage3Click()
    {
        GameManager.instance.ChangeSceneStage2To3();
    }
    public void Stage4Click()
    {
        GameManager.instance.ChangeSceneStage3To4();
    }
    public void Stage5Click()
    {
        GameManager.instance.ChangeSceneBoss1PageEnter();
    }
}
