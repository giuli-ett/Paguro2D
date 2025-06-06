using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfacciaFineLivello : MonoBehaviour
{
    public static InterfacciaFineLivello Instance;
    public TextMeshProUGUI number;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }
    
    private void OnEnable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ItemCollectedEvent += OnItemCollected;
    }
    
    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ItemCollectedEvent -= OnItemCollected;
    }

    private void Start()
    {
        AggiornaTestoUI();
    }

    public void SceltaLivello()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void TornaMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void OnItemCollected(Collezionabile collezionabile)
    {
        AggiornaTestoUI();
    }

    private void AggiornaTestoUI()
    {
        if (number != null && GameManager.Instance != null)
        {
            number.text = $"Collezionabili: {GameManager.Instance.TotalCollected}/3";
        }
    }
}
