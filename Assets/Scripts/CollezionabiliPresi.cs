/*
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

    private void Start()
    {
        // Lista globale dei collezionabili raccolti
        List<int> raccolti = GameManager.Instance.idCollezionabiliRaccolti;

        // Capisco quale livello � in corso
        int livelloCorrenteIndex = GameManager.Instance.livelli.IndexOf(GameManager.Instance.currentLivello) + 1;

        // Range di ID validi per questo livello
        int min = (livelloCorrenteIndex - 1) * 3 + 1;
        int max = livelloCorrenteIndex * 3;

        Debug.Log($"Livello corrente: {livelloCorrenteIndex}, Range ID: {min}-{max}");
        Debug.Log("Raccolti: " + string.Join(",", raccolti));

        // Aggiorno gli slot
        foreach (var slot in slotUI)
        {
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
*/
