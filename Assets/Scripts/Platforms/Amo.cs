using UnityEngine;
using UnityEngine.InputSystem;

public class Amo : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3f;
    private Rigidbody2D rb;
    public bool isAttached = false;
    public bool isClimbing = false;
    private Transform currentClimbable;
    public Collider2D currentClimbableCollider;
    private Vector3 initialScale;

    public Transform currentClimbTopLimit;
    public Transform currentClimbBottomLimit;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        initialScale = transform.localScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Assicurati che il player entri in contatto con un oggetto con tag "Hook"
        if (other.CompareTag("Hook"))
        {
            currentClimbTopLimit = other.gameObject.GetComponent<Pendolo>().climbTopLimit;
            currentClimbBottomLimit = other.gameObject.GetComponent<Pendolo>().climbBottomLimit;
            AttachTo(other.transform);
        }
    }

    private void AttachTo(Transform hook)
    {
        Debug.Log("Attacco all'amo");
        Vector3 originalGlobalScale = transform.lossyScale;

        transform.SetParent(hook);

        Vector3 parentGlobalScale = hook.lossyScale;

        transform.localScale = new Vector3(
            originalGlobalScale.x / parentGlobalScale.x,
            originalGlobalScale.y / parentGlobalScale.y,
            originalGlobalScale.z / parentGlobalScale.z
        );

        transform.localRotation = Quaternion.identity;

        rb.gravityScale = 0f;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        isAttached = true;
        Player.Instance.isGrounded = true;
        currentClimbable = hook;
        currentClimbableCollider = hook.GetComponent<Collider2D>();
    }

    public void Detach()
    {
        if (!isAttached)
        {
            return;
        }
        else
        {
            Debug.Log("Distacco dall'amo");
            transform.SetParent(null);

            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            transform.localScale = initialScale;

            currentClimbable = null;
            currentClimbableCollider = null;

            isClimbing = false;
            isAttached = false;
            Player.Instance.animator.SetBool("isAttached", isAttached);
            Player.Instance.animator.SetFloat("yClimbVelocity", 0f);


            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;

            Player.Instance.isClimbing = false;
            Player.Instance.ApplySwingImpulse();
        }
    }
}