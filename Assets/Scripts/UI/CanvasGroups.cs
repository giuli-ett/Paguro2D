using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroups : MonoBehaviour
{
    public Shell doppioSalto;
    public Shell luminescenza;
    public Shell dash;
    public Shell nascondiScava;
    public CanvasGroup doppioSaltoCanvas;
    public CanvasGroup luminescenzaCanvas;
    public CanvasGroup dashCanvas;
    public CanvasGroup nascondiScavaCanvas;
    public Dictionary<Shell, CanvasGroup> listaFeedbackGusci;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        listaFeedbackGusci = new Dictionary<Shell, CanvasGroup>();

        listaFeedbackGusci.Add(doppioSalto, doppioSaltoCanvas);
        listaFeedbackGusci.Add(luminescenza, luminescenzaCanvas);
        listaFeedbackGusci.Add(dash, dashCanvas);
        listaFeedbackGusci.Add(nascondiScava, nascondiScavaCanvas);
    }
}
