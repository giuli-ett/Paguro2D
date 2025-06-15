using UnityEngine;

public class BancoPesci : MonoBehaviour
{
    public Transform[] waypoints;
    [SerializeField] private float speed = 2f;
    private int currentIndex = 0;

    private Vector3 lastPosition;
    private SpriteRenderer spriteRenderer;
    private GameObject playerOnTop;
    private Player player;

    private bool isActivated = false;

    void Start()
    {
        lastPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isActivated || waypoints.Length == 0) return;

         // Calculate velocity BEFORE moving
    Vector3 currentPosition = transform.position;

    // Move towards the current waypoint
    Transform target = waypoints[currentIndex];
    Vector3 newPosition = Vector3.MoveTowards(currentPosition, target.position, speed * Time.fixedDeltaTime);
    Vector2 platformVelocity = (newPosition - currentPosition) / Time.fixedDeltaTime;

    transform.position = newPosition;

        // Flip sprite based on direction
        if (platformVelocity.x != 0)
        {
            spriteRenderer.flipX = platformVelocity.x < 0;
        }

        // Switch to next waypoint if close enough
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }

        // Move player with platform
        if (playerOnTop != null)
        {
            player = playerOnTop.GetComponent<Player>();
            if (player != null && player.isGrounded)
            {
                player.SetPlatformVelocity(platformVelocity);
            }
            else if (player != null && !player.isGrounded)
            {
                player.SetPlatformVelocity(Vector2.zero);
            }
        }

        lastPosition = transform.position;
    }

    public Vector3 GetDeltaMovement()
    {
        return transform.position - lastPosition;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in other.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    isActivated = true;
                    playerOnTop = other.gameObject;
                    break;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && playerOnTop == other.gameObject)
        {
            if (player != null)
            {
                player.SetPlatformVelocity(Vector2.zero);
            }
            playerOnTop = null;
        }
    }
}