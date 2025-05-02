using System.Collections.Generic;
using UnityEngine;

public class ShellManager : MonoBehaviour
{ 
    [Header("RIFERIMENTI")]
    
    public InventarioUI inventario;
    public Transform shellPosition;
    public Shell currentShell;
    public ShellPicker currentShellPicker;
    public Dictionary<Shell, ShellPicker> equippedShellPickers = new();

    private void Awake()
    {
        inventario = GameObject.FindAnyObjectByType<InventarioUI>();
    }

    // EQUIPAGGIA GUSCIO
    public void WearShell (Shell shell, ShellPicker shellPicker)
    {
        Debug.Log($"Hai equipaggiato il guscio {shell.name}");

        // Se hai già un guscio attivo, disattivalo
        if (currentShellPicker != null)
        {
            RemoveShell();
        }

        // Se è un nuovo guscio, aggiungilo alla lista
        if (!equippedShellPickers.ContainsKey(shell))
        {
            shellPicker.transform.SetParent(shellPosition);
            shellPicker.transform.localPosition = Vector3.zero;
            shellPicker.transform.localRotation = Quaternion.identity;
            shellPicker.GetComponent<Collider>().enabled = false;
            shellPicker.text.SetActive(false);

            equippedShellPickers[shell] = shellPicker;
        }

        // Aggiorna il riferimento corrente
        currentShell = shell;
        currentShellPicker = equippedShellPickers[shell];
        currentShellPicker.gameObject.SetActive(true);

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
            currentShell.PowerOff(Player.Instance);
            currentShellPicker.gameObject.SetActive(false);
            currentShellPicker = null;
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
}
