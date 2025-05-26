using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

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
    public CapsuleCollider2D collider2D;
    public Light2D luminescentLight;
    public Amo amo;
    public Transform climbTopLimit;
    public Transform climbBottomLimit;

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
    public bool isGrounded;

    [Header("GUSCI")]
    public int maxJump = 1;
    public int jumpCount = 0;
    [SerializeField] private bool canDash = false;
    private bool isDashing = false;
    public bool InLuminescenceZone = false;

    private float timeSinceLastJump = 0f;
    private float jumpResetBuffer = 0.1f; // ritardo prima di consentire il reset


    public static Player Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("🛑 Hai più di un Player nella scena!");
            Destroy(gameObject);
            return;
        }

        instance = this;

        shellManager = GetComponent<ShellManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        isGrounded = true;
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("xVelocity", 0f);
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
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
        }

        if (rb.linearVelocity.y > 0.1f && !isGrounded)
        {
            animator.SetBool("isJumping", true);
        }
        else if (rb.linearVelocity.y < -0.1f && !isGrounded)
        {
            animator.SetBool("isFalling", true);
        }
        else
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        CheckGrounded();
    }

    void Update()
    {
        Debug.Log($"jumpCount = {jumpCount}");

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
        if (amo.isAttached)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero; 

            Vector3 localPos = transform.localPosition;

            float newY = localPos.y + (verticalMovement * moveSpeed * Time.deltaTime);

            float topY = amo.currentClimbTopLimit.localPosition.y;
            float bottomY = amo.currentClimbBottomLimit.localPosition.y;
            newY = Mathf.Clamp(newY, bottomY, topY);

            transform.localPosition = new Vector3(0, newY, localPos.z);
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!canMove) { Debug.Log("Non posso muovermi"); return; }

        if (context.performed /* || Keyboard.current.spaceKey.isPressed */) 
        {
            if (jumpCount < maxJump)
            {
                if (amo.isAttached)
                {
                    amo.Detach();
                }
                
                jumpCount++;
                timeSinceLastJump = Time.time;
                Debug.Log($"Salto!, {jumpCount}");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower * 0.75f);
                animator.SetBool("isJumping", !isGrounded);
            }
        }
    }
    private void CheckGrounded()
    {
        bool grounded = Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0f, groundLayer);

        if (grounded || amo.isAttached)
        {
            Debug.Log("Sei a terra, azzero i salti");
            isGrounded = true;
            if (Time.time - timeSinceLastJump > jumpResetBuffer)
            {
                jumpCount = 0;
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            //Debug.Log("Non sei a terra");
            isGrounded = false;
        }
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
