using UnityEngine;

public class Fornello : MonoBehaviour
{
    public float tempoSopportabile = 5f;

    private float timer = 0f;
    private bool playerSopra = false;
    private LifeController playerLife;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerSopra = true;
            playerLife = other.GetComponent<LifeController>();
            timer = 0f; // reset del timer quando entra
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerSopra = false;
            timer = 0f; // reset del timer se esce
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
                timer = 0f; // reset per evitare danni multipli ogni frame
            }
        }
    }
}
