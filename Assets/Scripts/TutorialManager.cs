using System.Collections;
using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public CanvasGroup canvas;
    public SpriteMask spriteMask;
    public GameObject darkOverlay;
    public bool tutorialMode = false;
    public float fadeDuration = 0.5f;

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
        tutorialMode = true;
        canvas.gameObject.SetActive(false);
        spriteMask.gameObject.SetActive(false);
        darkOverlay.gameObject.SetActive(false);
    }

    public void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            tutorialMode = true;
            darkOverlay.gameObject.SetActive(true);
            spriteMask.gameObject.SetActive(true);

            Player.Instance.canMove = false;
            Player.Instance.audioManager.StopWalking();

            Sequence mostraTesto = DOTween.Sequence();
            mostraTesto.AppendInterval(2f);

            mostraTesto.OnComplete(() =>
            {
                canvas.gameObject.SetActive(true);

                Sequence mostraInterazione = DOTween.Sequence();
                mostraInterazione.AppendInterval(2f);

                mostraInterazione.OnComplete(() =>
                {
                    canvas.gameObject.SetActive(false);
                    this.GetComponent<BoxCollider2D>().enabled = false;
                    Player.Instance.GetComponent<ForziereController>().closeForziere.GetComponent<Forziere>().text.SetActive(true);
                    tutorialMode = false;
                    //StartCoroutine(FadeOut(canvas));
                    StartCoroutine(FadeOutSprite(darkOverlay.GetComponent<SpriteRenderer>()));
                    spriteMask.gameObject.SetActive(false);
                    Player.Instance.EnableMovement();
                });
                mostraInterazione.Play();
            });
            mostraTesto.Play();
        }
    }

    public IEnumerator FadeOutSprite(SpriteRenderer spriteRenderer)
    {
        float t = 0f;
        Color originalColor = spriteRenderer.color;

        while (t < fadeDuration)
        {
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            t += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        spriteRenderer.gameObject.SetActive(false);
    }

    public IEnumerator FadeOut(CanvasGroup group)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            group.alpha = Mathf.Lerp(1, 0, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        group.alpha = 0;
        group.gameObject.SetActive(false);
    }
}
