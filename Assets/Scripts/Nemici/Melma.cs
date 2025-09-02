using UnityEngine;

public class Melma : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlayMelma();
            Player.Instance.Melmato();
        }
    }
}
