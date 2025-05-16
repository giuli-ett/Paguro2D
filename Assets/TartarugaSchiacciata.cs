using UnityEngine;

public class TartarugaSchiacciata : MonoBehaviour
{
    private Tartaruga tartaruga;

    private void Start()
    {
        tartaruga = GetComponentInParent<Tartaruga>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tartaruga.SetSchiacciato(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            tartaruga.SetSchiacciato(true);
        }
    }
}
