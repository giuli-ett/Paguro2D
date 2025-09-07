using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;
    public bool isPaused = false;
    public Button defaultButton;
    public Button[] menuButtons;
    public Color32 originalColor;
    public Color32 color = new Color32(176, 159, 173, 255);
    private bool canNavigate = true;
    public float stickThreshold = 0.5f;
    private int currentIndex = 0;
    public Canvas canvas;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        canvas.gameObject.SetActive(false);
        //DontDestroyOnLoad(gameObject);

        if (menuButtons.Length > 0)
        {
            originalColor = menuButtons[0].GetComponent<Image>().color;
        }
    }

    void Update()
    {
        if (!isPaused) return;

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
    public void Pausa()
    {
        Cursor.visible = true;
        Time.timeScale = 0f;
        Player.Instance.canMove = false;
        canvas.gameObject.SetActive(true);
        isPaused = true;

        currentIndex = 0;
        DeselectAllSlots();
        if (menuButtons.Length > 0)
        {
            HighlightSlot(currentIndex);
            EventSystem.current.SetSelectedGameObject(menuButtons[currentIndex].gameObject);
        }

    }
    public void Continua()
    {
        AudioManager.Instance.PlayClick();
        Cursor.visible = false;
        Time.timeScale = 1f;
        Player.Instance.canMove = true;
        canvas.gameObject.SetActive(false);
        isPaused = false;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        AudioManager.Instance.PlayClick();
        canvas.gameObject.SetActive(false);
        SceneManager.LoadSceneAsync(0);
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

    private void DeselectAllSlots()
    {
        foreach (var b in menuButtons)
        {
            b.GetComponent<Image>().color = originalColor;
        }
    }

}
