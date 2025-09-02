using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public class FeedbackTartaruga : MonoBehaviour
{
    public static FeedbackTartaruga Instance;

    [Header("ANIMAZIONE FADE")]
    public float fadeDuration = 1f;
    public float visibleDuration = 2f;
    public CanvasGroup canvasGroup;
    private Sequence fadeSequence;

    [Header("TESTI GUSCI")]
    public GameObject doppioSalto;
    public GameObject dash;
    public GameObject scava;
    public GameObject luminoso;
    [Header("TESTI TUTORIAL")]
    public List<GameObject> frasi;
    private int currentFraseIndex = 0;
    private bool isWaitingForInput = false;
    public bool tutorialInCorso = false;
    public float timePerFrase = 3f;

    void Awake()
    {
        canvasGroup.alpha = 0f;

        doppioSalto.SetActive(false);
        dash.SetActive(false);
        scava.SetActive(false);
        luminoso.SetActive(false);

        foreach (var v in frasi)
        {
            v.gameObject.SetActive(false);
        }

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartShellFeedback(Shell shell)
    {
        SetText(shell);

        fadeSequence?.Kill();
        fadeSequence = DOTween.Sequence();

        fadeSequence.Append(canvasGroup.DOFade(1f, fadeDuration));
        fadeSequence.AppendInterval(visibleDuration);
        fadeSequence.Append(canvasGroup.DOFade(0f, fadeDuration));

        fadeSequence.OnComplete(() =>
        {
            doppioSalto.SetActive(false);
            dash.SetActive(false);
            scava.SetActive(false);
            luminoso.SetActive(false);
        });
    }

    public void SetText(Shell shell)
    {
        if (shell == null)
        {
            Debug.Log("Nessun guscio");
            return;
        }

        doppioSalto.SetActive(shell.shellName == "Guscio salterino");
        dash.SetActive(shell.shellName == "Guscio Dash");
        scava.SetActive(shell.shellName == "NascondiScava");
        luminoso.SetActive(shell.shellName == "Guscio luminescente");
    }

    void Update()
    {
        if (!tutorialInCorso) return;

        if (isWaitingForInput && Input.GetKeyDown(KeyCode.Tab))
        {
            isWaitingForInput = false;
            frasi[currentFraseIndex].SetActive(false);
            currentFraseIndex++;
            StartFrasiSequence();
        }
    }

    private void StartFrasiSequence()
    {
        fadeSequence = DOTween.Sequence();

        for (int i = currentFraseIndex; i < frasi.Count; i++)
        {
            int index = i;
            fadeSequence.AppendCallback(() => ShowFrase(index));
            fadeSequence.AppendInterval(timePerFrase);
            if (i < frasi.Count - 1)
            {
                fadeSequence.AppendCallback(() => frasi[index].SetActive(false));
            }
        }

        fadeSequence.AppendCallback(() =>
        {
            currentFraseIndex = frasi.Count;
            EndTutorial();
        });
    }
    
    public void StartTutorialIntro()
    {
        currentFraseIndex = 0;
        tutorialInCorso = true;
        ShowFrase(currentFraseIndex);

        fadeSequence?.Kill();
        fadeSequence = DOTween.Sequence();

        fadeSequence.Append(canvasGroup.DOFade(1f, fadeDuration));
        fadeSequence.AppendCallback(() =>
        {
            isWaitingForInput = true;
        });
    }

        private void ShowFrase(int index)
        {
            foreach (var f in frasi) f.SetActive(false);
            if (index < frasi.Count) frasi[index].SetActive(true);
        }

    private void EndTutorial()
    {
        tutorialInCorso = false;
        fadeSequence = DOTween.Sequence();
        fadeSequence.Append(canvasGroup.DOFade(0f, fadeDuration));
        fadeSequence.OnComplete(() =>
        {
            foreach (var f in frasi) f.SetActive(false);
        });
    }

    // Utility
    public bool IsOnFirstFrase() => currentFraseIndex == 0 && isWaitingForInput;
    public bool PuoAprireInventarioDuranteTutorial() => tutorialInCorso && currentFraseIndex == 0 && isWaitingForInput;
}

