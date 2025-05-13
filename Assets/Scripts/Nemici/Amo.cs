using UnityEngine;
using UnityEngine.InputSystem;

public class Amo : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    public float swingForce = 10f;
    public float climbSpeed = 2f;

    private bool isHooked = false;
    private Transform currentHook;
    private HingeJoint2D joint;

    private Vector2 moveInput;

     void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        joint = gameObject.AddComponent<HingeJoint2D>();
        joint.autoConfigureConnectedAnchor = false;
        joint.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hook") && !isHooked)
        {
            HookTo(collision.transform);
        }
    }

    private void HookTo(Transform hook)
    {
        isHooked = true;
        currentHook  = hook;

        Rigidbody2D hookRb = hook.GetComponent<Rigidbody2D>();

        joint.connectedBody = hookRb;
        joint.anchor = Vector2.zero;
        joint.connectedAnchor = Vector2.zero;
        joint.enabled = true;

        rb.linearVelocity = Vector2.zero;
    }

    public void MoveAmo(InputAction.CallbackContext context)
    {
        if (!isHooked) return;

        moveInput = context.ReadValue<Vector2>();

        if (Mathf.Abs(moveInput.x) > 0.1f)
        {
            rb.AddForce(Vector2.right * moveInput.x * swingForce);
        }

        if (Mathf.Abs(moveInput.y) > 0.1f)
        {
            transform.position += Vector3.up * moveInput.y * climbSpeed * Time.deltaTime;
        }
    }

    public void Unhook(InputAction.CallbackContext context)
    {
        if (isHooked && context.started)
        {
            isHooked = false;
            currentHook = null;
            joint.enabled = false;
        }
    }
}
