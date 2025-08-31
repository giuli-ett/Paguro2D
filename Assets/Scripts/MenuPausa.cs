using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject _pauseMenu;
    public bool pauseAperto = false;

    void Start()
    {
        _pauseMenu.SetActive(pauseAperto);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ApriPausa();
        }
    }
    public void ApriPausa()
    {
        // (apri o chiudi)

        pauseAperto = !pauseAperto;
        // Mostra o nascondi
        Time.timeScale = pauseAperto ? 0 : 1;
        _pauseMenu.SetActive(pauseAperto);
        if (pauseAperto)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        Cursor.visible = pauseAperto;
        
    }
}