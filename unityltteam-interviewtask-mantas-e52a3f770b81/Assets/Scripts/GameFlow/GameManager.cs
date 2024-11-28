using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Image settings;
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
    public void Pause()
    {

    }
    public void Resume()
    {

    }
}
