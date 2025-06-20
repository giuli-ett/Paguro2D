using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShellManager : MonoBehaviour
{
    [Header("RIFERIMENTI")]

    public InventarioUI inventario;
    public Transform shellPosition;
    public Shell baseShell;
    public ShellPicker baseShellPicker;
    public Shell currentShell;
    public ShellPicker currentShellPicker;
    public ShellPicker closeShell;
    public Dictionary<Shell, ShellPicker> equippedShellPickers = new();
    public bool siStaCambiando = false;

    private void Awake()
    {
        inventario = GameObject.FindAnyObjectByType<InventarioUI>();
        equippedShellPickers.Add(baseShell, baseShellPicker);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Tasto E premuto");
            if (closeShell != null)
            {
                Debug.Log("Sto tentando di indossare il guscio!");
                WearShell(closeShell.shell, closeShell);
            }
        }
        else
        {
            Debug.Log("Attendo input");
        }
    }

    // EQUIPAGGIA GUSCIO
    public void WearShell(Shell shell, ShellPicker shellPicker)
    {
        Debug.Log($"Hai equipaggiato il guscio {shell.name}");
        Player.Instance.animator.SetBool("isChange", true);
        // Se hai già un guscio attivo, disattivalo
        if (currentShellPicker != null)
        {
            RemoveShell();
        }

        // Se è un nuovo guscio, aggiungilo alla lista
        if (!equippedShellPickers.ContainsKey(shell))
        {
            shellPicker.transform.position = shellPosition.position; // Prima sposta
            shellPicker.transform.rotation = shellPosition.rotation;
            shellPicker.transform.SetParent(shellPosition);          // Poi setta il genitore
            Player.Instance.spriteRendererShell = shellPicker.GetComponent<SpriteRenderer>();

            var rb = shellPicker.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            shellPicker.GetComponent<Collider2D>().enabled = false;
            shellPicker.text.SetActive(false);

            equippedShellPickers[shell] = shellPicker;
        }

        // Aggiorna il riferimento corrente
        currentShell = shell;
        currentShellPicker = equippedShellPickers[shell];

        /*
        for (int i = 0; i < 99999999999; i++)
        {
            if (siStaCambiando)
            {
                break;
            }
            else
            {
                Debug.Log("Mi sto cambiando");
            }
            
        }
        */
        
        currentShellPicker.gameObject.SetActive(true);
        Player.Instance.spriteRendererShell = shellPicker.GetComponent<SpriteRenderer>();

        // Attiva il potere del guscio
        shell.PowerOn(Player.Instance);

        if (!inventario.shellList.Contains(shell))
        {
            inventario.AggiungiGuscio(shell);
        }
    }

    // RIMUOVI GUSCIO
    public void RemoveShell()
    {
        if (currentShellPicker != null)
        {
            Player.Instance.animator.SetBool("isChange", true);
            currentShell.PowerOff(Player.Instance);
            currentShellPicker.gameObject.SetActive(false);
            currentShellPicker = null;
            Player.Instance.spriteRendererShell = null;
        }

        currentShell = null;
    }

    // TROVA GUSCIO
    public ShellPicker GetShellPickerByShell(Shell shell)
    {
        if (equippedShellPickers.ContainsKey(shell))
        {
            return equippedShellPickers[shell];
        }
        return null;
    }

    public void OnChangeOver()
    {
        Player.Instance.animator.SetBool("isChange", false);
        siStaCambiando = false;
    }

    public void OnChangeStart()
    {
        siStaCambiando = true;
    }
}
