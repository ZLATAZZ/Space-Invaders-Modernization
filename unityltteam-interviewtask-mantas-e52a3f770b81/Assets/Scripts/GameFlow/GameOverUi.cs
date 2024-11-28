using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUi : MonoBehaviour {
    private AudioManager _audioManagerInstance = AudioManager.Instance;

    public void Open() {
        gameObject.SetActive(true);
        Time.timeScale = 0;
        _audioManagerInstance.PlayGameOverSound();
        _audioManagerInstance.ActivateMenuMusic();
    }

    public void Close() {
        Time.timeScale = 1;
        gameObject.SetActive(false);
        _audioManagerInstance.ActivateMainMusic();
    }

    public void OnRetryButton() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
