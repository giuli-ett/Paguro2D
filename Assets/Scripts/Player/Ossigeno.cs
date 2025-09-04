using UnityEngine;
using UnityEngine.UI;

public class Ossigeno : MonoBehaviour
{
    public static Ossigeno Instance;

    [Header("Ossigeno")]
    [SerializeField] private float maxOxygenTime = 20f;
    private float currentOxygenTime;
    private bool isConsuming = false;

    [Header("Interfaccia Bollicine")]
    [SerializeField] private Image[] bollicineOssigeno;

    private LifeController lifeController;

    private float bubbleUpdateInterval = 2f;
    private float bubbleTimer;
    private int currentBubbleIndex;

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
        bubbleTimer -= Time.deltaTime;

        if (bubbleTimer <= 0f)
        {
            bubbleTimer = bubbleUpdateInterval;
            UpdateNextBubble();
        }

        if (currentOxygenTime <= 0f)
        {
            isConsuming = false;
            ShowOxygenBubbles(false);
            lifeController.Die();
        }
    }

    public void StartOxygenConsumption()
    {
        currentOxygenTime = maxOxygenTime;
        isConsuming = true;
        bubbleTimer = bubbleUpdateInterval;
        currentBubbleIndex = 0;
        ShowOxygenBubbles(true);
    }

    public void StopOxygenConsumption()
    {
        isConsuming = false;
        ShowOxygenBubbles(false);
    }

    private void ShowOxygenBubbles(bool show)
    {
        foreach (Image bubble in bollicineOssigeno)
        {
            bubble.enabled = show;
        }
    }

    private void UpdateNextBubble()
    {
        if (currentBubbleIndex < bollicineOssigeno.Length)
        {
            bollicineOssigeno[currentBubbleIndex].enabled = false;
            currentBubbleIndex++;
        }
    }
}