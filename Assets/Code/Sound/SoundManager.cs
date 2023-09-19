using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : BaseSingleton<SoundManager>
{
    [SerializeField] private AudioSource _musicAudioSource;
    [SerializeField] private float _defaultMusicTransitionDuration;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixer _masterMixer;

    private Coroutine _musicLerp;
    private const float _musicLerpTick = .02f;
    private WaitForSeconds _delay;
    private List<AudioSource> _sfxAudioSources = new List<AudioSource>();
    private Queue<MusicAudioData> _musicQueue = new Queue<MusicAudioData>();

    #region "AudioDatas"
    [System.Serializable]
    public struct MusicAudioData
    {
        //use this if the music will transition to another and there is still a music playing
        //public bool TransitionCompletely;
        public AudioClip Clip;
        public float Volume;
        public float TransitionDuration;
        //public AudioSource AudioSource;

        public MusicAudioData(/*bool transitionCompletely,*/ AudioClip clip, float volume, /*AudioSource audioSource,*/ float transitionDuration = 0)
        {
            Contract.Ensures(volume >= 0 && volume <= 1);
            //TransitionCompletely = transitionCompletely;
            TransitionDuration = transitionDuration;
            Clip = clip;
            Volume = volume;
            //AudioSource = audioSource;
        }
    }

    [System.Serializable]
    public struct SfxAudioData
    {
        public AudioClip Clip;
        public float Volume;
        public bool IsSoundSpatial;
        [Min(0f)] public float MinSoundRange;
        [Min(0f)] public float MaxSoundRange;
        [Range(.1f, 3f)] public float[] RandomizePitch;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="clip"></param>
        /// <param name="volume"></param>
        /// <param name="isSoundSpatial"></param>
        /// <param name="randomizePitch">an array with the size of 2. 0 = min value, 1 = max value</param>
        /// <param name="audioSource"></param>
        public SfxAudioData(AudioClip clip, float volume, float minSoundRange, float maxSoundRange, bool isSoundSpatial = true, float[] randomizePitch = null)
        {
            //Contract.Ensures(volume >= 0 && volume <= 1);
            Contract.Ensures(randomizePitch.Length == 2);
            Clip = clip;
            Volume = volume;
            IsSoundSpatial = isSoundSpatial;
            RandomizePitch = randomizePitch;
            MinSoundRange = minSoundRange;
            MaxSoundRange = maxSoundRange;
        }
    }

    public enum SoundTypes
    {
        MUSIC,
        SFX
    }
    #endregion

#if UNITY_EDITOR
    [SerializeField] private bool _debugMessages;
#endif

    protected override void Awake()
    {
        base.Awake();
        _delay = new WaitForSeconds(_musicLerpTick);
    }
    #region "Sfx"
    /// <summary>
    /// 
    /// </summary>
    /// <param name="volume"></param>
    /// <param name="randomizePitch">an array with the size of 2. 0 = min value, 1 = max value </param>
    /// <param name="audioSource"></param>
    public void PlaySoundEffectInComponent(float volume, bool overrideCurrentSoundEffect, AudioClip clip, float[] randomizePitch = null, AudioSource audioSource = null)
    {
        Contract.Ensures(volume >= 0 && volume <= 1);
        Contract.Ensures(randomizePitch.Length == 2);
        if (!audioSource.isPlaying || overrideCurrentSoundEffect)
        {
            if (audioSource.isPlaying) audioSource.Stop();
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = randomizePitch.Length == 2 ? Random.Range(randomizePitch[0], randomizePitch[1]) : Random.Range(.1f, 3f);
            audioSource.Play();
        }
#if UNITY_EDITOR
        else
        {
            if (_debugMessages)
                Debug.Log($"there is already a sound playing in {audioSource.gameObject.name}");
        }
#endif
    }

    public void PlaySoundEffect(SfxAudioData data, Vector3 soundOrigin)
    {
        Contract.Ensures(data.Volume >= 0 && data.Volume <= 1);
        Contract.Ensures(data.RandomizePitch.Length == 2);

        AudioSource audioSource = GetAvailableSfxSoundPlayer();

        audioSource.clip = data.Clip;
        audioSource.volume = data.Volume;
        audioSource.spatialBlend = data.IsSoundSpatial ? 1 : 0;
        audioSource.maxDistance = data.MaxSoundRange;
        audioSource.minDistance = data.MinSoundRange;
        audioSource.pitch = data.RandomizePitch.Length == 2 ? Random.Range(data.RandomizePitch[0], data.RandomizePitch[1]) : Random.Range(-1f, 1f);
        audioSource.gameObject.transform.position = soundOrigin;
        audioSource.Play();
#if UNITY_EDITOR
        if (_debugMessages)
            Debug.Log($"playing the sound {audioSource.clip.name} at position {soundOrigin}");
#endif
    }

    private AudioSource GetAvailableSfxSoundPlayer()
    {
        for (int i = 0; i < _sfxAudioSources.Count; i++)
        {
            if (!_sfxAudioSources[i].isPlaying) return _sfxAudioSources[i];
        }

        GameObject temp = new GameObject("SfxSoundSource");
        AudioSource audioSource = temp.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = _sfxMixerGroup;
        temp.transform.SetParent(transform);
        _sfxAudioSources.Add(audioSource);
        return audioSource;
    }
    #endregion

    #region "Music"
    public void PlayMusic(MusicAudioData data)
    {
        _musicQueue.Enqueue(data);
        if (_musicLerp == null) _musicLerp = StartCoroutine(MusicSoundLerp(_musicQueue.Dequeue()));
    }

    private IEnumerator MusicSoundLerp(MusicAudioData data)
    {
#if UNITY_EDITOR
        if (_debugMessages)
            Debug.Log($"starting music blend from {_musicAudioSource.clip} to {data.Clip}");
#endif
        //bool operationCompleted = false;
        float duration = data.TransitionDuration > 0 ? data.TransitionDuration : _defaultMusicTransitionDuration;
        float delta = 0;
        float currentVolume = _musicAudioSource.volume;
        //float tempVolume = data.TransitionCompletely ? 0 : data.Volume;
        _musicAudioSource.clip = data.Clip;
        if (!_musicAudioSource.isPlaying && data.Volume > 0) _musicAudioSource.Play();
        while (delta < 1/*!operationCompleted*/)
        {
            _musicAudioSource.volume = Mathf.Lerp(currentVolume, data.Volume/*tempVolume*/, delta);
            delta += _musicLerpTick / duration;
            //if (delta >= 1)
            //{
            //    if (data.TransitionCompletely)
            //    {
            //        currentVolume = _musicAudioSource.volume;
            //        tempVolume = data.Volume;
            //        data.TransitionCompletely = false;
            //        delta = 0;
            //    }
            //    else
            //    {
            //        operationCompleted = true;
            //    }
            //    _musicAudioSource.clip = data.Clip;
            //    if (!_musicAudioSource.isPlaying) _musicAudioSource.Play();
            //}
            yield return _delay;
        }
        if (data.Volume <= 0) _musicAudioSource.Stop();
        _musicLerp = null;
#if UNITY_EDITOR
        if (_debugMessages)
            Debug.Log($"music blend endend, now playing {data.Clip}");
#endif
        if (_musicQueue.Count > 0)
        {
#if UNITY_EDITOR
            if (_debugMessages)
                Debug.Log($"there are {_musicQueue.Count} in request");
#endif
            PlayMusic(_musicQueue.Dequeue());
        }
    }
    #endregion

    #region "Configurations"
    public void UpdateMusicVolume()
    {
        _masterMixer.GetFloat("musicVolume", out float currentVolume);
        _masterMixer.SetFloat("musicVolume", currentVolume == 0 ? -80 : 0);
    }

    public void UpdateSfxVolume()
    {
        _masterMixer.GetFloat("sfxVolume", out float currentVolume);
        _masterMixer.SetFloat("sfxVolume", currentVolume == 0 ? -80 : 0);
    }
    #endregion
}
