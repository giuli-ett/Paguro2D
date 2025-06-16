using System;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGroups : MonoBehaviour
{
    public Shell doppioSalto;
    public Shell luminescenza;
    public CanvasGroup doppioSaltoCanvas;
    public CanvasGroup luminescenzaCanvas;
    public Dictionary<String, CanvasGroup> listaFeedbackGusci;

    void Start()
    {
        listaFeedbackGusci = new Dictionary<String, CanvasGroup>();

        listaFeedbackGusci.Add(doppioSalto.shellName, doppioSaltoCanvas);
        listaFeedbackGusci.Add(luminescenza.shellName, luminescenzaCanvas);
    }
}
