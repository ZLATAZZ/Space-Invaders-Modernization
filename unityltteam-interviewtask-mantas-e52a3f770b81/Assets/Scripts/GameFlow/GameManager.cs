using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image _settings;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _effectsSlider;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private TextMeshProUGUI score;

    private void Start()
    {
        
    }

    

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        _settings.gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        _settings.gameObject.SetActive(false);
    }
    public void SetMusicVolume(float volume)
    {
        volume = _musicSlider.value;
        _audioMixer.SetFloat("Music", volume);
    }
    public void SetEffectsVolume(float volume)
    {
        volume = _effectsSlider.value;
        _audioMixer.SetFloat("Effects", volume);
    }
    public void Pause()
    {
        AudioManager.Instance.ActivateMenuMusic();
        Time.timeScale = 0.0f;
        Debug.Log(Time.timeScale);
    }
    public void Resume()
    {
        Time.timeScale = 1.0f;
    }

    public void SetScore()
    {
        score.text = $"Your score: {FindObjectOfType<GameplayUi>().Score}";
    }
}
