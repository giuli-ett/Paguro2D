using UnityEngine;

public class ShellPicker : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    public Shell shell;
    public GameObject text;

    [Header("PARAMETRI")]
    public bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // PRENDI IL GUSCIO
            Player.Instance.shellManager.WearShell(shell, this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            text.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            text.SetActive(false);
        }
    }
}
