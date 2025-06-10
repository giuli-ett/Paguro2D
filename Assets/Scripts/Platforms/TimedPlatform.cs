using UnityEngine;

public class TimedPlatform : MonoBehaviour
{
    [SerializeField] private float durata = 2f;
    private float timer = 0f;
    [SerializeField] private float cooldown = 0.5f;
    private bool playerSopra = false;
    private Animator animator;
    private Collider2D collider;
    private Renderer renderer;
    void Start()
    {
        //animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        renderer = GetComponent<Renderer>();
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerSopra = true;
            timer = 0f;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerSopra = false;
            timer = 0f;
        }
    }

    void CheckTimer()
    {
        if (playerSopra)
        {
            timer += Time.deltaTime;

            if (timer >= durata)
            {
                collider.enabled = false;
                renderer.enabled = false; 
                //animazione
                timer = 0f;
            }
        }
    }
    void Cooldown()
    {
        if (!collider.enabled)
        {
            timer += Time.deltaTime;

            if (timer >= cooldown)
            {
                collider.enabled = true;
                renderer.enabled = true;
                //animazione
                timer = 0f;
            }
        }
    }

    void Update()
    {
        CheckTimer();
        Cooldown(); 
    }

}
