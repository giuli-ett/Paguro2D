using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroups : MonoBehaviour
{
    public Shell doppioSalto;
    public Shell luminescenza;
    public CanvasGroup doppioSaltoCanvas;
    public CanvasGroup luminescenzaCanvas;
    public Dictionary<Shell, CanvasGroup> listaFeedbackGusci;

    void Start()
    {
        listaFeedbackGusci = new Dictionary<Shell, CanvasGroup>();

        listaFeedbackGusci.Add(doppioSalto, doppioSaltoCanvas);
        listaFeedbackGusci.Add(luminescenza, luminescenzaCanvas);
    }
}
