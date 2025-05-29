using UnityEngine;

public class Pesci : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    private Vector3 startPosition;

    [Header("VARIABILI")]
    [SerializeField] private float velocita = 2f;
    [SerializeField] private float distanza = 1.2f;
    private SpriteRenderer spriteRenderer;
    private float lastX;

    private void Start()
    {
        startPosition = transform.position;
        lastX = startPosition.x;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Move()
    {
        float newX = startPosition.x + Mathf.Sin(Time.time * velocita) * distanza;
        transform.position = new Vector3(newX, startPosition.y, startPosition.z);

        
        float deltaX = newX - lastX;

        if (deltaX > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (deltaX < -0.01f)
        {
            spriteRenderer.flipX = true;
        }

        lastX = newX;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null); 
        }
    }

    void Update()
    {
        Move();
    }
}

