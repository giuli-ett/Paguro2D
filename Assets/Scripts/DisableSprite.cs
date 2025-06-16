using UnityEngine;

public class DisableSprite : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float fadeDuration = 1f;
    private bool isFading = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isFading)
        {
            if (spriteRenderer != null)
                StartCoroutine(FadeOut());
        }
    }

    private System.Collections.IEnumerator FadeOut()
    {
        isFading = true;
        float startAlpha = spriteRenderer.color.a;
        float elapsed = 0f;
        Color color = spriteRenderer.color;

        while (elapsed < fadeDuration)
        {
            float newAlpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            spriteRenderer.color = new Color(color.r, color.g, color.b, newAlpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        spriteRenderer.color = new Color(color.r, color.g, color.b, 0f);
        spriteRenderer.enabled = false;
        isFading = false;
    }
}