using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TimeLineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public TimelineAsset timeLine;
    
    public void Play()
    {
        // ���� playableDirector�� ��ϵǾ��ִ� Ÿ�Ӷ��� ����
        playableDirector.Play();
    }

    public void PlayFormTimeLine()
    {
        // ���ο� Ÿ�Ӷ��� ����
        playableDirector.Play(timeLine);
    }
}
