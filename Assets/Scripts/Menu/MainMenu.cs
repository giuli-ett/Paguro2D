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
    public InputActionAsset menu;
    private bool canNavigate = true;
    public float stickThreshold = 0.5f;
    private int currentIndex = -1;

    void Start()
    {
        MusicPlayer.Instance.PlayMenuMusic();

        originalColor = menuButtons[0].GetComponent<Image>().color;
        //HighlightSlot(0);
    }
    public void PlayGame()
    {
        AudioManager.Instance.PlayClick();
        //Cursor.visible = false;
        SceneManager.LoadSceneAsync(2);
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

    public void MenuNaviga(InputAction.CallbackContext context)
    {
        if (menuButtons.Length == 0) return;

        Vector2 navigation = context.ReadValue<Vector2>();

        if (canNavigate)
        {
            Debug.Log("Navigo");

            if (navigation.y > stickThreshold)
            {
                MoveSelection(-1);
                canNavigate = false;
            }
            else if (navigation.y < -stickThreshold)
            {
                MoveSelection(1);
                canNavigate = false;
            }
        }

        if (Mathf.Abs(navigation.y) < stickThreshold)
        {
            canNavigate = true;
        }
        
    }

    private void MoveSelection(int direction)
    {
        DeselectAllSlots();

        currentIndex += direction;

        if (currentIndex < 0) currentIndex = menuButtons.Length - 1;
        if (currentIndex >= menuButtons.Length) currentIndex = 0;

        HighlightSlot(currentIndex);
        EventSystem.current.SetSelectedGameObject(menuButtons[currentIndex].gameObject);
    }


    public void HighlightSlot(int indice)
    {
        menuButtons[indice].GetComponent<Image>().color = color;
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed && EventSystem.current.currentSelectedGameObject != null)
        {
            var button = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.Invoke();
            }
        }
    }

    public void DeselectAllSlots()
    {
        foreach (var slot in menuButtons)
            slot.GetComponent<Image>().color = originalColor;
    }

    void OnDisable()
    {
        if (menu != null)
        {
            menu.Disable(); 
        }
    }
}
