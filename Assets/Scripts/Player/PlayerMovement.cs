using System.Collections;
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
    public bool isOnTopMedusa = false;

    [Header("GUSCIO SALTO")]
    public int maxJump = 1;
    public int jumpCount = 0;
    
    [Header("DASH")]
    [SerializeField] private float dashMultiplier = 2f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private bool dashOnCooldown = false;
    private float originalMoveSpeed;
    private float timeSinceLastJump = 0f;
    private float jumpResetBuffer = 0.1f;
    public bool canDash;

    [Header("GUSCIO LUMINOSO")]
    public bool InLuminescenceZone = false;
    public bool isInvisible = false;
    private PlayerInput playerInput;

    [Header("IMPULSO AMO")]
    [SerializeField] private float swingImpulseSpeed = 12f;
    [SerializeField] private float swingImpulseDuration = 0.3f;
    private bool isSwingImpulseActive = false;


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
        originalMoveSpeed = moveSpeed;
        playerInput = GetComponent<PlayerInput>();
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
        else
        {
            animator.SetBool("isJumping", false);
        }

        CheckGrounded();
    }


    void Update()
    {
        Debug.Log($"jumpCount = {jumpCount}");
        HandleCamouflageInput();
    }


    // FLIP DELLO SPRITE

    public void Flip(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;

        if (horizontalMovement > 0.01f)
        {
            spriteRenderer.flipX = false; // Guarda a destra
            spriteRendererShell.flipX = false;
        }
        else if (horizontalMovement < -0.01f)
        {
            spriteRenderer.flipX = true; // Guarda a sinistra
            spriteRendererShell.flipX = true;
        }
    }

    // MOVIMENTO
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

    // ARRAMPICATA SULL'AMO
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

    // IMPULSO AMO
    public void ApplySwingImpulse()
    {
        if (isSwingImpulseActive) return;
        Debug.Log("✅ Impulso da amo applicato");
        StartCoroutine(PerformSwingImpulse());
    }

    // IMPULSO AMO
    private IEnumerator PerformSwingImpulse()
    {
        isSwingImpulseActive = true;

        float direction = spriteRenderer.flipX ? -1f : 1f;
        float elapsed = 0f;

        while (elapsed < swingImpulseDuration)
        {
            rb.linearVelocity = new Vector2(direction * swingImpulseSpeed, rb.linearVelocity.y);
            elapsed += Time.deltaTime;
            yield return null;
        }

        isSwingImpulseActive = false;
    }

    // SALTO
    public void Jump(InputAction.CallbackContext context)
    {
        if (!canMove) { Debug.Log("Non posso muovermi"); return; }

        if (context.performed)
        {
            if (amo.isAttached)
            {
                amo.Detach();
            }

            if (jumpCount < maxJump)
            {
                jumpCount++;
                timeSinceLastJump = Time.time;
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower * 0.75f);
                animator.SetBool("isJumping", !isGrounded);
            }
        }
    }

    // CONTROLLO GROUNDED
    private void CheckGrounded()
    {
        bool grounded = Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0f, groundLayer);

        if (grounded || amo.isAttached)
        {
            isGrounded = true;
            if (Time.time - timeSinceLastJump > jumpResetBuffer)
            {
                jumpCount = 0;
                animator.SetBool("isJumping", false);
            }
        }
        else
        {
            isGrounded = false;
        }
    }

    // CONTROLLO DASH
    public void Dash(InputAction.CallbackContext context)
    {
        if (!canDash || isDashing || dashOnCooldown || !context.performed)
            return;

        StartCoroutine(PerformDash());
    }
    
    // DASH
    private IEnumerator PerformDash()
    {
        isDashing = true;
        dashOnCooldown = true;

        moveSpeed *= dashMultiplier;
        yield return new WaitForSeconds(dashDuration);

        moveSpeed = originalMoveSpeed;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        dashOnCooldown = false;
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

    public void Hide(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (shellManager.currentShellPicker.shell.name == "NascondiScava")
            {
                // LOGICA NASCONDI
            }
        }
    }

    public void Scava(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (shellManager.currentShellPicker.shell.name == "NascondiScava")
            {
                // LOGICA SCAVA
            }
        }
    }

    private void HandleCamouflageInput()
    {
        if (playerInput == null || shellManager == null) return;

        if (playerInput.Camouflage &&
           shellManager.currentShellPicker.shell.name == "Mimetico" &&
            !isInvisible)
        {
            PowerLibrary.MimeticoOn(this);
        }
    }
}
