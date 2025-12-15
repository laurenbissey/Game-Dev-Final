using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Soundtracks")]
    [SerializeField] private List<AudioClip> levelSoundtracks;
    [SerializeField] private int level = 0;
    private AudioSource activeSoundtrack;

    [Header("Sound Effects")]
    [SerializeField] private int numOfSFXs = 5;
    private List<AudioSource> sfxAudioSources;

    // private float MusicVolumePercentage { get { return OptionsManager.instance.MusicVolumePercentage * OptionsManager.instance.MasterVolumePercentage; } }
    // private float SFXVolumePercentage { get { return OptionsManager.instance.SFXVolumePercentage * OptionsManager.instance.MasterVolumePercentage; } }
    // private float VoiceVolumePercentage { get { return OptionsManager.instance.VoiceVolumePercentage * OptionsManager.instance.MasterVolumePercentage; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        sfxAudioSources = new List<AudioSource>();
        for (int i = 0; i < numOfSFXs; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;

            sfxAudioSources.Add(audioSource);
        }

        activeSoundtrack = gameObject.AddComponent<AudioSource>();
        activeSoundtrack.volume = .125f;
    }

    private void Start()
    {
        if (level < levelSoundtracks.Count && level >= 0)
            activeSoundtrack.clip = levelSoundtracks[level];

        activeSoundtrack.Play();
        activeSoundtrack.ignoreListenerPause = true;
    }

    /// <summary>
    /// Call to add SFX to a pool of SFXs that are being played.
    /// </summary>
    /// <param name="clip">SFX clip that will be played.</param>
    public void PlaySFX(AudioClip clip, float volume)
    {
        if (clip == null) return;

        foreach (AudioSource audioSource in sfxAudioSources)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.clip = clip;
                // audioSource.volume = SFXVolumePercentage;
                audioSource.volume = volume;
                audioSource.Play();

                return;
            }
        }

        sfxAudioSources[0].clip = clip;
        // sfxAudioSources[0].volume = SFXVolumePercentage;
        sfxAudioSources[0].volume = volume;
        sfxAudioSources[0].Play();
    }

    private void SetSoundtrackVolume()
    {
        // activeSoundtrack.volume = MusicVolumePercentage;
    }
}