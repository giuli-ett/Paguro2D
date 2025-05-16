using UnityEngine;

public class Bolle : MonoBehaviour
{
    [SerializeField] float forza = 5f;

    public ParticleSystem bubbleParticles;
    [SerializeField] float emissionRateWhenActive = 10f;
    private float emissionRateWhenInactive = 0f;

    private bool isActive = true;
    private float timer;
    private ParticleSystem.EmissionModule emissionModule;

    void Start()
    {
        if (bubbleParticles != null)
        {
            emissionModule = bubbleParticles.emission;
            SetEmissionRate(emissionRateWhenActive);
            bubbleParticles.Play();
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!isActive) return;

        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(Vector2.up * forza, ForceMode2D.Force);
            }
        }
    }

    void SetEmissionRate(float rate)
    {
        if (bubbleParticles != null)
        {
            var emission = bubbleParticles.emission;
            var rateOverTime = emission.rateOverTime;
            rateOverTime.constant = rate;
            emission.rateOverTime = rateOverTime;
        }
    }

}
