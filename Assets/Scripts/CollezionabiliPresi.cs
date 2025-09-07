using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollezionabiliPresi : MonoBehaviour
{
    [System.Serializable]
    public class SlotCollezionabile
    {
        public int id;
        public Image imageSlot;
    }

    public List<SlotCollezionabile> slotUI;
    public int livelloCorrente; // da impostare nell’Inspector (1 o 2)

    private void Start()
    {
        List<int> raccolti = GameManager.Instance.idCollezionabiliRaccolti;

        foreach (var slot in slotUI)
        {
            // Filtra solo i collezionabili del livello corrente
            int min = (livelloCorrente - 1) * 3 + 1;
            int max = livelloCorrente * 3;

            if (slot.id >= min && slot.id <= max)
            {
                slot.imageSlot.enabled = raccolti.Contains(slot.id);
            }
            else
            {
                slot.imageSlot.enabled = false;
            }
        }
    }
}