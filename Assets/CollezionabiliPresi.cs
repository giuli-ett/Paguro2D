using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollezionabiliPresi : MonoBehaviour
{
    public List<Image> slotImages; // 3 slot assegnati dall'Inspector
    public Sprite placeholder; // Sprite da mostrare se lo slot è vuoto

    private void OnEnable()
    {
        AggiornaSlot();
    }

    public void AggiornaSlot()
    {
        if (GameManager.Instance == null || slotImages.Count == 0) return;

        var collezionati = GameManager.Instance.collectedItems;

        for (int i = 0; i < slotImages.Count; i++)
        {
            if (i < collezionati.Count && collezionati[i] != null)
            {
                slotImages[i].sprite = collezionati[i].GetComponent<SpriteRenderer>().sprite;
                slotImages[i].gameObject.SetActive(true);
            }
            else
            {
                slotImages[i].gameObject.SetActive(false);
            }
        }
    }

}
