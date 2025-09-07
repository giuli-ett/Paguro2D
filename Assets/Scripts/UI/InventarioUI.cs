using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;
using Unity.Collections;

public class InventarioUI : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    public GameObject panelInventario;
    public List<Shell> shellList;
    public Dictionary<ShellPower, int> shellSlotMap;
    public Dictionary<ShellPower, Shell> shellInventory;

    [Header("SLOT NAVIGATION")]
    public List<Slot> shellSlots;
    public int selectedSlot = 0;
    private int lastSelectedSlot = 0;
    private bool lastSelectedWasLocked = false;

    private void Awake()
    {
        panelInventario.SetActive(false);
        shellList = new List<Shell>();
        shellInventory = new Dictionary<ShellPower, Shell>();

        shellSlotMap = new Dictionary<ShellPower, int>
        {
            { ShellPower.JumpBoost, 0 },
            { ShellPower.SpeedBoost, 1 },
            { ShellPower.NascondiScava, 2 },
            { ShellPower.Luminescenza, 3 }
        };

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

    public void DeselectAllSlots()
    {
        foreach (var slot in shellSlots)
            slot.DeselectSlot();
    }

    private void SelectSlotByNumber(int index)
    {
        if (index >= 0 && index < shellSlots.Count)
        {
            DeselectAllSlots();
            selectedSlot = index;
            HighlightSlot(selectedSlot);

            ShellPower selezionato = shellSlotMap.FirstOrDefault(x => x.Value == selectedSlot).Key;

            if (shellInventory.TryGetValue(selezionato, out Shell guscioSelezionato))
            {
                if (guscioSelezionato == null)
                {
                    Player.Instance.animator.SetBool("isChange", true);
                    Player.Instance.shellManager.RemoveShell();
                }
                else if (Player.Instance.shellManager.currentShell != guscioSelezionato)
                {
                    Player.Instance.animator.SetBool("isChange", true);
                    Player.Instance.shellManager.RemoveShell();
                    var shellPicker = Player.Instance.shellManager.GetShellPickerByShell(guscioSelezionato);
                    Player.Instance.shellManager.WearShell(guscioSelezionato, shellPicker);
                }
            }
            else
            {
                Debug.Log("Non hai ancora trovato questo guscio!");
            }
        }
    }

    public void MostraInventario(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (PauseManager.Instance.isPaused) return;

        if (FeedbackTartaruga.Instance != null &&
            FeedbackTartaruga.Instance.tutorialInCorso &&
            !FeedbackTartaruga.Instance.PuoAprireInventarioDuranteTutorial())
        {
            Debug.Log("Inventario bloccato durante il tutorial");
            return;
        }

        bool isActive = !panelInventario.activeSelf;
        panelInventario.SetActive(isActive);

        if (isActive)
        {
            AggiornaInventarioUI();

            // Logica alla riapertura
            Shell currentShell = Player.Instance.shellManager.currentShell;

            if (lastSelectedWasLocked)
            {
                if (currentShell != null && shellSlotMap.TryGetValue(currentShell.power, out int equippedIndex))
                {
                    selectedSlot = equippedIndex; // ho un guscio indossato
                }
                else
                {
                    selectedSlot = 0; // nessun guscio → slot 0
                }
            }
            else
            {
                if (currentShell != null && shellSlotMap.TryGetValue(currentShell.power, out int equippedIndex))
                    selectedSlot = equippedIndex;
                else
                    selectedSlot = 0;
            }

            DeselectAllSlots();
            HighlightSlot(selectedSlot);
        }
        else
        {
            // Salva stato alla chiusura
            lastSelectedSlot = selectedSlot;
            ShellPower lastPower = shellSlotMap.FirstOrDefault(x => x.Value == lastSelectedSlot).Key;
            lastSelectedWasLocked = !shellInventory.ContainsKey(lastPower);
        }
    }

    public void Naviga(InputAction.CallbackContext context)
    {
        if (!panelInventario.activeSelf)
            return;

        /*
        if(PauseManager.Instance.isPaused) 
            return;
        */
        
        Vector2 navigation = context.ReadValue<Vector2>();
        //AudioManager.Instance.PlayClick();

        if (navigation.y > 0.5f)
        {
            MoveSelection(-1);
        }
        else if (navigation.y < -0.5f)
        {
            MoveSelection(1);
        }
    }

    private void MoveSelection(int direction)
    {
        DeselectAllSlots();

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
        SelectSlotByNumber(selectedSlot);
    }

    public void HighlightSlot(int indice)
    {
        shellSlots[indice].GetComponent<Slot>().SelectSlot();
    }

    public void AggiungiGuscio(Shell nuovoGuscio, ShellPicker shellPicker)
    {
        ShellPower power = nuovoGuscio.power;

        if (!shellInventory.ContainsKey(power))
        {
            shellInventory[power] = nuovoGuscio;

            if (shellSlotMap.TryGetValue(nuovoGuscio.power, out int slotIndex))
            {
                if (slotIndex < shellSlots.Count)
                {
                    shellSlots[slotIndex].SetIcon();
                }
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

