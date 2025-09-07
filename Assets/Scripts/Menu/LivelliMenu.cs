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
    public InputActionAsset mondi;
    private bool canNavigate = true;
    public float stickThreshold = 0.5f;
    private int currentIndex = 0;

    void Start()
    {
        originalColor = menuButtons[0].GetComponent<Image>().color;
        HighlightSlot(0);
        EventSystem.current.SetSelectedGameObject(menuButtons[0].gameObject);
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
        if (mondi != null)
        {
            mondi.Disable(); 
        }
    }
}
