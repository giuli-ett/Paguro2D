using UnityEngine;

public class Spikes : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player.Instance.GetComponent<LifeController>().TakeDamage();
        }
    }
}
