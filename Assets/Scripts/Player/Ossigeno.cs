using UnityEngine;
using UnityEngine.UI;

public class Ossigeno : MonoBehaviour
{
    public static Ossigeno Instance;

    [Header("Ossigeno")]
    [SerializeField] private float maxOxygenTime = 10f;
    private float currentOxygenTime;
    private bool isConsuming = false;

    [Header("Interfaccia Bollicine")]
    [SerializeField] private Image[] bollicineOssigeno; // Assegna le immagini nell’Inspector

    private LifeController lifeController;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentOxygenTime = maxOxygenTime;
        lifeController = FindObjectOfType<LifeController>();
        ShowOxygenBubbles(false);
    }

    private void Update()
    {
        if (!isConsuming) return;

        currentOxygenTime -= Time.deltaTime;
        UpdateOxygenBubbles(currentOxygenTime / maxOxygenTime);

        if (currentOxygenTime <= 0f)
        {
            isConsuming = false;
            ShowOxygenBubbles(false);
            lifeController.Die(); // Morte gestita dal LifeController
        }
    }

    // Inizio consumo ossigeno
    public void StartOxygenConsumption()
    {
        currentOxygenTime = maxOxygenTime;
        isConsuming = true;
        ShowOxygenBubbles(true);
    }

    // Fine consumo ossigeno
    public void StopOxygenConsumption()
    {
        isConsuming = false;
        ShowOxygenBubbles(false);
    }

    // Mostra/Nasconde bollicine
    private void ShowOxygenBubbles(bool show)
    {
        foreach (Image bubble in bollicineOssigeno)
        {
            bubble.enabled = show;
        }
    }

    // Aggiorna bollicine in base al tempo rimasto
    private void UpdateOxygenBubbles(float oxygenRatio)
    {
        int total = bollicineOssigeno.Length;
        int active = Mathf.CeilToInt(oxygenRatio * total);

        for (int i = 0; i < total; i++)
        {
            bollicineOssigeno[i].enabled = i < active;
        }
    }
}