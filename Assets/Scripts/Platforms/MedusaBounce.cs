using UnityEngine;

public class MedusaBounce : MonoBehaviour
{
    [SerializeField] private float bounceStrength = 5f; // Forza del rimbalzo   

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with Medusa");
            AudioManager.Instance.PlayJellyfishBounce();
            Player.Instance.jumpCount = 0;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceStrength, ForceMode2D.Impulse);

            Player.Instance.jumpCount = 1;

        }
    }
}
