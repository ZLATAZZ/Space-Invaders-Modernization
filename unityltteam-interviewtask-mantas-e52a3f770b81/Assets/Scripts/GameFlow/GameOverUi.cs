using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUi : MonoBehaviour {
    private AudioManager _audioManagerInstance = AudioManager.Instance;

    public void Open() {
        gameObject.SetActive(true);
        FindObjectOfType<GameManager>().SetScore();
        FindObjectOfType<GameplayUi>().SetBestScore();
        Time.timeScale = 0;
        _audioManagerInstance.PlayGameOverSound();
        _audioManagerInstance.ActivateMenuMusic();
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void OnRetryButton() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
