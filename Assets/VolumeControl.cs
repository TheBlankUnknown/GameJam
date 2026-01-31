using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource[] sfxSources; // All your SFX sources

    [Header("UI Sliders")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("SFX Clips")]
    public AudioClip[] sfxClips; // Assign your sounds here
    public AudioSource sfxSourcet; // We'll play sounds through this

    private void Awake()
    {
        // Assign the first AudioSource from sfxSources or create one
        if (sfxSources.Length > 0)
            sfxSourcet = sfxSources[0];
    }
    private void Start()
    {
        // Load saved volumes
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);

        // Apply initial volume
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        // Add slider listeners
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public float GetMusicVolume()
    {
       return musicSource.volume; // Slider is 0-1
       
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value; // Slider is 0-1
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void SetSFXVolume(float value)
    {
        foreach (AudioSource sfx in sfxSources)
        {
            sfx.volume = value; // Set all SFX sources
        }
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
    public void PlaySFX(int clipIndex)
    {
        if (clipIndex < 0 || clipIndex >= sfxClips.Length) return;
        if(clipIndex == 4)
        {
            sfxSourcet.PlayOneShot(sfxClips[clipIndex], sfxSlider.value*10);
        }
        else if (clipIndex == 5)
        {
            sfxSourcet.PlayOneShot(sfxClips[clipIndex], sfxSlider.value * 5);
        }
        else if (clipIndex == 2)
        {
            sfxSourcet.PlayOneShot(sfxClips[clipIndex], sfxSlider.value * 5);
        }
        else
        {
            sfxSourcet.PlayOneShot(sfxClips[clipIndex], sfxSlider.value);
        }
        
    }
}