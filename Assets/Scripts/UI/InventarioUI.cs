using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InventarioUI : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    public GameObject panelInventario;
    public PlayerInput input;
    public List<Shell> shellList;
    public ShellPicker shellPrefab;
    public Shell baseShell;

    [Header("SLOT NAVIGATION")]
    public List<Slot> shellSlots;
    public int selectedSlot = 0;

    private void Start()
    {
        panelInventario.SetActive(false);
        shellList = new List<Shell>();
        AggiornaInventarioUI();
    }

    private void Update()
    {
        if (panelInventario.activeSelf)
        {
            for (int i = 0; i < shellSlots.Count; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SelectSlotByNumber(i);
                }
            }
        }
    }

    private void SelectSlotByNumber(int index)
    {
        if (index >= 0 && index < shellSlots.Count)
        {
            shellSlots[selectedSlot].DeselectSlot();
            selectedSlot = index;
            HighlightSlot(selectedSlot);
        }
    }

    public void MostraInventario(InputAction.CallbackContext context)
    {
        if (FeedbackTartaruga.Instance != null)
        {
            if (FeedbackTartaruga.Instance.tutorialInCorso)
            {
                // Se siamo nel tutorial, permetti apertura solo alla prima frase
                if (!panelInventario.activeSelf && FeedbackTartaruga.Instance.IsOnFirstFrase())
                {
                    // consenti apertura
                }
                else
                {
                    return; // blocca apertura o chiusura durante il resto del tutorial
                }
            }
        }

        if (!Player.Instance.isGrounded)
            return;

        if (context.started)
        {
            AudioManager.Instance.PlayClick();
            bool isActive = !panelInventario.activeSelf;
            panelInventario.SetActive(isActive);
            Player.Instance.canMove = !isActive;

            if (isActive)
            {
                AggiornaInventarioUI();
                HighlightSlot(selectedSlot);
            }
            else
            {
                if (selectedSlot >= 0 && selectedSlot < shellList.Count)
                {
                    Shell selezionato = shellList[selectedSlot];
                    if (Player.Instance.shellManager.currentShell != selezionato)
                    {
                        Player.Instance.animator.SetBool("isChange", true);
                        Player.Instance.shellManager.RemoveShell();
                        var shellPicker = Player.Instance.shellManager.GetShellPickerByShell(selezionato);
                        Player.Instance.shellManager.WearShell(selezionato, shellPicker);
                    }
                }
            }
        }
    }

    public void Naviga(InputAction.CallbackContext context)
    {
        if (!panelInventario.activeSelf)
            return;

        Vector2 navigation = context.ReadValue<Vector2>();
        AudioManager.Instance.PlayNavigateInventory();

        if (navigation.x > 0.5f)
        {
            MoveSelection(1);
        }
        else if (navigation.x < -0.5f)
        {
            MoveSelection(-1);
        }
    }

    private void MoveSelection(int direction)
    {
        foreach (var slot in shellSlots)
        {
            slot.DeselectSlot();
        }

        selectedSlot += direction;

        if (selectedSlot < 0)
        {
            selectedSlot = shellSlots.Count - 1;
        }

        if (selectedSlot >= shellSlots.Count)
        {
            selectedSlot = 0;
        }

        HighlightSlot(selectedSlot);
    }

    private void HighlightSlot(int indice)
    {
        shellSlots[indice].GetComponent<Slot>().SelectSlot();
    }

    public void EquipaggiaGuscioSelezionato(InputAction.CallbackContext context)
    {
        if (!panelInventario.activeSelf)
            return;

        if (context.started)
        {
            if (selectedSlot < 0 || selectedSlot >= shellList.Count)
            {
                Debug.Log("Non hai ancora trovato questo guscio!");
                return;
            }

            Shell selezionato = shellList[selectedSlot];

            if (selezionato == null)
            {
                Player.Instance.animator.SetBool("isChange", true);
                Player.Instance.shellManager.RemoveShell();
            }
            else if (Player.Instance.shellManager.currentShell != selezionato)
            {
                Player.Instance.animator.SetBool("isChange", true);
                Player.Instance.shellManager.RemoveShell();
                var shellPicker = Player.Instance.shellManager.GetShellPickerByShell(selezionato);
                Player.Instance.shellManager.WearShell(selezionato, shellPicker);
            }

            panelInventario.SetActive(false);
            Player.Instance.GetComponent<PlayerInput>().inputBlock = false;
            Player.Instance.canMove = true;
        }
    }

    public void AggiungiGuscio(Shell nuovoGuscio, ShellPicker shellPicker)
    {
        if (!shellList.Contains(nuovoGuscio))
        {
            shellList.Add(nuovoGuscio);

            int slotIndex = shellList.Count - 1;
            if (slotIndex < shellSlots.Count)
            {
                shellSlots[slotIndex].SetIcon();
            }

            AggiornaInventarioUI();
        }
    }

    private void AggiornaInventarioUI()
    {
        for (int i = 0; i < shellSlots.Count; i++)
        {
            shellSlots[i].gameObject.SetActive(true);
        }
    }
}

