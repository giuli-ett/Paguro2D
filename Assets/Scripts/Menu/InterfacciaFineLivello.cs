using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InterfacciaFineLivello : MonoBehaviour
{
    public static InterfacciaFineLivello Instance;
    public TextMeshProUGUI number;
    public GameObject livello1;
    public GameObject livello2;
    [Header("UI")]
    public Button[] menuButtons;
    public Color32 originalColor;
    public Color32 color = new Color32(176, 159, 173, 255);
    private bool canNavigate = true;
    public float stickThreshold = 0.5f;
    private int currentIndex = -1;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        if (menuButtons.Length > 0)
        {
            originalColor = menuButtons[0].GetComponent<Image>().color;
        }
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
            MusicPlayer.Instance.PlayLevel2Music();
            Debug.Log("Carico livello 2");
            SceneManager.LoadSceneAsync(4);
        }
        if (GameManager.Instance.livello2Completato)
        {
            AudioManager.Instance.PlayClick();
            MusicPlayer.Instance.PlayMenuMusic();
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
    
    void Update()
    {
        if (GameManager.Instance.isUsingController)
        {
            Vector2 nav = Vector2.zero;
            if (Gamepad.current != null)
            {
                nav = Gamepad.current.leftStick.ReadValue();
            }

            if (canNavigate)
            {
                if (nav.y > stickThreshold)
                {
                    MoveSelection(-1);
                    canNavigate = false;
                }
                else if (nav.y < -stickThreshold)
                {
                    MoveSelection(1);
                    canNavigate = false;
                }
            }
            if (Mathf.Abs(nav.y) < stickThreshold)
            {
                canNavigate = true;
            }

            if (Gamepad.current != null && Gamepad.current.buttonSouth.wasPressedThisFrame)
            {
                PressCurrentButton();
            }
        }
        else
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                PressCurrentButton();
            }
        }
    }

    private void MoveSelection(int direction)
    {
        if (menuButtons.Length == 0) return;

        DeselectAllSlots();

        currentIndex += direction;
        if (currentIndex < 0) currentIndex = menuButtons.Length - 1;
        if (currentIndex >= menuButtons.Length) currentIndex = 0;

        HighlightSlot(currentIndex);
        EventSystem.current.SetSelectedGameObject(menuButtons[currentIndex].gameObject);
    }

    private void PressCurrentButton()
    {
        if (menuButtons.Length == 0) return;

        var button = menuButtons[currentIndex];
        if (button != null)
        {
            button.onClick.Invoke();
        }
    }

    private void HighlightSlot(int index)
    {
        if (menuButtons.Length == 0) return;
        menuButtons[index].GetComponent<Image>().color = color;
    }

    public void DeselectAllSlots()
    {
        foreach (var slot in menuButtons)
            slot.GetComponent<Image>().color = originalColor;
    }
}
