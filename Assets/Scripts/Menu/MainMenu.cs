using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("UI")]
    public Button[] menuButtons;
    public Color32 originalColor;
    public Color32 color = new Color32(176, 159, 173, 255);
    private bool canNavigate = true;
    public float stickThreshold = 0.5f;
    private int currentIndex = -1;

    void Start()
    {
        MusicPlayer.Instance.PlayMenuMusic();

        if (menuButtons.Length > 0)
        {
            originalColor = menuButtons[0].GetComponent<Image>().color;
        }
    }
    public void PlayGame()
    {
        AudioManager.Instance.PlayClick();
        //Cursor.visible = false;
        SceneManager.LoadSceneAsync(2);
        GameManager.Instance.currentLevel = 1;
    }

    public void ContinueGame()
    {
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(1);
    }

    public void ExitGame()
    {
        AudioManager.Instance.PlayClick();
        Application.Quit();
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
