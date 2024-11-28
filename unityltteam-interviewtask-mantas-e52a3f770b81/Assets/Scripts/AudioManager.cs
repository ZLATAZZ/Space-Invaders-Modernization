using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[DefaultExecutionOrder(-1)]
public class AudioManager : MonoBehaviour
{
    [Header("AudioSources")]
    [SerializeField] private AudioSource _effectsSource;
    [SerializeField] private AudioSource _mainBackgroundMusicSource;
    [SerializeField] private AudioSource _menuBackgroundMusicSource;

    [Header("AudioClips")]
    [SerializeField] private AudioClip _blasterSound;
    [SerializeField] private AudioClip _explosionSound;
    [SerializeField] private AudioClip _powerUpSound;
    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private AudioClip _hitSound;
   
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBlasterSound()
    {
        _effectsSource.PlayOneShot(_explosionSound);
    }

    public void PlayExplosionSound()
    {
        _effectsSource.PlayOneShot(_explosionSound);
    }

    public void PlayPowerUpSound()
    {
        _effectsSource.PlayOneShot(_powerUpSound);
    }

    public void PlayGameOverSound()
    {
        _effectsSource.PlayOneShot(_gameOverSound);
    }

    public void PlayHitSound()
    {
        _effectsSource.PlayOneShot(_hitSound);
    }

    public void ActivateMainMusic()
    {
        _mainBackgroundMusicSource.gameObject.SetActive(true);
        _menuBackgroundMusicSource.gameObject.SetActive(false);
    }
    public void ActivateMenuMusic()
    {
        _mainBackgroundMusicSource.gameObject.SetActive(false);
        _menuBackgroundMusicSource.gameObject.SetActive(true);
    }
}
