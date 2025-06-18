using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
        // DA METTERE LA CUTSCENE INIZIALE
    }

    /*
    public void ContinueGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
    */

    public void ExitGame()
    {
        Application.Quit();
    }
}
