using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfacciaFineLivello : MonoBehaviour
{
    public static InterfacciaFineLivello Instance;
    public TextMeshProUGUI number;
    public GameObject livello1;
    public GameObject livello2;

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
        if (GameManager.Instance.livello1Completato && !GameManager.Instance.livello2Completato)
        {
            AudioManager.Instance.PlayClick();
            Debug.Log("Carico livello 2");
            SceneManager.LoadSceneAsync(4);
        }
        if (GameManager.Instance.livello2Completato)
        {
            AudioManager.Instance.PlayClick();
            Debug.Log("Torno alla home");
            SceneManager.LoadSceneAsync(0);
        }
        
    }

    public void TornaMenu()
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(0);
    }

    private void OnItemCollected(Collezionabile collezionabile)
    {
        AggiornaTestoUI();
    }

    private void AggiornaTestoUI()
    {
        if (GameManager.Instance.livello1Completato)
        {
            livello2.SetActive(false);
            livello1.SetActive(true);
        }
        else if (GameManager.Instance.livello2Completato)
        {
            livello1.SetActive(false);
            livello2.SetActive(true);
        }
        
        
        if (number != null && GameManager.Instance != null)
        {
            number.text = $"Collezionabili: {GameManager.Instance.TotalCollected}/3";
        }
    }
}
