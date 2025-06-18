using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

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
        //shellList.Add(baseShell);

        AggiornaInventarioUI();
    }

    private void Update()
    {
        if (panelInventario.activeSelf)
        {
            for (int i = 0; i < shellSlots.Count; i++)
            {
                // I tasti numerici vanno da Alpha1 (1) a Alpha9 (9)
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
        if (TabTutorial.Instance != null && TabTutorial.Instance.isRunningTutorial)
        return;

        if (context.started)
        {
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
                // Quando chiudi l'inventario, cambia guscio se la selezione è diversa
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

        if (navigation.x > 0.5f)
        {
            MoveSelection(1);
        }
        else if (navigation.x < -0.5f)
        {
            MoveSelection(-1);
        }
    }

    private void MoveSelection (int direction)
    {
        foreach (var slot in shellSlots)
        {
            slot.DeselectSlot();
        }

        //shellSlots[selectedSlot].GetComponent<Outline>().effectColor = Color.white;

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
        //shellSlots[indice].GetComponent<Image>().color = new Color(0.925f, 0.925f, 0.537f);
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

    public void AggiungiGuscio (Shell nuovoGuscio, ShellPicker shellPicker)
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
