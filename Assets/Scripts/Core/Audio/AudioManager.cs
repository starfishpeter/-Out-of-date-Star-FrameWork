using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AudioManager : BaseManager<AudioManager>
{
    private AudioSource BGM = null;
    private float BGMVolume = 1;

    private float SoundVolume = 1;
    private List<GameObject> soundList = new List<GameObject>();

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="name">背景音乐名</param>
    public void BGMOn(string name)
    {
        if(BGM == null)
        {
            GameObject obj = new GameObject{ name = "BGM" };
            BGM = obj.AddComponent<AudioSource>();
            ResourceManager.GetInstance().LoadAsync<AudioClip>("Music/BGM/" + name, (clip) =>
            {
                BGM.clip = clip;
                BGM.loop = true;
                BGM.volume = BGMVolume;
                BGM.Play();
            });
        }
    }

    /// <summary>
    /// 改变背景音乐音量
    /// </summary>
    /// <param name="value">音量大小 在0~1之间</param>
    public void ChangeBGMVolume(float value)
    {
        if (value >= 0 && value <= 1) 
        {
            BGMVolume = value;
        }
        
        if(BGM == null)
        {
            return;
        }

        BGM.volume = BGMVolume;
    }

    /// <summary>
    /// 停止播放BGM
    /// </summary>
    public void BGMOff()
    {
        if(BGM == null)
        {
            return;
        }
        BGM.Stop();
    }

    /// <summary>
    /// 暂停BGM
    /// </summary>
    public void BGMPause()
    {
        if (BGM == null)
        {
            return;
        }
        BGM.Pause();
    }

    /// <summary>
    /// 音效播放
    /// </summary>
    /// <param name="name">音效名</param>
    /// <param name="callBack">返回音效 用于停止播放</param>
    public void SoundPlay(string name, bool isLoop, UnityAction<GameObject> callBack = null)
    {
        ResourceManager.GetInstance().LoadAsync<AudioClip>("Music/Sound/" + name, (clip) =>
        {
            PoolManager.GetInstance().GetObj("Music/SoundPlay", (obj) =>
            {
                obj.name = "Music/SoundPlay";

                AudioSource source = obj.GetComponent<AudioSource>();
                source.clip = clip;

                if(!source.isPlaying)
                {
                    source.Play();
                }

                source.volume = SoundVolume;

                if (isLoop)
                {
                    source.loop = true;
                }

                if (callBack != null)
                {
                    callBack(obj);
                }

                MonoManager.GetInstance().StartCoroutine(source.GetComponent<SoundRecycle>().Recycle());

                soundList.Add(obj);
            });
        });
    }

    /// <summary>
    /// 改变音效音量大小
    /// </summary>
    /// <param name="value"></param>
    public void ChangeSoundVolume(float value)
    {
        if(value >= 0 && value <= 1)
        {
            SoundVolume = value;
        }

        for (int i = 0; i < soundList.Count; i++)
        {
            soundList[i].GetComponent<AudioSource>().volume = SoundVolume;
        } 
        
    }

    /// <summary>
    /// 关闭音效
    /// </summary>
    /// <param name="Sound"></param>
    public void SoundOff(GameObject Sound)
    {
        if (soundList.Contains(Sound))
        {
            PoolManager.GetInstance().PushObj(Sound.name, Sound);
            soundList.Remove(Sound);
        }
    }
}
