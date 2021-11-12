using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    #region 单例
    private static MusicManager instance = null;
    public static MusicManager Instance
    {
        get
        {
            GameObject musicMgr = GameObject.Find("MusicManager");
            if (musicMgr == null)
            {
                musicMgr = new GameObject("MusicManager");
            }
            if (instance == null)
            {
                instance = musicMgr.GetComponent<MusicManager>();
                if (instance == null)
                {
                    instance = musicMgr.AddComponent<MusicManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    #region 参数
    private AudioSource musicAudioSource;//背景音乐的AudioSource

    private List<AudioSource> unusedSoundAudioSourceList;   // 存放可以使用的音频组件

    private List<AudioSource> usedSoundAudioSourceList;     // 存放正在使用的音频组件

    private Dictionary<string, AudioClip> audioClipDict;       // 缓存音频文件

    private float musicVolume = 1; //背景音乐声音

    private float soundVolume = 1; //音效声音

    public string musicVolumePrefs = "MusicVolume";//本地缓存背景音乐的键

    public string soundVolumePrefs = "SoundVolume";//本地缓存音效的键

    private int poolCount = 5;         // AudioSource对象池数量

    #endregion

    void Awake()
    {
        //初始化
        musicAudioSource = gameObject.AddComponent<AudioSource>();
        unusedSoundAudioSourceList = new List<AudioSource>();
        usedSoundAudioSourceList = new List<AudioSource>();
        audioClipDict = new Dictionary<string, AudioClip>();
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() { }

    public void Init()
    {
        // 从本地缓存读取声音音量
        if (PlayerPrefs.HasKey(musicVolumePrefs))
        {
            musicVolume = PlayerPrefs.GetFloat(musicVolumePrefs, 1);
        }
        if (PlayerPrefs.HasKey(soundVolumePrefs))
        {
            soundVolume = PlayerPrefs.GetFloat(soundVolumePrefs, 1);
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="isLoop">是否循环</param>
    public void PlayMusic(string path, bool isLoop = true)
    {
        //TODO背景音乐的淡入淡出用DOTWEEN
        musicAudioSource.clip = GetAudioClip(path);
        musicAudioSource.clip.LoadAudioData();
        musicAudioSource.loop = isLoop;
        musicAudioSource.volume = musicVolume;
        musicAudioSource.Play();
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="path">音效路径</param>
    /// <param name="func">回调</param>
    public void PlaySound(string path)
    {
        AudioSource audioSource = null;
        if (unusedSoundAudioSourceList.Count != 0)
        {
            audioSource = UnusedToUsed();
        }
        else
        {
            AddAudioSource();
            audioSource = UnusedToUsed();
        }
        audioSource.clip = GetAudioClip(path);
        audioSource.clip.LoadAudioData();
        audioSource.volume = soundVolume;
        audioSource.loop = false;
        audioSource.Play();
        StartCoroutine(WaitPlayEnd(audioSource));
    }

    /// <summary>
    /// 设置音量
    /// </summary>
    /// <param name="music">背景音乐的音量</param>
    /// <param name="sound">音效的音量</param>
    public void SetVolume(float music, float sound)
    {
        ChangeMusicVolume(music);
        ChangeSoundVolume(sound);
    }

    /// <summary>
    /// 得到背景音乐音量
    /// </summary>
    /// <returns></returns>
    public float GetMusicVolume()
    {
        if (!PlayerPrefs.HasKey(musicVolumePrefs)) return 1;
        return PlayerPrefs.GetFloat(musicVolumePrefs);
    }

    /// <summary>
    /// 得到音效音量
    /// </summary>
    /// <returns></returns>
    public float GetSoundVolume()
    {
        if (!PlayerPrefs.HasKey(soundVolumePrefs)) return 1;
        return PlayerPrefs.GetFloat(soundVolumePrefs);
    }

    /// <summary>
    /// 获取音频文件，获取后会缓存一份
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private AudioClip GetAudioClip(string path)
    {
        if (audioClipDict.ContainsKey(path)==false)
        {
            AudioClip ac = Resources.Load(path) as AudioClip;
            if (ac == null)
            {
                Debug.LogError("音频在Resource中不存在,请检查音频资源--path:" + path);
                return null;
            }
            audioClipDict.Add(path, ac);
        }
        return audioClipDict[path];
    }

    /// <summary>
    /// 添加音频组件
    /// </summary>
    /// <returns></returns>
    private AudioSource AddAudioSource()
    {
        if (unusedSoundAudioSourceList.Count != 0)
        {
            return UnusedToUsed();
        }
        else
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            unusedSoundAudioSourceList.Add(audioSource);
            return audioSource;
        }
    }

    /// <summary>
    /// 将未使用的音频组件移至已使用集合里
    /// </summary>
    /// <returns></returns>
    private AudioSource UnusedToUsed()
    {
        AudioSource audioSource = unusedSoundAudioSourceList[0];
        unusedSoundAudioSourceList.RemoveAt(0);
        usedSoundAudioSourceList.Add(audioSource);
        return audioSource;
    }

    /// <summary>
    /// 当播放音效结束后，将其移至未使用集合
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    IEnumerator WaitPlayEnd(AudioSource audioSource)
    {
        yield return new WaitUntil(() => { return !audioSource.isPlaying; });
        UsedToUnused(audioSource);
    }

    /// <summary>
    /// 将使用完的音频组件移至未使用集合里
    /// </summary>
    /// <param name="audioSource"></param>
    private void UsedToUnused(AudioSource audioSource)
    {
        if (usedSoundAudioSourceList.Contains(audioSource))
        {
            usedSoundAudioSourceList.Remove(audioSource);
        }
        if (unusedSoundAudioSourceList.Count >= poolCount)
        {
            Destroy(audioSource);
        }
        else if (audioSource != null && !unusedSoundAudioSourceList.Contains(audioSource))
        {
            unusedSoundAudioSourceList.Add(audioSource);
        }
    }

    /// <summary>
    /// 修改背景音乐音量
    /// </summary>
    /// <param name="volume"></param>
    private void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = volume;
        PlayerPrefs.SetFloat(musicVolumePrefs, volume);
    }

    /// <summary>
    /// 修改音效音量
    /// </summary>
    /// <param name="volume"></param>
    private void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;
        for (int i = 0; i < unusedSoundAudioSourceList.Count; i++)
        {
            unusedSoundAudioSourceList[i].volume = volume;
        }
        for (int i = 0; i < usedSoundAudioSourceList.Count; i++)
        {
            usedSoundAudioSourceList[i].volume = volume;
        }
        PlayerPrefs.SetFloat(soundVolumePrefs, volume);
    }
}