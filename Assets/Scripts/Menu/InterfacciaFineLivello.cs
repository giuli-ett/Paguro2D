using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfacciaFineLivello : MonoBehaviour
{
    public static InterfacciaFineLivello Instance;
    public List<Collezionabili> collezionabili = new List<Collezionabili>();
    public delegate void OnItemCollected(Collezionabili collezionabile);
    public event OnItemCollected ItemCollected;
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
    public void SceltaLivello()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void TornaMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void CollectItem(Collezionabili collezionabile)
    {
        if (!collezionabili.Contains(collezionabile))
        {
            collezionabili.Add(collezionabile);
            Debug.Log($"✨ Collezionabile raccolto: {collezionabile}");

            ItemCollected?.Invoke(collezionabile);

            AggiornaTestoUI();
        }
    }

    public bool HasCollected(Collezionabili collezionabile)
    {
        return collezionabili.Contains(collezionabile);
    }

    public int TotalCollected => collezionabili.Count;

    private void AggiornaTestoUI()
    {
        if (number != null)
        {
            number.text = $"Collezionabili: {TotalCollected}/3";
        }
            
    }
}
