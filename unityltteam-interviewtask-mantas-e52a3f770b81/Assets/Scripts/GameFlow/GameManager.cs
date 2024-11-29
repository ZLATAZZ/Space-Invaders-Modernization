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
        SetBestScore();
    }

    private void Update()
    {
        ChangeMusicSliderLook();
        ChangeEffectsSliderLook();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenSettings()
    {
        SaveDataManager.Instance.LoadData();
        _settings.gameObject.SetActive(true);
    }

    public void CloseSettings()
    {
        SaveDataManager.Instance.SaveData();
        _settings.gameObject.SetActive(false);
    }
    public void SetMusicVolume(float volume)
    {
        SaveDataManager.Instance.musicVolume = volume;
        ChangeMusicSliderLook();
    }
    public void SetEffectsVolume(float volume)
    {
        SaveDataManager.Instance.effectsVolume = volume;
        ChangeEffectsSliderLook();
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
    public void BackToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        SaveDataManager.Instance.SaveData();
    }
    public void CloseApp()
    {
        Application.Quit();
    }

    //set score for gameover
    public void SetScore()
    {
        score.text = $"Your score: {FindObjectOfType<GameplayUi>().Score}";
    }
    //set score in main menu
    private void SetBestScore()
    {
        SaveDataManager.Instance.LoadData();
        score.text = $"Your best score: {SaveDataManager.Instance.score}";
    }

    #region methods for visual part (slidera)
    private void ChangeMusicSliderLook()
    {
        _musicSlider.value = SaveDataManager.Instance.musicVolume;
        _audioMixer.SetFloat("Music", SaveDataManager.Instance.musicVolume);
    }
    private void ChangeEffectsSliderLook()
    {
        _effectsSlider.value = SaveDataManager.Instance.effectsVolume;
        _audioMixer.SetFloat("Effects", SaveDataManager.Instance.effectsVolume);
    }
    #endregion

}
