using UnityEngine;

public class TrascinamentoPesci : MonoBehaviour
{
    private BancoPesci fishGroup;

    void Start()
    {
        fishGroup = GetComponentInParent<BancoPesci>();
    }

    [SerializeField] float forzaTrascinamento = 2f; // Puoi regolarlo da Inspector

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null && fishGroup != null)
            {
                Vector3 delta = fishGroup.GetDeltaMovement() * forzaTrascinamento;
                rb.position += new Vector2(delta.x, delta.y);
            }
        }
    }
}
