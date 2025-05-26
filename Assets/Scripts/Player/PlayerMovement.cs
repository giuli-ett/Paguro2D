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
    private int maxJump = 1;
    public int jumpCount = 0;
    [SerializeField] private bool canDash = false;
    private bool isDashing = false;
    public bool InLuminescenceZone = false;

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

        if (!isGrounded && rb.linearVelocity.x != 0f)
        {
            animator.SetFloat("xVelocity", 0f);
            
        }

        CheckGrounded();
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
            // Blocca la fisica
            rb.gravityScale = 0f;
            rb.linearVelocity = Vector2.zero; // non usare linearVelocity se Rigidbody2D

            // Prendi posizione locale rispetto all’amo
            Vector3 localPos = transform.localPosition;

            // Calcola nuova posizione locale lungo l’asse Y (cioè su/giù lungo la corda)
            float newY = localPos.y + (verticalMovement * moveSpeed * Time.deltaTime);

            // Applica limiti locali
            float topY = amo.currentClimbTopLimit.localPosition.y;
            float bottomY = amo.currentClimbBottomLimit.localPosition.y;
            newY = Mathf.Clamp(newY, bottomY, topY);

            // Applica nuova posizione, mantenendo X e Z locali
            transform.localPosition = new Vector3(0, newY, localPos.z);
        }
    }


    /*
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
    */

    public void Jump(InputAction.CallbackContext context)
    {
        if (!canMove) { Debug.Log("Non posso muovermi"); return; }

        if (jumpCount < maxJump)
        {
            if (context.performed || Keyboard.current.spaceKey.isPressed)
            {
                if (amo.isAttached)
                {
                    amo.Detach();
                }
                Debug.Log("Salto!");
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower * 0.75f);
                isGrounded = false;
                animator.SetBool("isJumping", !isGrounded);
                jumpCount++;
            }
            /*
            else if (context.canceled)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                isGrounded = false;
                animator.SetBool("isJumping", !isGrounded);
                jumpCount++;
            }
            */
        }
    }
    private void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0f, groundLayer);

        if (isGrounded || amo.isAttached)
        {
            jumpCount = 0;
            animator.SetBool("isJumping", false);
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
