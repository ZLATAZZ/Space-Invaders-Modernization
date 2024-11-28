using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image settings;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private AudioMixer audioMixer;
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        settings.gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.gameObject.SetActive(false);
    }
    public void SetMusicVolume(float volume)
    {
        volume = musicSlider.value;
        audioMixer.SetFloat("Music", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        volume = effectsSlider.value;
        audioMixer.SetFloat("Effects", volume);
    }
    public void Pause()
    {
        Time.timeScale = 0.0f;
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
    }
}
