using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public bool isPaused = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }
    public void Pausa()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        Player.Instance.canMove = false;
        gameObject.SetActive(true);
        isPaused = true;
    }
    public void Continua()
    {
        AudioManager.Instance.PlayClick();
        Cursor.visible = false;
        Time.timeScale = 1f;
        Player.Instance.canMove = true;
        gameObject.SetActive(false);
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayClick();
        gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(0);
    }
}
