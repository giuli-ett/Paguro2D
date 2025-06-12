using UnityEngine;
using UnityEngine.InputSystem;

public class Tartaruga : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    private Vector3 startPosition;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Player player;

    [Header("VARIABILI")]
    [SerializeField] private float velocita = 2f;
    [SerializeField] private float distanza = 1.2f;

  

    private float lastX;
    private Vector3 lastPosition;
    private GameObject playerOnTop;

    void Start()
    {
        startPosition = transform.position;
        animator = GetComponent<Animator>();
        animator.SetFloat("Schiacciato", 0f);
        lastX = startPosition.x;
        lastPosition = transform.position;
    }

    private void Move()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * velocita) * distanza;
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);

        float deltaX = newX - lastX;

        // Flip sprite in base alla direzione
        if (deltaX > 0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (deltaX < -0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        lastX = newX;
    }

    void FixedUpdate()
    {
        Move();
        Vector2 platformVelocity = new Vector2((transform.position - lastPosition).x / Time.fixedDeltaTime, 0f);

        if (playerOnTop != null)
        {
            player = playerOnTop.GetComponent<Player>();
            if (player != null && player.isGrounded)
            {
                player.SetPlatformVelocity(platformVelocity);
            }
            else if(player !=null && !player.isGrounded)
            {
                player.SetPlatformVelocity(Vector2.zero);
            }

            lastPosition = transform.position;
        }

        lastPosition = transform.position;
    }

    public void SetSchiacciato(bool valore)
    {
        animator.SetFloat("Schiacciato", valore ? 1f : 0f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f) // Contatto dall'alto
                {
                    playerOnTop = collision.gameObject;
                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && playerOnTop == collision.gameObject)
        {
            if (player != null)
        {
            player.SetPlatformVelocity(Vector2.zero);
        }
            playerOnTop = null;
        }
    }
}