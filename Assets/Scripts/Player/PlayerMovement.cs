using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Player : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    private static Player instance;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteRendererShell;
    public Rigidbody2D rb;
    public ShellManager shellManager;
    public InventarioUI inventarioUI;
    public UIController uIController;
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
    public bool isInForno = false;
    private Vector3 externalPlatformDelta = Vector3.zero;
    private bool isOnMovingPlatform = false;
    private Vector2 platformVelocity = Vector2.zero;
    private Vector2 smoothedPlatformVelocity = Vector2.zero;
    public float targetSpeed;
    public bool wasOnMovingPlatformLastFrame = false;
    [SerializeField] private float platformLerpSpeed = 10f;

    [Header("SALTO")]
    public float jumpPower = 18f;
    public Transform groundCheckPosition;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;
    public bool isGrounded;
    public bool isOnTopMedusa = false;
    private bool isJumpingHeld = false;

    [Header("FLIP GUSCIO")]
    [SerializeField] private Transform shellPositionTransform;
    [SerializeField] private float shellOffsetX = 1.8f;
    [SerializeField] private float shellRotationZ = 10f;

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

    [Header("GUSCIO SALTO")]
    public int maxJump = 1;
    public int jumpCount = 0;

    [Header("GUSCIO LUMINOSO")]
    public bool InLuminescenceZone = false;
    public bool isInvisible = false;
    public float lightDuration = 10f;
    public Coroutine lightFadeCoroutine;
    private PlayerInput playerInput;


    [Header("GUSCIO NASCONDI/SCAVA")]
    [SerializeField] private float digRange = 5f;
    [SerializeField] private LayerMask diggableLayer;
    public bool canDig = false;
    public bool canHide = false;
    public bool isHiding = false;

    [Header("IMPULSO AMO")]
    [SerializeField] private float swingImpulseSpeed = 12f;
    [SerializeField] private float swingImpulseDuration = 0.3f;
    private bool isSwingImpulseActive = false;
    private float lastYPosition;

    [Header("BOSSFIGHT")]

    public int escapeAttempts = 0;
    public int requiredPresses = 5;
    public bool isGrabbed = false;

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
        DontDestroyOnLoad(gameObject);

        shellManager = GetComponent<ShellManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        amo = GetComponent<Amo>();
        isGrounded = true;
        originalMoveSpeed = moveSpeed;
        playerInput = GetComponent<PlayerInput>();

        lastYPosition = transform.position.y;
    }

    private void FixedUpdate()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetFloat("xVelocity", 0f);
            return;
        }
        smoothedPlatformVelocity = Vector2.Lerp(smoothedPlatformVelocity, platformVelocity, Time.fixedDeltaTime * platformLerpSpeed);

        if (amo.isAttached)
        {
            Climb();
            animator.SetFloat("yClimbVelocity", verticalMovement);
        }
        else
        {
            rb.gravityScale = 6.0f;
            MovePlayer();
            float velocityInput = Mathf.Abs(horizontalMovement);
            animator.SetFloat("xVelocity", velocityInput);
            animator.SetFloat("yJumpVelocity", rb.linearVelocity.y);
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
        //Debug.Log($"jumpCount = {jumpCount}");
        HandleCamouflageInput();
    }


    // FLIP DELLO SPRITE
    public void Move(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        horizontalMovement = context.ReadValue<Vector2>().x;
        verticalMovement = context.ReadValue<Vector2>().y;

        if (horizontalMovement > 0.01f)
        {
            // Guarda a destra
            spriteRenderer.flipX = false;
            spriteRendererShell.flipX = false;

            shellPositionTransform.localPosition = new Vector3(shellOffsetX, shellPositionTransform.localPosition.y, shellPositionTransform.localPosition.z);
            shellPositionTransform.localEulerAngles = new Vector3(0, 0, shellRotationZ);
        }
        else if (horizontalMovement < -0.01f)
        {
            // Guarda a sinistra
            spriteRenderer.flipX = true;
            spriteRendererShell.flipX = true;

            shellPositionTransform.localPosition = new Vector3(-shellOffsetX, shellPositionTransform.localPosition.y, shellPositionTransform.localPosition.z);
            shellPositionTransform.localEulerAngles = new Vector3(0, 0, -shellRotationZ);
        }
    }

    // MOVIMENTO
    private void MovePlayer()
    {
        if (isClimbing) return;

        else
        {
            targetSpeed = horizontalMovement * moveSpeed + smoothedPlatformVelocity.x;
            float speedDifference = targetSpeed - rb.linearVelocity.x;
            float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

            float movement = speedDifference * accelRate;

            rb.AddForce(new Vector2(movement, 0));
        }
    }
    // SET VELOCITÀ PIATTAFORMA MOBILE
    public void SetPlatformVelocity(Vector3 velocity, bool instant = false)
    {
         platformVelocity = velocity;
        if (instant)
        smoothedPlatformVelocity = velocity;
    }

    // ARRAMPICATA SULL'AMO
    private void Climb()
    {
        if (amo.isAttached)
        {
            animator.SetBool("isAttached", true);
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
        UnityEngine.Debug.Log("✅ Impulso da amo applicato");
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
        if (!canMove) return;

        if (context.started)
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
                isJumpingHeld = true; // Tasto appena premuto
                animator.SetBool("isJumping", !isGrounded);
            }
        }

        if (context.canceled)
        {
            isJumpingHeld = false; // Tasto rilasciato
            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f); // taglia il salto
            }
        }
    }


    public void EscapeGrab(InputAction.CallbackContext context)
    {
        if (context.performed && isGrabbed)
        {
            if (escapeAttempts >= requiredPresses)
            {
                Debug.Log("Il paguro si è liberato!");
                return;
            }

            escapeAttempts++;
            Debug.Log($"Tentativi di fuga: {escapeAttempts}/{requiredPresses}");
        }
    }

    // CONTROLLO GROUNDED
    private void CheckGrounded()
    {
        int combinedLayerMask = groundLayer | diggableLayer;
        bool grounded = Physics2D.OverlapBox(groundCheckPosition.position, groundCheckSize, 0f, combinedLayerMask);

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

    // HIDE
    public void Hide(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (shellManager.currentShellPicker.shell.name != "NascondiScava") return;

        if (canHide)
        {
            if (!isHiding) // Hai premuto per entrare nel guscio
            {
                isHiding = true;
                canMove = false;
                rb.linearVelocity = Vector2.zero;
                this.spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);
            }
            else // Hai premuto per uscire dal guscio
            {
                isHiding = false;
                canMove = true;
                spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
        }

    }

    // SCAVA
    public void Scava(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        if (shellManager.currentShellPicker.shell.name != "NascondiScava") return;

        if (canDig)
        {
            Vector2 origin = transform.position;
            Vector2 direction;

            if (verticalMovement < -0.5f)
            {
                direction = Vector2.down;
            }
             else if (verticalMovement > 0.5f)
            {
                direction = Vector2.up;
            }
            else
            {
                direction = spriteRenderer.flipX ? Vector2.left : Vector2.right;
            }

            RaycastHit2D hit = Physics2D.Raycast(origin, direction, digRange, diggableLayer);
            Debug.DrawRay(origin, direction * digRange, Color.red, 5f);

            if (hit.collider != null)
            {
                StartCoroutine(DigAnimationCoroutine());
                var block = hit.collider.gameObject;
                var blockSprite = block.GetComponent<SpriteRenderer>();
                var blockCollider = block.GetComponent<Collider2D>();
                if (blockSprite != null) blockSprite.enabled = false;
                if (blockCollider != null) blockCollider.enabled = false;
                Debug.Log("✅ Blocco scavato in direzione: " + direction);
            }
            else
            {
                animator.SetBool("isDigging", false);
                Debug.Log("❌ Nessun blocco scavabile in direzione: " + direction);
            }
        }
    }

        private IEnumerator DigAnimationCoroutine()
    {
        animator.SetBool("isDigging", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("isDigging", false);
    }
                

            

    // GUSCIO MIMETICO
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

    // FUNZIONI ATTIVA/DISATTIVA FUNZIONALITA' DEL GUSCIO
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

    public void DisableDash()
    {
        canDash = false;
        isDashing = false;
    }

    public void EnableNascondiScava()
    {
        canHide = true;
        canDig = true;
    }

    public void DisableNascondiScava()
    {
        canHide = false;
        canDig = false;
    }

    // GESTIONE PIATTAFORME MOBILI
    public void SetExternalMovement(Vector3 delta)
    {
        externalPlatformDelta = delta;
        isOnMovingPlatform = true;
    }
    private void LateUpdate()
    {
        if (isOnMovingPlatform)
        {
            rb.MovePosition(rb.position + (Vector2)externalPlatformDelta);
            externalPlatformDelta = Vector3.zero;
            isOnMovingPlatform = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("TriggerWASD"))
        {
            Debug.Log("Trigger WASD");
            StartCoroutine(uIController.FadeInAndOut(uIController.bollaWASD));
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else if (other.CompareTag("TriggerSPACE"))
        {
            Debug.Log("Trigger SPACE");
            StartCoroutine(uIController.FadeInAndOut(uIController.bollaSPACE));
            other.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    public void DisableMovement()
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    public void EnableMovement()
    {
        canMove = true;
        horizontalMovement = 0f;
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
    }
}
