using UnityEngine;

public class RicaricaLuminescenza : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.InLuminescenceZone = true;
            PowerLibrary.RechargeLuminescence(player);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player.InLuminescenceZone)
        {
            // Mantieni la luminosità al massimo costantemente
            player.luminescentLight.enabled = true;
            player.luminescentLight.intensity = 1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null)
        {
            player.InLuminescenceZone = false;
            // Eventualmente ricomincia il fading
            PowerLibrary.LuminescenzaOn(player);
        }
    }
}