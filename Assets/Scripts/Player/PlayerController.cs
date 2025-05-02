using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using NUnit.Framework;
using UnityEngine;


public class Player : MonoBehaviour
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
    private float boundingBoxWidth = 1.0f;


    [Header("SALTO")]
    [SerializeField] private bool isGrounded; 
    [SerializeField] private float groundDistance = 1.0f;
    [SerializeField] private float ceilingDistance = 1.0f;
    [SerializeField] private float gravity = 50f; 
    [SerializeField] private float jumpPower = 25f;
    [SerializeField] private float terminalSpeed = -100f;

    [Header("POTERI")]
    // DOPPIO SALTO
    private int jumpCount = 0;
    private int maxJump = 1;
    // SCATTO
    [SerializeField] private bool canDash = false;
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTimeLeft = 1.0f;
    private int dashDirection = 0;
 

    public static Player Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        NUnit.Framework.Assert.IsNull(instance);
        instance = this;

        input = GetComponent<PlayerInput>();
        shellManager = GetComponent<ShellManager>();

    }

    private void Move()
    {
        if (!canMove)
        {
            return;
        }

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
            {
                isDashing = false;
            }
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
            transform.position += new Vector3(currentMovementSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
        else
        {
            currentMovementSpeed = moveSpeed * input.Horizontal;
            transform.position += new Vector3(currentMovementSpeed * Time.deltaTime, 0.0f, 0.0f);
        }
    }

    public void StartDash()
    {
        isDashing = true;
        dashTimeLeft = dashDuration;
    }

    private void Jump()
    {
        if (input.Jump && jumpCount < maxJump)
        {
            verticalSpeed = jumpPower;
            isGrounded = false;
            jumpCount ++;
        }
    }

    private void Fall()
    {
        if (isGrounded)
            return;

        if (verticalSpeed > terminalSpeed)
        {
            verticalSpeed = verticalSpeed - (gravity * Time.deltaTime);

            if (verticalSpeed <= terminalSpeed)
            {
                verticalSpeed = terminalSpeed;
            }
        }

        transform.position += new Vector3(0.0f, verticalSpeed * Time.deltaTime, 0.0f);
    }

    private void CheckGrounded()
    {
        if (verticalSpeed > 0)
            return;

        RaycastHit hitInfoLeft;
        RaycastHit hitInfoRight;

        bool isGroundedLeft = Physics.Raycast(transform.position - new Vector3(boundingBoxWidth / 2f, 0f, 0f), Vector3.down, out hitInfoLeft, groundDistance);
        bool isGroundeRight = Physics.Raycast(transform.position + new Vector3(boundingBoxWidth / 2f, 0f, 0f), Vector3.down, out hitInfoRight, groundDistance);

        isGrounded = isGroundedLeft || isGroundeRight;

        if (isGrounded)
        {
            verticalSpeed = 0f;
            jumpCount = 0;
            float yPoint = hitInfoLeft.point.y + groundDistance;

            if (!isGroundedLeft)
            {
                yPoint = hitInfoRight.point.y + groundDistance;
            }
            transform.position = new Vector3(transform.position.x, yPoint, transform.position.z);
        }
    }

    private void CheckCeiling()
    {
        if (isGrounded || verticalSpeed < 0)
            return;

        RaycastHit hitInfo;
        bool ceilingCollision = Physics.Raycast(transform.position, Vector3.up, out hitInfo, ceilingDistance);

        if (ceilingCollision)
        {
            verticalSpeed /= 2f;
            float yPoint = hitInfo.point.y - ceilingDistance;

            transform.position = new Vector3(transform.position.x, yPoint, transform.position.z);
        }
    }

    private void CheckWalls()
    {
        if (currentMovementSpeed == 0f)
            return;

        float movementDirection = Mathf.Sign(currentMovementSpeed);

        RaycastHit hitInfo;
        bool wallCollision = Physics.Raycast(transform.position, Vector3.right * movementDirection, out hitInfo, boundingBoxWidth / 2f);

        if (wallCollision)
        {
            currentMovementSpeed = 0f;
            float xPoint = hitInfo.point.x - (movementDirection * (boundingBoxWidth / 2f));
            transform.position = new Vector3(xPoint, transform.position.y, transform.position.z);
        }
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

    // POTERI GUSCI

    // GUSCIO DOPPIO SALTO ATTIVATO
    public void EnableDoubleJump()
    {
        maxJump = 2;
    }

    // GUSCIO DOPPIO SALTO DISATTIVATO
    public void DisableDoubleJump()
    {
        maxJump = 1;
    }

    // GUSCIO SCATTO ATTIVATO
    public void EnableDash()
    {
        canDash = true;
    }

    // GUSCIO SCATTO DISATTIVATO
    public void DisableDush()
    {
        canDash = false;
        isDashing = false;
    }

}