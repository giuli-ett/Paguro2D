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
    public float forceMagnitude;

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
        // Salva la scala globale originale del player
        Vector3 originalGlobalScale = transform.lossyScale;

        // Imposta il parent al punto di aggancio (che potrebbe essere figlio di un oggetto scalato)
        transform.SetParent(hook);

        // Ottieni la scala globale del nuovo parent (hook)
        Vector3 parentGlobalScale = hook.lossyScale;

        // Calcola la nuova localScale per mantenere il player invariato visivamente
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
        Player.Instance.isClimbing = true;
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

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1f;

            Player.Instance.isClimbing = false;
            Player.Instance.ApplySwingImpulse();
        }
    }
}

