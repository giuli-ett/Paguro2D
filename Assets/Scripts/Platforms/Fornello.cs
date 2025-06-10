using UnityEngine;

using UnityEngine;

public class Fornello : MonoBehaviour
{
    public float tempoSopportabile = 2f;

    private float timer = 0f;
    private bool playerSopra = false;
    private LifeController playerLife;
    private Animator animator;

    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerSopra = true;
            playerLife = other.GetComponent<LifeController>();
            timer = 0f; // Reset del timer

            // Attiva la fiamma
            animator.SetInteger("acceso", 1);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerSopra = false;
            timer = 0f; // Reset del timer

            // Spegne la fiamma
            animator.SetInteger("acceso", 0);
            Debug.Log("Animazione tornata a idle");

        }
    }

    void Update()
    {
        if (playerSopra && playerLife != null)
        {
            timer += Time.deltaTime;

            if (timer >= tempoSopportabile)
            {
                playerLife.TakeDamage();
                timer = 0f; // Reset per evitare danni multipli ogni frame
            }
        }
    }
}