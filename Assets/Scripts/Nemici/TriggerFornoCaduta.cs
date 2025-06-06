using UnityEngine;

public class TriggerFornoCaduta : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player.Instance.GetComponent<LifeController>().Die();
        }
    }
}
