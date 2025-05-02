using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventarioUI : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    public GameObject panelInventario;
    public PlayerInput input;
    public List<Shell> shellList;
    public ShellPicker shellPrefab;

    [Header("SLOT NAVIGATION")]
    public List<Slot> shellSlots;
    public int selectedSlot = 0;


    private void Start()
    {
        panelInventario.SetActive(false);

        shellList = new List<Shell>();
        shellList.Add(null);

        AggiornaInventarioUI();
    }

    private void Update()
    {
        if (input.Inventory)
        {
            MostraInventario();
        }

        if (panelInventario.activeSelf)
        {
            Naviga();

            if (input.Select)
            {
                EquipaggiaGuscioSelezionato();
            }
        }
    }

    private void MostraInventario()
    {
        bool isActive = !panelInventario.activeSelf;
        panelInventario.SetActive(isActive);
        input.inputBlock = isActive;

        if (isActive)
        {
            AggiornaInventarioUI();
            HighlightSlot(selectedSlot);
        }
    }

    private void Naviga()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveSelection(1);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            MoveSelection(-1);
        }
    }

    private void MoveSelection (int direction)
    {
        shellSlots[selectedSlot].GetComponent<Outline>().effectColor = Color.white;

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

    private void HighlightSlot (int indice)
    {
        shellSlots[indice].GetComponent<Outline>().effectColor = new Color(0.925f, 0.925f, 0.537f);
    }

    private void EquipaggiaGuscioSelezionato()
    {
        if (selectedSlot < 0 || selectedSlot >= shellList.Count)
        {
            Debug.Log("Non hai ancora trovato questo guscio!");
            return;
        }

        Shell selezionato = shellList[selectedSlot];

        if (selezionato == null)
        {
            Player.Instance.shellManager.RemoveShell();
        }
        else if (Player.Instance.shellManager.currentShell != selezionato)
        {
            Player.Instance.shellManager.RemoveShell();
            var shellPicker = Player.Instance.shellManager.GetShellPickerByShell(selezionato);
            Player.Instance.shellManager.WearShell(selezionato, shellPicker);
        }

        panelInventario.SetActive(false);
        Player.Instance.GetComponent<PlayerInput>().inputBlock = false;
    }

    public void AggiungiGuscio (Shell nuovoGuscio)
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

    /*
    private void AggiornaInventarioUI()
    {
        for (int i = 0; i < shellSlots.Count; i++)
        {
            if (i < shellList.Count)
            {
                shellSlots[i].gameObject.SetActive(true);
            }
            else if (i == 0)
            {
                shellSlots[i].gameObject.SetActive(false);
            }
            else
            {
                shellSlots[i].gameObject.SetActive(false);
            }
        }
    }
    */
}
