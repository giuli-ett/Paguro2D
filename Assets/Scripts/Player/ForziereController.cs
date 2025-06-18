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

}
