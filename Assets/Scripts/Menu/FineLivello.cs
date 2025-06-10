using UnityEngine;
using UnityEngine.SceneManagement;

public class FineLivello : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.currentLivello.isCompleted = true;
            SceneManager.LoadSceneAsync(5);
        }
    }
}
