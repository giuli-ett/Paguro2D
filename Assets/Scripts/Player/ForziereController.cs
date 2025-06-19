using UnityEngine;
using UnityEngine.InputSystem;

public class ForziereController : MonoBehaviour
{
    public GameObject closeForziere = null;
    public void Interact(InputAction.CallbackContext context)
    {
        if (closeForziere == null)
        {
            return;
        }

        bool isClose = closeForziere.GetComponent<Forziere>().isPlayerClose;

        if (context.started && isClose)
        {
            closeForziere.GetComponent<Animator>().SetBool("canOpen", true);
            AudioManager.Instance.PlayAperturaCassa();
            closeForziere.GetComponent<Forziere>().text.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Forziere"))
        {
            AudioManager.Instance.PlayTrovaNuovoGuscio();
            Player.Instance.animator.SetBool("newShell", true);
        }
    }

    public void OnNewShellEnd()
    {
        Player.Instance.animator.SetBool("newShell", false);
    }
}
