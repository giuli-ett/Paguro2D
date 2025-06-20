using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollezionabiliPresi : MonoBehaviour
{
    public List<Image> slotImages; // 3 slot assegnati dall'Inspector

    private void OnEnable()
    {
        AggiornaSlot();
    }

    public void AggiornaSlot()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogWarning("GameManager.Instance è null!");
            return;
        }

        if (slotImages == null || slotImages.Count == 0)
        {
            Debug.LogWarning("Nessuna immagine assegnata negli slot.");
            return;
        }

        var collezionati = GameManager.Instance.collectedItems;
        Debug.Log($"Numero collezionabili raccolti: {collezionati.Count}");

        for (int i = 0; i < slotImages.Count; i++)
        {
            if (i < collezionati.Count && collezionati[i] != null)
            {
                slotImages[i].gameObject.SetActive(true);
                Debug.Log($"Slot {i} ATTIVO con oggetto: {collezionati[i].nome}");
            }
            else
            {
                slotImages[i].gameObject.SetActive(false);
                Debug.Log($"Slot {i} DISATTIVATO (nessun collezionabile).");
            }
        }
    }
}
