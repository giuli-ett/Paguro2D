using System.Collections.Generic;
using System.Data;
using DG.Tweening;
using UnityEngine;

public class TabTutorial : MonoBehaviour
{
    public static TabTutorial Instance;
    public CanvasGroup canvasGroup;
    public List<GameObject> lines;
    public CanvasGroup intro;
    public int currentLineIndex = 0;
    public float fadeDuration = 0.5f;
    public float waitDuration = 2f;

    public bool playerInTrigger = false;
    public bool tutorialStarted = false;
    public bool isRunningTutorial = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        canvasGroup.gameObject.SetActive(false);
        foreach (GameObject line in lines)
        {
            line.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInTrigger && !tutorialStarted && Input.GetKeyDown(KeyCode.Tab))
        {
            tutorialStarted = true;
            canvasGroup.gameObject.SetActive(true);
            AvanzaTutorial();
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            ShowIntro();
            canvasGroup.gameObject.SetActive(true);
        }
    }


    public void AvanzaTutorial()
    {
        isRunningTutorial = true;
        Sequence sequence = DOTween.Sequence();

        for (int i = 0; i < lines.Count; i++)
        {
            int index = i; // necessario per evitare closure problem
            CanvasGroup lineGroup = lines[index].GetComponent<CanvasGroup>();

            sequence.AppendCallback(() =>
            {
                lines[index].SetActive(true);
                lineGroup.alpha = 0;
            });

            sequence.Append(lineGroup.DOFade(1, fadeDuration));
            sequence.AppendInterval(waitDuration);
            sequence.Append(lineGroup.DOFade(0, fadeDuration));

            sequence.AppendCallback(() =>
            {
                lines[index].SetActive(false);
            });
        }

        sequence.OnComplete(() =>
        {
            isRunningTutorial = false; // tutorial finito
            canvasGroup.gameObject.SetActive(false);
        });

        sequence.Play();
    }

    public void ShowIntro()
    {
        Debug.Log("Prima frase");
        Sequence sequence = DOTween.Sequence();

        sequence.AppendCallback(() =>
        {
            intro.gameObject.SetActive(true);
            intro.alpha = 0;
        });

        sequence.Append(intro.DOFade(1, fadeDuration));
        sequence.AppendInterval(waitDuration);
        sequence.Append(intro.DOFade(0, fadeDuration));

        sequence.AppendCallback(() =>
        {
            intro.gameObject.SetActive(false);
        });
        sequence.Play();
    }
}
