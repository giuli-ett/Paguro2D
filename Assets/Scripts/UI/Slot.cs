using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Sprite icon;

    public void SetIcon()
    {
       transform.GetChild(0).GetComponent<Image>().sprite = icon;
    }
}
