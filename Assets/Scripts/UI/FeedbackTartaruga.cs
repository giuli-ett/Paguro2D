using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public class FeedbackTartaruga : MonoBehaviour
{
    public static FeedbackTartaruga Instance;

    [Header("MOVIMENTO")]
    public float swimDuration = 5f;
    public float pauseDuration = 2f;
    public float exitOffset = 200f;
    public Vector2 targetPosition;
    private Vector2 startFixedPosition;

    [Header("RIFERIMENTI")]
    public RectTransform canvasRect;
    public RectTransform rectTransform;
    private Sequence swimSequence;
    private Animator animator;

    [Header("TESTI GUSCI")]
    public GameObject doppioSalto;
    public GameObject dash;
    public GameObject scava;
    [Header("TESTI TUTORIAL")]
    public List<GameObject> frasi;
    private int currentFraseIndex = 0;
    private bool isWaitingForInput = false;
    public bool tutorialInCorso = false;
    public float timePerFrase = 3f;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
        startFixedPosition = rectTransform.anchoredPosition;

        doppioSalto.SetActive(false);
        dash.SetActive(false);
        scava.SetActive(false);

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

    public void StartSwimForShellFeedback()
    {
        swimSequence?.Kill();

        float canvasWidth = canvasRect.rect.width;
        float turtleWidth = rectTransform.rect.width;
        Vector2 finalPos = new Vector2(-canvasWidth / 2 - turtleWidth - exitOffset, targetPosition.y);

        rectTransform.anchoredPosition = startFixedPosition;
        animator.SetBool("isMoving", true);

        swimSequence = DOTween.Sequence();
        swimSequence.Append(rectTransform.DOAnchorPos(targetPosition, swimDuration / 2f).SetEase(Ease.Linear));
        swimSequence.AppendCallback(() =>
        {
            animator.SetBool("isMoving", false);
        });
        swimSequence.AppendInterval(pauseDuration);
        swimSequence.AppendCallback(() =>
        {
            animator.SetBool("isMoving", true);
        });
        swimSequence.Append(rectTransform.DOAnchorPos(finalPos, swimDuration / 2f).SetEase(Ease.Linear));

        swimSequence.OnComplete(() =>
        {
            animator.SetBool("isMoving", false);
            doppioSalto.SetActive(false);
            dash.SetActive(false);
            scava.SetActive(false);
        });
    }

    public void SetText(Shell shell)
    {
        if (shell == null)
        {
            Debug.Log("Nessun guscio");
        }
        else if (shell.shellName == "Guscio salterino")
        {
            doppioSalto.SetActive(true);
        }
        else if (shell.shellName == "Guscio Dash")
        {
            dash.SetActive(true);
        }
        else if (shell.shellName == "NascondiScava")
        {
            scava.SetActive(true);
        }
    }

    public void StartSwimForTutorialIntro()
    {
        ShowFrase(currentFraseIndex);
        currentFraseIndex = 0;
        tutorialInCorso = true;
        swimSequence?.Kill();

        rectTransform.anchoredPosition = startFixedPosition;
        animator.SetBool("isMoving", true);

        swimSequence = DOTween.Sequence();
        swimSequence.Append(rectTransform.DOAnchorPos(targetPosition, swimDuration / 2f).SetEase(Ease.Linear));
        swimSequence.AppendCallback(() =>
        {
            animator.SetBool("isMoving", false);
            isWaitingForInput = true;
        });
    }

    private void ShowFrase(int index)
    {
        foreach (var f in frasi) f.SetActive(false);
        if (index < frasi.Count)
        {
            frasi[index].SetActive(true);
        }
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
        swimSequence = DOTween.Sequence();

        for (int i = currentFraseIndex; i < frasi.Count; i++)
        {
            int index = i;
            swimSequence.AppendCallback(() =>
            {
                ShowFrase(index);
            });

            swimSequence.AppendInterval(timePerFrase);

            if (i < frasi.Count - 1)
            {
                swimSequence.AppendCallback(() =>
                {
                    frasi[index].SetActive(false);
                });
            }
        }
        
        swimSequence.AppendCallback(() =>
        {
            currentFraseIndex = frasi.Count;
            StartExitAfterTutorial();
        });
    }

    private void StartExitAfterTutorial()
    {
        tutorialInCorso = false;
        float canvasWidth = canvasRect.rect.width;
        float turtleWidth = rectTransform.rect.width;
        Vector2 finalPos = new Vector2(-canvasWidth / 2 - turtleWidth - exitOffset, targetPosition.y);

        animator.SetBool("isMoving", true);

        swimSequence = DOTween.Sequence();
        swimSequence.Append(rectTransform.DOAnchorPos(finalPos, swimDuration / 2f).SetEase(Ease.Linear));
        swimSequence.OnComplete(() =>
        {
            animator.SetBool("isMoving", false);
            foreach (var f in frasi) f.SetActive(false);
        });
    }

    public bool IsOnFirstFrase()
    {
        return currentFraseIndex == 0 && isWaitingForInput;
    }

    public bool PuoAprireInventarioDuranteTutorial()
    {
        return tutorialInCorso && currentFraseIndex == 0 && isWaitingForInput;
    }
}

