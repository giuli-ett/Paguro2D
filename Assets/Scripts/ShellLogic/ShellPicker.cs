using UnityEngine;
using UnityEngine.InputSystem;

public class ShellPicker : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    public Shell shell;
    public GameObject text;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Sei vicino al guscio");
            text.SetActive(true);
            Player.Instance.shellManager.closeShell = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            text.SetActive(false);
            Player.Instance.shellManager.closeShell = null;
        }
    }
}
