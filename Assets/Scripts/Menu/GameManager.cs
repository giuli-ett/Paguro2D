using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<CollezionabiliLivello> listaLivelli = new();
    public List<Collezionabile> collectedItems = new();
    public event Action<Collezionabile> ItemCollectedEvent;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public void CollectItem(Collezionabile item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            ItemCollectedEvent?.Invoke(item);
        }
    }

    public int TotalCollected => collectedItems.Count;
}
