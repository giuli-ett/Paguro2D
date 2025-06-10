using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerLibrary : MonoBehaviour
{
    private static Coroutine luminescenceCoroutine;
    private static Coroutine camouflageCoroutine;


    public static void ActivatePower(ShellPower type, Player player)
    {
        switch (type)
        {
            case ShellPower.Base:
                BaseOn(player);
                break;

            case ShellPower.Rotola:
                RotolaOn(player);
                break;

            case ShellPower.SpeedBoost:
                SpeedBoostOn(player);
                break;

            case ShellPower.JumpBoost:
                JumpBoostOn(player);
                break;

            case ShellPower.Luminescenza:
                LuminescenzaOn(player);
                break;

            case ShellPower.NascondiScava:
                NascondiScavaOn(player);
                break;
            
            case ShellPower.Mimetico:
                MimeticoOn(player);
                break;
        }
    }

    public static void DisactivatePower(ShellPower type, Player player)
    {
        switch (type)
        {
            case ShellPower.Base:
                BaseOff(player);
                break;

            case ShellPower.Rotola:
                RotolaOff(player);
                break;

            case ShellPower.SpeedBoost:
                SpeedBoostOff(player);
                break;

            case ShellPower.JumpBoost:
                JumpBoostOff(player);
                break;

            case ShellPower.Luminescenza:
                LuminescenzaOff(player);
                break;

            case ShellPower.NascondiScava:
                NascondiScavaOff(player);
                break;

            case ShellPower.Mimetico:
                MimeticoOff(player);
                break;
        }
    }

    public static void BaseOn(Player player)
    {
        Debug.Log("Guscio base on");
    }

    public static void BaseOff(Player player)
    {
        Debug.Log("Guscio base off");
    }

    public static void RotolaOn(Player player)
    {

    }
    public static void JumpBoostOn(Player player)
    {
        Player.Instance.EnableDoubleJump();
        Debug.Log($"Hai disattivato il potere: {Player.Instance.shellManager.currentShell.shellPower}");
    }

    public static void SpeedBoostOn(Player player)
    {
        Player.Instance.EnableDash();
        Debug.Log($"Hai un nuovo super potere: {Player.Instance.shellManager.currentShell.shellPower}");
    }
    public static void LuminescenzaOn(Player player)
    {
        var light = player.luminescentLight;
        if (light == null) return;

        light.enabled = true;
        light.intensity = 1f;

        if (luminescenceCoroutine != null)
            player.StopCoroutine(luminescenceCoroutine);

        luminescenceCoroutine = player.StartCoroutine(LuminescenceFade(light, 10f));
    }

    public static void NascondiScavaOn(Player player)
    {
        Player.Instance.EnableNascondiScava();
        Debug.Log($"Hai un nuovo super potere: {Player.Instance.shellManager.currentShell.shellPower}");
    }

    public static void MimeticoOn(Player player)
    {
        if (camouflageCoroutine != null)
            player.StopCoroutine(camouflageCoroutine);

        camouflageCoroutine = player.StartCoroutine(CamouflageRoutine(player));
    }

    public static void RotolaOff(Player player)
    {

    }

    public static void JumpBoostOff(Player player)
    {
        Player.Instance.DisableDoubleJump();
        Debug.Log($"Hai rimosso il guscio: {Player.Instance.shellManager.currentShell.shellPower}");
    }

    public static void LuminescenzaOff(Player player)
    {
        var light = player.luminescentLight;
        if (light == null) return;

        if (luminescenceCoroutine != null)
            player.StopCoroutine(luminescenceCoroutine);

        light.enabled = false;

    }

    public static void SpeedBoostOff(Player player)
    {
        Player.Instance.DisableDash();
        Debug.Log($"Hai disattivato il potere: {Player.Instance.shellManager.currentShell.shellPower}");
    }

    public static void NascondiScavaOff(Player player)
    {
        Player.Instance.DisableNascondiScava();
        Debug.Log($"Hai rimosso il guscio: {Player.Instance.shellManager.currentShell.shellPower}");
    }

    public static void MimeticoOff(Player player)
    {

    }

    // altre funzioni

    // gestione luminescenza

    private static IEnumerator LuminescenceFade(Light2D light, float duration)
    {
        float startIntensity = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            light.intensity = Mathf.Lerp(startIntensity, 0f, elapsed / duration);
            yield return null;
        }

        light.intensity = 0f;
        light.enabled = false;
    }
    public static void RechargeLuminescence(Player player)
    {
            var light = player.luminescentLight;
            if (light == null) return;

            if (luminescenceCoroutine != null)
                player.StopCoroutine(luminescenceCoroutine);

            light.enabled = true;
            light.intensity = 1f;
    }

    // gestione mimetizzazione

    private static IEnumerator CamouflageRoutine(Player player)
    {
        player.isInvisible = true;

        // Effetto visivo: semitrasparente
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.3f);

        yield return new WaitForSeconds(5f);

        player.isInvisible = false;

        if (spriteRenderer != null)
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

        camouflageCoroutine = null;
    }


}
