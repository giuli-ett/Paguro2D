using System.Collections;
using UnityEngine;

public class Palla : MonoBehaviour
{
    [SerializeField] public float speed = 15f;
    [SerializeField] public bool destra = false;
    [SerializeField] private float TempoVita = 5f; // Tempo di vita in secondi

    private bool isRolling = false;
    private Rigidbody2D rb;
    private Vector3 startPosition;
    private Coroutine lifetimeCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
    }

    public void StartRolling()
    {
        isRolling = true;
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
        }
        lifetimeCoroutine = StartCoroutine(LifetimeTimer());
    }

    private IEnumerator LifetimeTimer()
    {
        yield return new WaitForSeconds(TempoVita);
        gameObject.SetActive(false); // Oppure Destroy(gameObject) se vuoi eliminarla del tutto
    }

    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.position = startPosition;
        isRolling = false;

        // Riattiva la palla se era stata disattivata
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        // Ferma la coroutine del timer se attiva
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
            lifetimeCoroutine = null;
        }
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
