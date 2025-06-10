using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InventarioCircolare : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    public GameObject panelInventario;
    public PlayerInput input;
    public List<Shell> shellList;
    public Shell baseShell;
    [SerializeField] private RectTransform slotsContainer;

    [Header("LAYOUT SEMICERCHIO")]
    public float radius = 200f;
    [Range(-360, 360)]
    public float startAngle = -90f;
    [Range(-360, 360)]
    public float endAngle = 90f;

    [Header("SLOT NAVIGATION")]
    public List<Slot> shellSlots;
    public int selectedSlot = 0;


    private void Awake()
    {
        panelInventario.SetActive(false);

        shellList = new List<Shell>();
        shellList.Add(baseShell);
    }

    private void Start()
    {
        AggiornaInventarioUI();
        HighlightSlot(selectedSlot);
    }

    public void MostraInventario(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            bool isActive = !panelInventario.activeSelf;
            panelInventario.SetActive(isActive);

            Player.Instance.canMove = !isActive;

            if (isActive)
            {
                AggiornaInventarioUI(); 
                selectedSlot = 0;
                HighlightSlot(selectedSlot);
            }
            /*
            else
            {
                if (shellSlots.Count > 0 && selectedSlot >= 0 && selectedSlot < shellSlots.Count)
                {
                    shellSlots[selectedSlot].GetComponent<Image>().color = Color.white;
                }
            }
            */
        }
    }

    public void Naviga(InputAction.CallbackContext context)
    {
        if (!panelInventario.activeSelf) return;

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

    private void MoveSelection(int direction)
    {
        if (shellSlots.Count > 0)
        {
            shellSlots[selectedSlot].GetComponent<Image>().color = Color.white;
        }

        selectedSlot += direction;

        if (selectedSlot < 0)
        {
            selectedSlot = shellSlots.Count - 1;
        }
        else if (selectedSlot >= shellSlots.Count)
        {
            selectedSlot = 0;
        }

        HighlightSlot(selectedSlot);
    }

    private void HighlightSlot(int indice)
    {
        if (indice >= 0 && indice < shellSlots.Count)
        {
            shellSlots[indice].GetComponent<Image>().color = new Color(0.925f, 0.925f, 0.537f);
            shellSlots[indice].transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack);
            for (int i = 0; i < shellSlots.Count; i++)
            {
                if (i != indice)
                {
                    shellSlots[i].transform.DOScale(1.0f, 0.1f).SetEase(Ease.OutQuad);
                }
            }
        }
    }

    public void EquipaggiaGuscioSelezionato(InputAction.CallbackContext context)
    {
        if (!panelInventario.activeSelf) return;

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
            Player.Instance.GetComponent<PlayerInput>().inputBlock = false; // Se inputBlock è un tuo campo
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
                shellSlots[slotIndex].gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Hai più gusci che slot UI predefiniti! Considera di istanziare nuovi slot.");
            }

            AggiornaInventarioUI();
        }
    }

    private void AggiornaInventarioUI()
    {
        if (shellSlots == null || shellSlots.Count == 0 || slotsContainer == null)
        {
            Debug.LogWarning("Cannot update inventory UI: slots or container are null/empty.");
            return;
        }

        float totalAngle = endAngle - startAngle;
        if (totalAngle < 0) totalAngle += 360; 

        float angleStep = shellSlots.Count > 1 ? totalAngle / (shellSlots.Count - 1) : 0;

        for (int i = 0; i < shellSlots.Count; i++)
        {
            Slot slot = shellSlots[i];
            if (slot == null) continue;

            slot.gameObject.SetActive(true); 

            if (i < shellList.Count)
            {
                slot.SetIcon(); 
            }

            float currentAngle = startAngle + (i * angleStep);

            float angleRad = currentAngle * Mathf.Deg2Rad;

            float x = radius * Mathf.Cos(angleRad);
            float y = radius * Mathf.Sin(angleRad);

            slot.GetComponent<RectTransform>().anchoredPosition = new Vector2(x, y);

            slot.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, currentAngle + 90);
        }
    }
}
