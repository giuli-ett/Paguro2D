using UnityEngine;

public class Palla : MonoBehaviour
{
    [SerializeField] public float speed = 15f;
    [SerializeField] public bool destra = false;

    private bool isRolling = false;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void StartRolling()
    {
        isRolling = true;
    }

    void FixedUpdate()
    {
        if (isRolling)
        {
            float direction = destra ? 1f : -1f;
            rb.linearVelocity = new Vector2(direction * speed, rb.linearVelocity.y);

            float rotationSpeed = 360f;
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.fixedDeltaTime * direction);
        }
    }
}
