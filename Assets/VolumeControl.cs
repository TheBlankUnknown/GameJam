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
}