using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LifeController : MonoBehaviour
{

    [Header("VITE")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;
    private bool isInvincible = false;
    [SerializeField] private float invincibilityTime = 1.0f;
    [SerializeField] private GameObject[] hearts;
    private Vector3 respawnPosition;

    [SerializeField] private Palla palla;

    [Header("DANNO")]
    [SerializeField] private float damageImpulseSpeed = 12f;
    [SerializeField] private float damageImpulseDuration = 0.3f;
    public bool isDamageImpulseActive = false;

    void Awake()
    {
        currentHealth = maxHealth;
        respawnPosition = transform.position;
    }

    // gestione vite
    private IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityTime);
        isInvincible = false;
    }

    public void Die()
    {
        //Player.Instance.animator.SetBool("isDead", true);
        Debug.Log("Giocatore morto!");
        currentHealth = maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(true);
        }

        if (palla != null)
        {
            palla.ResetPosition();
        }

        StartCoroutine(WaitABit());
        transform.position = respawnPosition;
        StartCoroutine(InvincibilityCoroutine());
    }

    public void TakeDamage()
    {
        if (isInvincible)
            return;

        currentHealth--;
        if (currentHealth < hearts.Length && currentHealth >= 0)
        {
            hearts[currentHealth].SetActive(false);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ApplayDamageImpulse();
            Player.Instance.animator.SetBool("isTakingDamage", true);
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void GainHealth()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            hearts[currentHealth - 1].SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Trigger con: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage();
            
        }
        if (other.gameObject.CompareTag("Hand") && Hand.Instance.isSmashing)
        {
            TakeDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Checkpoint"))
        {
            respawnPosition = other.transform.position;
            Debug.Log("Checkpoint raggiunto!");
        }

        if (other.CompareTag("HealthPickup"))
        {
            GainHealth();
            Destroy(other.gameObject);
        }
    }


    public void ApplayDamageImpulse()
    {
        if (isDamageImpulseActive) return;
        UnityEngine.Debug.Log("✅ Impulso da namico applicato");
        StartCoroutine(PerformDamegeImpulse());
    }

    private IEnumerator PerformDamegeImpulse()
    {
        isDamageImpulseActive = true;

        float direction = Player.Instance.spriteRenderer.flipX ? 1f : -1f;
        float elapsed = 0f;

        while (elapsed < damageImpulseDuration)
        {
            Player.Instance.rb.linearVelocity = new Vector2(direction * damageImpulseSpeed, Player.Instance.rb.linearVelocity.y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isDamageImpulseActive = false;
    }

    public void OnDamageAnimationOver()
    {
        Player.Instance.animator.SetBool("isTakingDamage", false);
    }

    private IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(1.0f);
    }

    public void OnDeathAnimationOver()
    {
        Debug.Log("Fine animazione morte");
        Player.Instance.animator.SetBool("isDead", false);
    }
}
