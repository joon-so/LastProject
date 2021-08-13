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
        // 현재 playableDirector에 등록되어있는 타임라인 실행
        playableDirector.Play();
    }

    public void PlayFormTimeLine()
    {
        // 새로운 타임라인 시작
        playableDirector.Play(timeLine);
    }
}
