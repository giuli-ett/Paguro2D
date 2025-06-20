using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FineLivello : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Imposta il livello come completato
            GameManager.Instance.currentLivello.isCompleted = true;

            // Avvia la coroutine per l'animazione e il cambio scena
            StartCoroutine(FineLivelloCoroutine(other.gameObject));
        }
    }

    private IEnumerator FineLivelloCoroutine(GameObject player)
    {
        // Attiva l'animazione di vittoria
        Animator anim = player.GetComponent<Animator>();
        if (anim != null)
        {
            AudioManager.Instance.PlayVittoria();
            anim.SetBool("isWinning", true);
        }

        // Aspetta 1 secondo
        yield return new WaitForSeconds(0.8f);

        // Cambia scena
        SceneManager.LoadSceneAsync(4);
        Cursor.visible = true;
    }
}
