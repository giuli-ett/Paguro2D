using UnityEngine;

/// <summary>
/// Applies an upward force to the player and controls bubble particle emission,
/// with improved feel and consistent feedback.
/// </summary>
public class BolleNEW : MonoBehaviour
{
    [Header("Force Settings")]
    [SerializeField] private float force = 5f;
    [SerializeField] private AnimationCurve forceCurve = null;
    [SerializeField] private float liftDuration = 1f;
    //[SerializeField] private float maxForcePerFrame = 10f;

    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem bubbleParticles;
    [SerializeField] private float activeEmissionRate = 10f;
    [SerializeField] private float inactiveEmissionRate = 0f;

    private bool isActive = true;
    private bool playerInside = false;
    private float liftTimer = 0f;
    private ParticleSystem.EmissionModule emissionModule;
    private Rigidbody2D playerRb = null;

    void Start()
    {
        if (bubbleParticles != null)
        {
            emissionModule = bubbleParticles.emission;
            SetEmissionRate(activeEmissionRate);
            bubbleParticles.Play();
        }
        else
        {
            Debug.LogWarning("BubbleParticles not assigned in Bolle script.", this);
        }

        if (forceCurve == null)
        {
            // Default to ease in/out if not set in Inspector
            forceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        }
    }

    void Update()
    {
        if (playerInside && isActive)
        {
            float t = Mathf.Clamp01(liftTimer / liftDuration);
            SetEmissionRate(Mathf.Lerp(activeEmissionRate, activeEmissionRate * 2f, t));
        }
        else
        {
            SetEmissionRate(activeEmissionRate);
        }
    }

    void FixedUpdate()
    {
        if (playerInside && playerRb != null && isActive)
        {
            liftTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(liftTimer / liftDuration);
            float appliedForce = force * forceCurve.Evaluate(t);
            Debug.Log($"inspector force: {force}, appliedForce: {appliedForce}");
            playerRb.AddForce(Vector2.up * appliedForce, ForceMode2D.Force);
        }
        else
        {
            liftTimer = 0f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive || other == null) return;

        if (other.CompareTag("Player"))
        {
            playerInside = true;
            liftTimer = 0f;
            playerRb = other.GetComponent<Rigidbody2D>();
            AudioManager.Instance.PlayBubbles();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            liftTimer = 0f;
            playerRb = null;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        // No force logic here anymore; handled in FixedUpdate for consistency
    }

    private void SetEmissionRate(float rate)
    {
        if (bubbleParticles != null)
        {
            var emission = bubbleParticles.emission;
            var rateOverTime = emission.rateOverTime;
            rateOverTime.constant = rate;
            emission.rateOverTime = rateOverTime;
        }
    }

    /// <summary>
    /// Activates the bubble effect.
    /// </summary>
    public void Activate()
    {
        isActive = true;
        SetEmissionRate(activeEmissionRate);
        if (bubbleParticles != null && !bubbleParticles.isPlaying)
            bubbleParticles.Play();
    }

    /// <summary>
    /// Deactivates the bubble effect.
    /// </summary>
    public void Deactivate()
    {
        isActive = false;
        SetEmissionRate(inactiveEmissionRate);
        if (bubbleParticles != null && bubbleParticles.isPlaying)
            bubbleParticles.Stop();
    }
}