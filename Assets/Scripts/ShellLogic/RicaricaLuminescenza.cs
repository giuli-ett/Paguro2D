using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.Rendering.Universal;
public class RicaricaLuminescenza : MonoBehaviour
{
    public float durataRicarica = 5f; // Durata luce quando esco da alga
    private Coroutine fadeCoroutine;

    private void Start()
    {
        // Inizializza la coroutine a null
        fadeCoroutine = null;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();

        
                
        if (player != null && player.shellManager.currentShell.power == ShellPower.Luminescenza)
            {
                if (player.lightFadeCoroutine != null)
                {
                    StopCoroutine(player.lightFadeCoroutine);
                    player.lightFadeCoroutine = null;
                }

                Debug.Log("Player entered luminescence zone");
                player.InLuminescenceZone = true;
                player.luminescentLight.enabled = true;
                player.luminescentLight.intensity = 1f;
                PowerLibrary.RechargeLuminescence(player);
            }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Player player = other.GetComponent<Player>();
        if (player != null && player.InLuminescenceZone && player.shellManager.currentShell.power == ShellPower.Luminescenza)
        {
            // Mantieni la luminosit� al massimo costantemente
            
            player.luminescentLight.intensity = 1f;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player exited luminescence zone");
            Player player = other.GetComponent<Player>();
            if (player != null && player.shellManager.currentShell.power == ShellPower.Luminescenza)
            {
               
                player.InLuminescenceZone = false;
                player.lightDuration = durataRicarica;

                 if (player.lightFadeCoroutine != null)
                {
                    StopCoroutine(player.lightFadeCoroutine);
                    player.lightFadeCoroutine = null;
                }

                player.lightFadeCoroutine = StartCoroutine(FadeLightIntensity(player.luminescentLight, player.luminescentLight.intensity, 0f, durataRicarica));
                PowerLibrary.LuminescenzaOn(player);
            }
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
        light.intensity = endIntensity; // Assicurati che l'intensit� finale sia impostata correttamente
    }
}