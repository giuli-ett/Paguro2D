using UnityEngine;
using UnityEngine.InputSystem;

public class ForziereController : MonoBehaviour
{
    public GameObject closeForziere = null;
    public void Interact(InputAction.CallbackContext context)
    {
        Debug.Log($"[DEBUG] Interact chiamato. context.phase = {context.phase}");
        Debug.Log($"closeForziere = {closeForziere}");

        if (closeForziere == null)
        {
            Debug.LogWarning("closeForziere è NULL!");
            return;
        }

        bool isClose = closeForziere.GetComponent<Forziere>().isPlayerClose;
        Debug.Log($"isPlayerClose: {isClose}");

        if (context.started && isClose)
        {
            Debug.Log("Setto canOpen = true");
            closeForziere.GetComponent<Animator>().SetBool("canOpen", true);
            closeForziere.GetComponent<Forziere>().text.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Forziere"))
        {
            Player.Instance.animator.SetBool("newShell", true);
        }
    }

    public void OnNewShellEnd()
    {
        Player.Instance.animator.SetBool("newShell", false);
    }
}
