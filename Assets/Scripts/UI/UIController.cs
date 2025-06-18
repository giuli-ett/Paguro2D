using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public CanvasGroup bollaWASD;
    public CanvasGroup bollaSPACE;
    public CanvasGroup bollaDoppioSalto;
    public CanvasGroup bollaLuminescente;
    public CanvasGroup bollaDash;
    public CanvasGroup bollaNascondiScava;
    public float fadeDuration = 0.5f;
    public float visibleDuration = 1f;

    void Awake()
    {
        bollaWASD.gameObject.SetActive(false);
        bollaSPACE.gameObject.SetActive(false);
        bollaDoppioSalto.gameObject.SetActive(false);
        bollaLuminescente.gameObject.SetActive(false);
        bollaDash.gameObject.SetActive(false);
        bollaNascondiScava.gameObject.SetActive(false);
    }

    public IEnumerator FadeIn(CanvasGroup group)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            group.alpha = Mathf.Lerp(0, 1, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        group.alpha = 1;
        group.gameObject.SetActive(true);
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

    public IEnumerator FadeInAndOut(CanvasGroup group)
    {
        group.gameObject.SetActive(true);
        float t = 0;

        // Fade In
        while (t < fadeDuration)
        {
            group.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        group.alpha = 1f;

        // Stay visible
        yield return new WaitForSeconds(visibleDuration);

        // Fade Out
        t = 0;
        while (t < fadeDuration)
        {
            group.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        group.alpha = 0f;
        group.gameObject.SetActive(false);
    }

}

