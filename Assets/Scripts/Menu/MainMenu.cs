using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        MusicPlayer.Instance.PlayMenuMusic();
    }
    public void PlayGame()
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(3);
    }

    public void ContinueGame()
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlayClick();
        Application.Quit();
    }
}
