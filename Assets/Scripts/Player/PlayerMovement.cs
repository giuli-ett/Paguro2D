using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    private static Player instance;
    private SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererShell;
    public Rigidbody2D rb;
    public ShellManager shellManager;
    public InventarioUI inventarioUI;
    public Animator animator;

    [Header("MOVIMENTO")]
    public float moveSpeed = 5f;
    public float acceleration = 10f;         
    public float deceleration = 20f;        
    float horizontalMovement;
    float verticalMovement;
    public bool canMove = true;
    public bool isClimbing = false;

    [Header("SALTO")]
    public float jumpPower = 18f;
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;

    [Header ("GUSCI")]
    private int maxJump = 1;
    private int jumpCount = 0;
    [SerializeField] private bool canDash = false;
    private bool isDashing = false;

    public static Player Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        instance = this;
        shellManager = GetComponent<ShellManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        BoxCollider2D box = GetComponent<BoxCollider2D>();

        groundCheckSize = new Vector2(box.size.x * 0.9f, 0.1f);
    }

    private void FixedUpdate()
    {
        if (!canMove) 
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("xVelocity", 0f);
            GroundCheck();
            return;
        }

        if (this.GetComponent<Amo>().isAttached)
        {
            Climb();
            animator.SetFloat("xVelocity", 0f);
        }
        else
        {
            rb.gravityScale = 6.0f;
            MovePlayer();
            float velocityInput = Mathf.Abs(horizontalMovement);
            animator.SetFloat("xVelocity", velocityInput);

            //animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
        }
        
        GroundCheck();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;

        if (horizontalMovement > 0.01f)
        {
            spriteRenderer.flipX = false; // Guarda a destra
            if (shellManager.currentShellPicker != null)
            {
                spriteRendererShell.flipX = false; // Guarda a destra
            }
        }
        else if (horizontalMovement < -0.01f)
        {
            spriteRenderer.flipX = true; // Guarda a sinistra
            if (shellManager.currentShellPicker != null)
            {
                spriteRendererShell.flipX = true; // Guarda a destra
            }
        }
    }

    private void MovePlayer()
    {
        if (isClimbing) return;

        else
        {
            float targetSpeed = horizontalMovement * moveSpeed;
            float speedDifference = targetSpeed - rb.linearVelocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

            float movement = speedDifference * accelRate;

            rb.AddForce(new Vector2(movement, 0));
        }
    }

    private void Climb()
    {
        if (this.GetComponent<Amo>().isAttached)
        {
            if (AmoHasBounds(out Bounds bounds))
            {
                Vector3 playerPos = transform.position;

                if (verticalMovement > 0 && playerPos.y + 0.5f >= bounds.max.y)
                {
                    rb.linearVelocity = Vector2.zero;
                    return;
                }

                if (verticalMovement < 0 && playerPos.y - 0.5f <= bounds.min.y)
                {
                    rb.linearVelocity = Vector2.zero;
                    return;
                }
            }
            rb.gravityScale = 0f;
            float climbVelocity = verticalMovement * moveSpeed;
            rb.linearVelocity = new Vector2(0, climbVelocity);
        }
    }

    private bool AmoHasBounds(out Bounds bounds)
    {
        bounds = default;

        if (TryGetComponent<Amo>(out Amo amo))
        {
            if (amo.isAttached && amo.currentClimbableCollider != null)
            {
                bounds = amo.currentClimbableCollider.bounds;
                return true;
            }
        }

        return false;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!canMove) { Debug.Log("Non posso muovermi"); return; }

        if (jumpCount < maxJump)
        {
            if (context.performed)
            {
                Debug.Log("Salto!");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower*0.75f);
                jumpCount++;
            }
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                jumpCount++;
            }
        }
    }

    /*
    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0, groundLayer))
        {
            Debug.Log("Tocco terra!");
            jumpCount = 0;
        }
        else
        {
            Debug.Log($"Numero di salti: {jumpCount}");
            Debug.Log("NON tocco terra");
        }
    }
    */
    
    private void GroundCheck()
    {
        Collider2D hit = Physics2D.OverlapBox(
            groundCheckPosition.position,
            groundCheckSize,
            0f,
            groundLayer
        );

        if (hit != null)
        {
            Debug.Log("✅ Tocco terra: " + hit.name);
            jumpCount = 0;
        }
        else
        {
            Debug.Log("❌ NON tocco terra");
        }
    }

    private void OnDrawGizmos()
    {
        if (groundCheckPosition == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckPosition.position, groundCheckSize);
    }

    public void EnableDoubleJump()
    {
        maxJump = 2;
    }

    public void DisableDoubleJump()
    {
        maxJump = 1;
    }

    public void EnableDash()
    {
        canDash = true;
    }

    public void DisableDush()
    {
        canDash = false;
        isDashing = false;
    }
}
