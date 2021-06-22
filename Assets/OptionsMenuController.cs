using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;
    private float _multiplier = 30;
    [SerializeField] Slider _sliderMaster;
    [SerializeField] Slider _sliderMusic;
    [SerializeField] Slider _sliderSFX;

    float _master = 1f;
    float _music = 1f;
    float _sfx = 1f;

    private void Start()
    {
        _sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", _sliderMaster.value);
        _sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", _sliderMusic.value);
        _sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", _sliderSFX.value);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MasterVolume", _master);
        PlayerPrefs.SetFloat("MusicVolume", _music);
        PlayerPrefs.SetFloat("SFXVolume", _sfx);
    }

    public void SetMaster(float volume)
    {
        _master = volume;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * _multiplier);
    }

    public void SetMusic(float volume)
    {
        _music = volume;
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * _multiplier);
    }

    public void SetSFX(float volume)
    {
        _sfx = volume;
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * _multiplier);
    }

    /*public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }*/

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

}
