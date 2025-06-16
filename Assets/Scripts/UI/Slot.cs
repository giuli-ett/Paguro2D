using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Sprite icon;
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
        //transform.GetChild(0).GetComponent<Image>().sprite = icon;
    }

    public void SetCollezionabile()
    {
        this.GetComponent<Image>().sprite = icon;
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
