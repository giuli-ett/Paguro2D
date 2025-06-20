using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class ShellManager : MonoBehaviour
{
    [Header("RIFERIMENTI")]

    public InventarioUI inventario;
    public UIController uIController;
    public FeedbackTartaruga feedbackTartaruga;
    public Transform shellPosition;
    public Shell baseShell;
    public ShellPicker baseShellPicker;
    public Shell currentShell;
    public ShellPicker currentShellPicker;
    public ShellPicker closeShell;
    public Dictionary<Shell, ShellPicker> equippedShellPickers = new();
    public CanvasGroup doppioSaltoCanvas;
    public CanvasGroup luminescenzaCanvas;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        inventario = GameObject.FindAnyObjectByType<InventarioUI>();
        equippedShellPickers.Add(baseShell, baseShellPicker);
    }

    /*
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
    */

    // EQUIPAGGIA GUSCIO
    public void WearShell(Shell shell, ShellPicker shellPicker)
    {
        Debug.Log($"Cambio guscio con {shellPicker} , {shell}");
        Player.Instance.animator.SetBool("isChange", true);

        // Se hai già un guscio attivo, disattivalo
        if (currentShellPicker != null)
        {
            RemoveShell();
        }

        // Se è un nuovo guscio, aggiungilo alla lista
        if (!equippedShellPickers.ContainsKey(shell))
        {
            Debug.Log("Nuovo guscio, lo aggiungo.");
            shellPicker.transform.position = shellPosition.position;
            shellPicker.transform.rotation = shellPosition.rotation;
            shellPicker.transform.SetParent(shellPosition);
            Player.Instance.spriteRendererShell = shellPicker.GetComponent<SpriteRenderer>();

            var rb = shellPicker.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            shellPicker.GetComponent<Collider2D>().enabled = false;
            shellPicker.text.SetActive(false);

            equippedShellPickers[shell] = shellPicker;
            feedbackTartaruga.StartSwimForShellFeedback();
        }

        currentShell = shell;
        currentShellPicker = equippedShellPickers[shell];

        Player.Instance.spriteRendererShell = shellPicker.GetComponent<SpriteRenderer>();

        shell.PowerOn(Player.Instance);

        if (!inventario.shellList.Contains(shell))
        {
            inventario.AggiungiGuscio(shell, currentShellPicker);
        }

        Player.Instance.spriteRendererShell.flipX = Player.Instance.spriteRenderer.flipX;
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
        if (currentShellPicker != null)
        {
            currentShellPicker.gameObject.SetActive(true);
        }
    }

    public CanvasGroup ShowShellUI(Shell shell)
    {
        Dictionary<Shell, CanvasGroup> lista = Player.Instance.GetComponent<CanvasGroups>().listaFeedbackGusci;
        CanvasGroup canvas = null;
        foreach (KeyValuePair<Shell, CanvasGroup> pair in lista)
        {
            if (string.Equals(pair.Key.shellName, shell.shellName, System.StringComparison.OrdinalIgnoreCase))
            {
                canvas = pair.Value;
            }
        }

        if (canvas != null)
        {
            return canvas;
        }
        else
        {
            Debug.Log("Nessun feedback associato al guscio");
            return null;
        }
    }
}
