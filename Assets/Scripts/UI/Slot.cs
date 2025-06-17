using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    //public Sprite icon;
    public Image unlockIcon;
    public Image selectionIcon;

    void Start()
    {
        if (selectionIcon != null)
        {
            selectionIcon.gameObject.SetActive(false);
            unlockIcon.gameObject.SetActive(false);
        }
    }
    public void SetIcon()
    {
        unlockIcon.gameObject.SetActive(true);
    }

    public void SetCollezionabile(Sprite sprite)
    {
        this.GetComponent<Image>().sprite = sprite;
    }

    public void SelectSlot()
    {
        selectionIcon.gameObject.SetActive(true);
    }

    public void DeselectSlot()
    {
        selectionIcon.gameObject.SetActive(false);
    }
}
