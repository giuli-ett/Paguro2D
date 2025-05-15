using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Pendolo : MonoBehaviour
{
    private Rigidbody2D rb;

    public float moveSpeed;
    public float leftAngle;
    public float rightAngle;

    private bool movingClockWise;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        movingClockWise = true;
    }

    void Update()
    {
        Move();
    }
    
    public void ChangeMoveDirection()
    {
        Debug.Log("Cambio direzione");
        if (transform.rotation.z > rightAngle)
        {
            movingClockWise  = false;
        }
        if (transform.rotation.z < leftAngle)
        {
            movingClockWise = true;
        }
    }

    public void Move()
    {
        ChangeMoveDirection();

        if (movingClockWise)
        {
            rb.angularVelocity = moveSpeed;
        }

        if (!movingClockWise)
        {
            rb.angularVelocity = -1 * moveSpeed;
        }
    }

}

