using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class LightProgressBar : MonoBehaviour
{
    public static LightProgressBar Instance { get; private set; }

    [SerializeField] private Image fillImage;      // immagine che si consuma
    [SerializeField] private Image staticImage;    // immagine che rimane sempre fissa (es. contorno lampadina)

    [SerializeField] private Color fullColor = Color.yellow;
    [SerializeField] private Color lowColor = Color.red;
    [SerializeField] private float lowThreshold = 0.2f; // 20%

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Nascondi tutto all'avvio
        gameObject.SetActive(false);
        if (fillImage != null) fillImage.enabled = false;
        if (staticImage != null) staticImage.enabled = false;
    }

    public void Show()
    {
        gameObject.SetActive(true);
        if (fillImage != null) fillImage.enabled = true;
        if (staticImage != null) staticImage.enabled = true;
    }

    public void Hide()
    {
        if (fillImage != null) fillImage.enabled = false;
        if (staticImage != null) staticImage.enabled = false;
        gameObject.SetActive(false);
    }

    public void UpdateProgress(float percentage)
    {
        if (fillImage == null) return;

        float normalizedValue = percentage / 100f;
        fillImage.fillAmount = normalizedValue;

        // Cambia colore solo per l’immagine consumabile
        if (normalizedValue <= lowThreshold)
            fillImage.color = Color.Lerp(lowColor, fullColor, normalizedValue / lowThreshold);
        else
            fillImage.color = fullColor;
    }
}
