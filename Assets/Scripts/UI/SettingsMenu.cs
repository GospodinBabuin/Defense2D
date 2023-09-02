using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixerGroup Mixer;
    public GameObject masterSlider; 
    public GameObject musicSlider; 
    public GameObject effectsSlider; 
    public GameObject uiSlider;
     

    private void Start()
    {
        masterSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MasterVolume", 1);       
        musicSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("MusicVolume", 1);
        effectsSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("EffectsVolume", 1);
        uiSlider.GetComponent<Slider>().value = PlayerPrefs.GetFloat("UIVolume", 1);
        gameObject.SetActive(false);
    }

    public void ChangeMasterVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-80, 0, volume));
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
    public void ChangeMusicVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("MusicVolume", Mathf.Lerp(-80, 0, volume));
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
    public void ChangeEffectsVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("EffectsVolume", Mathf.Lerp(-80, 0, volume));
        PlayerPrefs.SetFloat("EffectsVolume", volume);
    }
    public void ChangeUIVolume(float volume)
    {
        Mixer.audioMixer.SetFloat("UIVolume", Mathf.Lerp(-80, 0, volume));
        PlayerPrefs.SetFloat("UIVolume", volume);
    }

    public void OpenSettingsMenu()
    {
        gameObject.SetActive(true);
    }
    public void CloseSettingsMenu()
    {
        gameObject.SetActive(false);
    }
}
