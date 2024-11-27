using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUi : MonoBehaviour {

    public void Open() {
        gameObject.SetActive(true);
    }

    public void Close() {
        gameObject.SetActive(false);
    }

    public void OnRetryButton() {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
