using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<LivelloData> livelli;
    public List<Collezionabile> collectedItems = new();
    public LivelloData currentLivello;
    public event Action<Collezionabile> ItemCollectedEvent;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        currentLivello = livelli[0];
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        Cursor.visible = false;
    }

    public void CollectItem(Collezionabile item)
    {
        if (!collectedItems.Contains(item))
        {
            collectedItems.Add(item);
            ItemCollectedEvent?.Invoke(item);
        }
    }

    public void SetCurrentLevel(int numero)
    {
        if (numero >= 0 && numero < livelli.Count)
        {
            currentLivello = livelli[numero];
            Debug.Log($"Livello corrente impostato su: {currentLivello.nomeLivello}");
        }
        else
        {
            Debug.LogError($"Indice livello non valido: {numero}");
        }
    }


    public int TotalCollected => collectedItems.Count;
}
