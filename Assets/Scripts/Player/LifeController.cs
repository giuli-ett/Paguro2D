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

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger con: " + other.name);
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
        }

        if (other.CompareTag("HealthPickup"))
        {
            GainHealth();
            Destroy(other.gameObject); // Rimuove il cuore dalla scena
        }

        if (other.CompareTag("Checkpoint"))
        {
            respawnPosition = other.transform.position;
            Debug.Log("Checkpoint raggiunto!");
        }
    }


    /* void Update()
    {
        
    } */
}
