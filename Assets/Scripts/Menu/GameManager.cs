using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public List<LivelloData> livelli;
    public List<Collezionabile> collectedItems = new();
    public LivelloData currentLivello;
    public List<int> idCollezionabiliRaccolti = new();
    public bool livello1Completato = false;
    public bool livello2Completato = false;
    public bool isUsingController;
    public Dictionary<int, bool> listaLivello1 = new();
    public Dictionary<int, bool> listaLivello2 = new();
    public int currentLevel;

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
        isUsingController = Gamepad.current != null;

        listaLivello1.Add(1, false);
        listaLivello1.Add(2, false);
        listaLivello1.Add(3, false);

        listaLivello2.Add(4, false);
        listaLivello2.Add(5, false);
        listaLivello2.Add(6, false);
    }

    void Update()
    {
        isUsingController = Gamepad.current != null;
        string currentScene = SceneManager.GetActiveScene().name;
        bool canPause = currentScene == "Livello1DEMO" || currentScene == "Livello2DEMO";

        if (!canPause) return;

        if (!isUsingController)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseManager.Instance.Pausa();
            }
        }
        else
        {
            var gamepad = Gamepad.current;
            if (gamepad != null)
            {
                if (gamepad.startButton.wasPressedThisFrame) // PS Options / Xbox Menu
                {
                    PauseManager.Instance.Pausa();
                }
            }
        }
    }

    public void CollectItem(Collezionabile item)
    {
        if (currentLevel == 1)
        {
            if (listaLivello1.ContainsKey(item.idCollezionabile))
            {
                listaLivello1[item.idCollezionabile] = true;
            }
        }
        else if (currentLevel == 2)
        {
            if (listaLivello2.ContainsKey(item.idCollezionabile))
            {
                listaLivello2[item.idCollezionabile] = true;
            }
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
