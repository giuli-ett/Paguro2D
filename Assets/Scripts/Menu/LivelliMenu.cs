using UnityEngine;
using UnityEngine.SceneManagement;

public class LivelliMenu : MonoBehaviour
{
    public void PlayLivelloUno()
    {
        //GameManager.Instance.SetCurrentLevel(0);
        SceneManager.LoadSceneAsync(2);
    }
    public void PlayLivelloDue()
    {
        /*
        if (GameManager.Instance.currentLivello.isCompleted)
        {
            GameManager.Instance.SetCurrentLevel(1);
            SceneManager.LoadSceneAsync(3);
        }
        else
        {
            Debug.Log($"Devi prima completare {GameManager.Instance.currentLivello.nomeLivello}");
        }
        */

        GameManager.Instance.SetCurrentLevel(1);
        SceneManager.LoadSceneAsync(3);
    }
    public void PlayLivelloTre()
    {
        /*
        if (GameManager.Instance.currentLivello.isCompleted)
        {
            GameManager.Instance.SetCurrentLevel(2);
            SceneManager.LoadSceneAsync(4);
        }
        else
        {
            Debug.Log($"Devi prima completare {GameManager.Instance.currentLivello.nomeLivello}");
        }
        */

        GameManager.Instance.SetCurrentLevel(2);
        SceneManager.LoadSceneAsync(4);
            
    }
    public void PlayLivelloQuattro()
    {
        /*
        if (GameManager.Instance.currentLivello.isCompleted)
        {
            GameManager.Instance.SetCurrentLevel(3);
            //SceneManager.LoadSceneAsync(5);
        }
        else
        {
            Debug.Log($"Devi prima completare {GameManager.Instance.currentLivello.nomeLivello}");
        }
        */

        GameManager.Instance.SetCurrentLevel(3);
        SceneManager.LoadSceneAsync(5);
            
    }
    
}
