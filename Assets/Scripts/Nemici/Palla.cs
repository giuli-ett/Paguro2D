using System.Collections;
using UnityEngine;

public class Palla : MonoBehaviour
{
    [SerializeField] public float speed = 15f;
    [SerializeField] private float TempoVita = 5f;
    [SerializeField] private Transform startPoint;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private Coroutine lifetimeCoroutine;
    private bool isRolling = false;
    public bool flag = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = startPoint.position;
    }

    public void StartRolling()
    {
        flag = true;
        isRolling = true;
        gameObject.SetActive(true); // Assicurati che sia attiva prima di iniziare a muoversi
        rb.linearVelocity = new Vector2(-speed, 0f);

        if (lifetimeCoroutine != null)
            StopCoroutine(lifetimeCoroutine);

        lifetimeCoroutine = StartCoroutine(LifetimeTimer());
    }


    private IEnumerator LifetimeTimer()
    {
        yield return new WaitForSeconds(TempoVita);
        gameObject.SetActive(false);
    }


    public void ResetPosition()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;

        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
            lifetimeCoroutine = null;
        }

        isRolling = false;

        gameObject.SetActive(false); // disattiva prima
        transform.position = startPoint.position; // sposta dopo
        gameObject.SetActive(true); // riattiva alla fine
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LifeController vita = other.GetComponent<LifeController>();
            if (vita != null)
            {
                Debug.Log("Palla ha ucciso il giocatore!");
                vita.Die();
            }
        }
    }
}
