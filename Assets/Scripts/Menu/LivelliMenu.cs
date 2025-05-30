using UnityEngine;
using UnityEngine.SceneManagement;

public class LivelliMenu : MonoBehaviour
{
    public void PlayLivelloUno()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void PlayLivelloDue()
    {
        //SceneManager.LoadSceneAsync(3);
    }
    public void PlayLivelloTre()
    {
        //SceneManager.LoadSceneAsync(4);
    }
    public void PlayLivelloQuattro()
    {
        //SceneManager.LoadSceneAsync(5);
    }
    public void SceltaLivello()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void TornaMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
