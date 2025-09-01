using UnityEngine;
using UnityEngine.UI;

public class LightProgressBar : MonoBehaviour
{
    public static LightProgressBar Instance { get; private set; }

    [SerializeField] private Image fillImage;
    [SerializeField] private Color fullColor = Color.yellow;
    [SerializeField] private Color lowColor = Color.red;
    [SerializeField] private float lowThreshold = 0.2f; // 20%

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

     private void Start()
    {
        // Nascondi la barra all'avvio
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateProgress(float percentage)
    {
        float normalizedValue = percentage / 100f;
        fillImage.fillAmount = normalizedValue;

        // Cambia colore quando la luce è bassa
        if (normalizedValue <= lowThreshold)
        {
            fillImage.color = Color.Lerp(lowColor, fullColor, normalizedValue / lowThreshold);
        }
        else
        {
            fillImage.color = fullColor;
        }
    }
}