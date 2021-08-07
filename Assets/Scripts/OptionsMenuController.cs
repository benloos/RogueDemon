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
    [SerializeField] Slider _sliderSensitivity;
    [SerializeField] Text _sensText;

    float _master = 1f;
    float _music = 1f;
    float _sfx = 1f;
    float _sens = 100f;

    private void Start()
    {
        _sliderMaster.value = PlayerPrefs.GetFloat("MasterVolume", _sliderMaster.value);
        _sliderMusic.value = PlayerPrefs.GetFloat("MusicVolume", _sliderMusic.value);
        _sliderSFX.value = PlayerPrefs.GetFloat("SFXVolume", _sliderSFX.value);
        _sens = PlayerPrefs.GetFloat("MouseSensitivity", _sliderSensitivity.value);
        _sliderSensitivity.value = PlayerPrefs.GetFloat("MouseSensitivity", _sliderSensitivity.value);
        _sensText.text = Mathf.Round(PlayerPrefs.GetFloat("MouseSensitivity", _sliderSensitivity.value)).ToString();
        _sensText.transform.position = _sliderSensitivity.handleRect.position;
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat("MasterVolume", _master);
        PlayerPrefs.SetFloat("MusicVolume", _music);
        PlayerPrefs.SetFloat("SFXVolume", _sfx);
        PlayerPrefs.SetFloat("MouseSensitivity", _sens);
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

    public void SetSensitiviy(float value)
    {
        _sens = value;
        _sensText.text = Mathf.Round(value).ToString();
        _sensText.transform.position =_sliderSensitivity.handleRect.position;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

}
