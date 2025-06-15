using UnityEngine;

public class Coltello : MonoBehaviour
{
    Rigidbody2D rb;
    BoxCollider2D boxCollider2D;
    bool isFalling = false;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rb.isKinematic = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFalling && other.CompareTag("Player"))
        {
            rb.isKinematic = false;
            isFalling = true;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFalling && ((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            boxCollider2D.enabled = false;
            isFalling = false;
        }
    }
}