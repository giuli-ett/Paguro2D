using UnityEngine;

public class RicaricaLuminescenza : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player.shellManager.currentShell.power == ShellPower.Luminescenza)
        {
            player.InLuminescenceZone = true;
            PowerLibrary.RechargeLuminescence(player);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player.InLuminescenceZone && player.shellManager.currentShell.power == ShellPower.Luminescenza)
        {
            // Mantieni la luminosità al massimo costantemente
            player.luminescentLight.enabled = true;
            player.luminescentLight.intensity = 1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player.shellManager.currentShell.power == ShellPower.Luminescenza)
        {
            player.InLuminescenceZone = false;
            PowerLibrary.LuminescenzaOn(player);
        }
    }
}