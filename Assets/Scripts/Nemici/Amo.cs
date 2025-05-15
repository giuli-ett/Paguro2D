using UnityEngine;
using UnityEngine.InputSystem;

public class Amo : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3f;
    private Rigidbody2D rb;
    public bool isAttached = false;
    private bool isClimbing = false;
    private Transform currentClimbable;
    public Collider2D currentClimbableCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hook"))
        {
            AttachTo(other.transform);
        }
    }

    private void AttachTo(Transform hook)
    {
        transform.SetParent(hook);

        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;

        isAttached = true;
        currentClimbable = hook;
        currentClimbableCollider = hook.GetComponent<Collider2D>();
        Player.Instance.isClimbing = true;
    }

    public void Detach(InputAction.CallbackContext context)
    {
        if (!isAttached)
        return;

        if (context.started)
        {
            transform.SetParent(null);
            
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;

            currentClimbable = null;
            currentClimbableCollider = null;

            isClimbing = false;
            isAttached = false;

            Player.Instance.isClimbing = false;
        }
    }
}
