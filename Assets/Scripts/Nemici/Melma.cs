using UnityEngine;

public class Melma : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.PlayMelma();
            Player.Instance.Melmato();
        }
    }
}
