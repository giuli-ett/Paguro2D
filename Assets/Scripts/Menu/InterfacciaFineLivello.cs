using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfacciaFineLivello : MonoBehaviour
{
    public void SceltaLivello()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void TornaMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
