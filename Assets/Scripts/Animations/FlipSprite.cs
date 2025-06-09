using UnityEngine;

public class FlipSprite : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool facingRight = false; // PARTENZA: guarda a sinistra

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float moveX = rb.linearVelocity.x;

        if (moveX > 0.1f && !facingRight)
        {
            Flip();
        }
        else if (moveX < -0.1f && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
