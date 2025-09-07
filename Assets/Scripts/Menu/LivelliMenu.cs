using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LivelliMenu : MonoBehaviour
{
    public Button defaultButton;
    public Button[] menuButtons;
    public Color32 originalColor;
    public Color32 color = new Color32(176, 159, 173, 255);
    private bool canNavigate = true;
    public float stickThreshold = 0.5f;
    private int currentIndex = 0;

    void Start()
    {
        if (menuButtons.Length > 0)
        {
            originalColor = menuButtons[0].GetComponent<Image>().color;
        }
        HighlightSlot(0);
        EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
    }

    void Update()
    {
        if (GameManager.Instance.isUsingController)
        {
            Vector2 nav = Vector2.zero;

            if (Gamepad.current != null)
            {
                nav = Gamepad.current.leftStick.ReadValue();
                Vector2 dpad = Gamepad.current.dpad.ReadValue();
                if (Mathf.Abs(dpad.y) > Mathf.Abs(nav.y))
                    nav.y = dpad.y;
            }

            if (canNavigate && menuButtons.Length > 0)
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

        // Se non c'è selezione, partiamo dal primo
        if (currentIndex == -1)
            currentIndex = 0;
        else
            currentIndex += direction;

        if (currentIndex < 0) currentIndex = menuButtons.Length - 1;
        if (currentIndex >= menuButtons.Length) currentIndex = 0;

        HighlightSlot(currentIndex);
        EventSystem.current.SetSelectedGameObject(menuButtons[currentIndex].gameObject);
    }

    private void PressCurrentButton()
    {
        if (menuButtons.Length == 0 || currentIndex == -1) return;

        var button = menuButtons[currentIndex];
        if (button != null)
        {
            button.onClick.Invoke();
        }
    }

    private void HighlightSlot(int index)
    {
        if (menuButtons.Length == 0 || index < 0 || index >= menuButtons.Length) return;
        menuButtons[index].GetComponent<Image>().color = color;
    }

    public void DeselectAllSlots()
    {
        foreach (var slot in menuButtons)
            slot.GetComponent<Image>().color = originalColor;
    }

    public void PlayLivelloUno()
    {
        GameManager.Instance.livello1Completato = false;
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(3);
        Cursor.visible = false;
        MusicPlayer.Instance.PlayLevel1Music();
    }
    public void PlayLivelloDue()
    {
        GameManager.Instance.livello2Completato = false;
        AudioManager.Instance.PlayClick();
        SceneManager.LoadSceneAsync(4);
        Cursor.visible = false;
        MusicPlayer.Instance.PlayLevel2Music();
    }
    public void PlayLivelloTre()
    {
        AudioManager.Instance.PlayClick();
    }
    public void PlayLivelloQuattro()
    {
        AudioManager.Instance.PlayClick();
    }

}
