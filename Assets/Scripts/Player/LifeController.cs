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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    private void Die()
    {
        Debug.Log("Giocatore morto!");
        currentHealth = maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].SetActive(true);
        }

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
            StartCoroutine(InvincibilityCoroutine());
        }
    }

    public void GainHealth()
    {
        if (currentHealth < maxHealth)
        {
            hearts[currentHealth].SetActive(true); // Riattiva il cuore
            currentHealth++;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Trigger con: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Enemy"))
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


    /* void Update()
    {
        
    } */
}
