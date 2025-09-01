using UnityEngine;
using System.Collections;

public class TimedPlatform : MonoBehaviour
{
    [Header("Platform Settings")]
    [SerializeField] private float breakTime = 2f;
    [SerializeField] private float respawnTime = 0.5f;
    [SerializeField] private float shakeIntensity = 0.1f;
    
    [Header("Optional Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private ParticleSystem breakEffect;

    private bool isBreaking;
    private bool isRespawning;
    private Vector3 originalPosition;
    private Collider2D platformCollider;
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        platformCollider = GetComponent<Collider2D>();
        originalPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player") || isBreaking) return;

        AudioManager.Instance.PlayCrack();
        StartBreaking();
    }

    private void StartBreaking()
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        isBreaking = true;
        shakeCoroutine = StartCoroutine(ShakePlatform());
        StartCoroutine(BreakPlatformAfterDelay());
    }

    private IEnumerator ShakePlatform()
    {
        while (isBreaking)
        {
            transform.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
            yield return new WaitForSeconds(0.05f);
        }
        transform.position = originalPosition;
    }

    private IEnumerator BreakPlatformAfterDelay()
    {
        yield return new WaitForSeconds(breakTime);
        
        // Break platform
        platformCollider.enabled = false;
        SetChildrenSpriteRenderers(false);
        
        if (breakEffect != null)
            breakEffect.Play();
            
        if (animator != null)
            animator.SetTrigger("Break");

        AudioManager.Instance.StopCrack();
        isBreaking = false;

        // Start respawn countdown
        StartCoroutine(RespawnPlatform());
    }

    private IEnumerator RespawnPlatform()
    {
        isRespawning = true;
        yield return new WaitForSeconds(respawnTime);

        // Respawn platform
        platformCollider.enabled = true;
        SetChildrenSpriteRenderers(true);
        
        if (animator != null)
            animator.SetTrigger("Respawn");
            
        isRespawning = false;
    }

    private void SetChildrenSpriteRenderers(bool enabled)
    {
        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = enabled;
        }
    }
}