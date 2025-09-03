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
        List<int> raccolti = GameManager.Instance.idCollezionabiliRaccolti;

        foreach (var slot in slotUI)
        {
            slot.imageSlot.enabled = raccolti.Contains(slot.id);
        }
    }
}