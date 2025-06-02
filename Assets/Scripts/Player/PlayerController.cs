using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOld : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    private static Player instance;
    private PlayerInput input;
    public ShellManager shellManager;
    public InventarioUI inventarioUI;

    [Header("MOVIMENTO")]
    [SerializeField] private bool canMove = true;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float runSpeed = 13f;
    private float verticalSpeed;
    private float currentMovementSpeed;
    private float boundingBoxWidth = 0.1f;

    [Header("SALTO")]
    [SerializeField] private bool isGrounded = true; 
    [SerializeField] private float groundDistance = 1.0f;
    [SerializeField] private float ceilingDistance = 1.0f;
    [SerializeField] private float gravity = 50f; 
    [SerializeField] private float jumpPower = 25f;
    [SerializeField] private float terminalSpeed = -100f;

    [Header("POTERI")]
    private int jumpCount = 0;
    private int maxJump = 1;
    [SerializeField] private bool canDash = false;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTimeLeft = 1.0f;
    

    public static Player Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Esiste già un'istanza di Player!");
            Destroy(gameObject);
            return;
        }
        //instance = this;

        input = GetComponent<PlayerInput>();
        shellManager = GetComponent<ShellManager>();
}

    private void Update()
    {
        CheckGrounded();
        Jump();
        CheckCeiling();
        Fall();
        Move();
        CheckWalls();
    }

    private void Move()
    {
        if (!canMove)
            return;

        if (input.Horizontal > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);  
        }
        else if (input.Horizontal < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);  
        }

        inventarioUI.transform.rotation = Quaternion.Euler(0, 0, 0);

        if (isDashing)
        {
            currentMovementSpeed = dashSpeed * input.Horizontal;
            transform.position += new Vector3(currentMovementSpeed * Time.deltaTime, 0f, 0f);
            dashTimeLeft -= Time.deltaTime;

            if (dashTimeLeft <= 0)
                isDashing = false;

            return;
        }

        if (input.Dash && canDash && !isDashing)
        {
            StartDash();
            return;
        }

        if (input.Run)
        {
            currentMovementSpeed = runSpeed * input.Horizontal;
        }
        else
        {
            currentMovementSpeed = moveSpeed * input.Horizontal;
        }

        transform.position += new Vector3(currentMovementSpeed * Time.deltaTime, 0f, 0f);
    }

    private void Jump()
    {
        if (input.Jump && jumpCount < maxJump)
        {
            verticalSpeed = jumpPower;
            isGrounded = false;
            jumpCount++;
        }
    }

    private void Fall()
    {
        if (isGrounded)
            return;

        if (verticalSpeed > terminalSpeed)
        {
            verticalSpeed -= gravity * Time.deltaTime;

            if (verticalSpeed <= terminalSpeed)
                verticalSpeed = terminalSpeed;
        }

        transform.position += new Vector3(0f, verticalSpeed * Time.deltaTime, 0f);
    }

    private void CheckGrounded()
    {
        if (verticalSpeed > 0)
            return;

        Vector2 position = transform.position;
        Vector2 boxSize = new Vector2(0.9f, 0.2f); // Larghezza come il player, altezza sottile
        Vector2 boxCenter = position + Vector2.down * 0.6f; // Sotto il player

        Collider2D collider = Physics2D.OverlapBox(boxCenter, boxSize, 0f);

        isGrounded = collider != null;

        if (isGrounded)
        {
            verticalSpeed = 0f;
            jumpCount = 0;
        }
    }

    private void CheckCeiling()
    {
        if (isGrounded || verticalSpeed < 0)
            return;

        Vector2 position = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(position, Vector2.up, ceilingDistance);

        if (hit.collider != null)
        {
            verticalSpeed /= 2f;
            float yPoint = hit.point.y - ceilingDistance;
            transform.position = new Vector3(transform.position.x, yPoint, transform.position.z);
        }
    }

    private void CheckWalls()
    {
        if (currentMovementSpeed == 0f)
            return;

        float movementDirection = Mathf.Sign(currentMovementSpeed);
        Vector2 position = transform.position;
        Vector2 direction = movementDirection > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, boundingBoxWidth / 2f);

        if (hit.collider != null)
        {
            currentMovementSpeed = 0f;
            float xPoint = hit.point.x - (movementDirection * (boundingBoxWidth / 2f));
            transform.position = new Vector3(xPoint, transform.position.y, transform.position.z);
        }
    }

    public void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
    }

    // POTERI GUSCI

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
