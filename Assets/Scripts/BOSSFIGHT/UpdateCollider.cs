using System.Collections.Generic;
using UnityEngine;

public class UpdateColliderShape : MonoBehaviour
{
    private PolygonCollider2D polyCollider;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        polyCollider = GetComponent<PolygonCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (polyCollider == null || spriteRenderer == null)
        {
            Debug.LogError("UpdateColliderShape: PolygonCollider2D o SpriteRenderer mancante!");
            return;
        }

        AggiornaCollider();
    }

    void LateUpdate()
    {
        AggiornaCollider();
    }

    private void AggiornaCollider()
{
    if (polyCollider != null && spriteRenderer != null && spriteRenderer.sprite != null)
    {
        List<Vector2> punti = new List<Vector2>();
        spriteRenderer.sprite.GetPhysicsShape(0, punti); // Passiamo la lista
        polyCollider.SetPath(0, punti.ToArray()); // Convertiamo la lista in array per il collider
    }
}
}

