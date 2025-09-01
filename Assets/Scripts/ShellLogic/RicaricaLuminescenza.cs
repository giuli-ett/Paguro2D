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

    player.InLuminescenceZone = isEntering;
    
    if (isEntering)
    {
        // When entering recharge zone, stop any existing fade and set to full
        if (player.lightFadeCoroutine != null)
        {
            player.StopCoroutine(player.lightFadeCoroutine);
            player.lightFadeCoroutine = null;
        }
        player.luminescentLight.intensity = 1f;
        player.lastLightIntensity = 1f;
        LightProgressBar.Instance.UpdateProgress(100f);
        Debug.Log("Player entered luminescence zone");
    }
    else
    {
        // When exiting, start new fade from current intensity
        PowerLibrary.LuminescenzaOn(player, durataRicarica);
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
            float currentIntensity = Mathf.Lerp(startIntensity, endIntensity, elapsedTime / duration);
            light.intensity = currentIntensity;
            
            // Usa il singleton invece del riferimento locale
            float percentageRemaining = (currentIntensity / startIntensity) * 100f;
            LightProgressBar.Instance.UpdateProgress(percentageRemaining);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        light.intensity = endIntensity;
        light.enabled = false;
        LightProgressBar.Instance.UpdateProgress(0f);
        Player.Instance.GetComponent<LifeController>().Die();
    }
}