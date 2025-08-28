using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class RicaricaLuminescenza : MonoBehaviour
{
    public float durataRicarica = 5f;

   private void HandleLuminescence(Player player, bool isEntering)
{
    if (player == null || player.shellManager.currentShell.power != ShellPower.Luminescenza) 
        return;

    if (player.lightFadeCoroutine != null)
    {
        StopCoroutine(player.lightFadeCoroutine);
        player.lightFadeCoroutine = null;
    }

    player.InLuminescenceZone = isEntering;
    player.luminescentLight.enabled = true;
    player.luminescentLight.intensity = 1f;

    if (isEntering)
    {
        PowerLibrary.RechargeLight(player);
        Debug.Log("Player entered luminescence zone");
    }
    else
    {
        // Only start fading when leaving the trigger area
        player.lightDuration = durataRicarica;
        player.lightFadeCoroutine = StartCoroutine(FadeLightIntensity(
            player.luminescentLight, 
            1f, 
            0f, 
            durataRicarica
        ));
        Debug.Log("Player exited luminescence zone");
    }
}

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleLuminescence(other.GetComponent<Player>(), true);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var player = other.GetComponent<Player>();
        if (player != null && player.InLuminescenceZone && 
            player.shellManager.currentShell.power == ShellPower.Luminescenza)
        {
            player.luminescentLight.intensity = 1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            HandleLuminescence(other.GetComponent<Player>(), false);
        }
    }
    
    private IEnumerator FadeLightIntensity(Light2D light, float startIntensity, float endIntensity, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            light.intensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        light.intensity = endIntensity;
    }
}