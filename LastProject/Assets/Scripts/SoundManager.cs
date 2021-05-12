using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgSound;
    public AudioClip[] bgList;
    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
            Destroy(gameObject);
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        for (int i = 0; i < bgList.Length; i++)
        {
            if (arg0.name == bgList[i].name)
            {
                BgSoundPlay(bgList[i]);
            }
        }
    }

    public void SFXPlay(string sfxName, AudioClip clip)
    {
        GameObject obj = new GameObject(sfxName + "Sound");
        AudioSource audioSource = obj.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.Play();

        Destroy(obj, clip.length);
    }

    public void BgSoundPlay(AudioClip clip)
    {
        bgSound.clip = clip;
        bgSound.loop = true;
        bgSound.volume = 0.1f;
        bgSound.Play();
    }

    //public AudioSource[] audioSourceEffects;
    //public AudioSource audioSourceBgm;
    
    //public string[] playSoundName;

    //public Sound[] effectSounds;
    //public Sound[] bgmSound;

    //void Awake()
    //{
    //    if (instance == null)
    //        instance = this;
    //    else
    //        Destroy(this.gameObject);
    //}

    //void Start()
    //{
    //    playSoundName = new string[audioSourceEffects.Length];    
    //}

    //public void PlaySoundEffect(string name)
    //{
    //    for (int i = 0; i < effectSounds.Length; i++)
    //    {
    //        if(name == effectSounds[i].name)
    //        {
    //            for (int j = 0; j < audioSourceEffects.Length; j++)
    //            {
    //                if (!audioSourceEffects[j].isPlaying)
    //                {
    //                    playSoundName[j] = effectSounds[i].name;
    //                    audioSourceEffects[j].clip = effectSounds[i].clip;
    //                    audioSourceEffects[j].PlayOneShot(audioSourceEffects[j].clip);
    //                    return;
    //                }
    //            }
    //            Debug.Log("모든 가용 AudioSource가 사용중입니다.");
    //            return;
    //        }
    //        Debug.Log(name + "사운드가 SoundManager에 등록되지 않았습니다.");
    //    }
    //}
    //public void StopAllSoundEffect()
    //{
    //    for (int i = 0; i < audioSourceEffects.Length; i++)
    //    {
    //        audioSourceEffects[i].Stop();
    //    }
    //}

    //public void StopSoundEffect(string name)
    //{
    //    for (int i = 0; i < audioSourceEffects.Length; i++)
    //    {
    //        if (playSoundName[i] == name)
    //        {
    //            audioSourceEffects[i].Stop();
    //            return;
    //        }
    //    }
    //    Debug.Log("재생 중인" + name + "사운드가 없습니다.");
    //}
}
