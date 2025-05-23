using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerLibrary 
{
    private static Coroutine luminescenceCoroutine;

    public static void ActivatePower (ShellPower type, Player player)
    {
        switch (type)
        {
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
        }
    }

    public static void DisactivatePower (ShellPower type, Player player)
    {
        switch (type)
        {
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
        }
    }

    public static void RotolaOn (Player player)
    {

    }

    public static void SpeedBoostOn (Player player)
    {
        Player.Instance.EnableDash();
        Debug.Log($"Hai un nuovo super potere: {Player.Instance.shellManager.currentShell.shellPower}");
    }

    public static void JumpBoostOn (Player player)
    {
        Player.Instance.EnableDoubleJump();
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

    public static void RotolaOff (Player player)
    {

    }

    public static void SpeedBoostOff (Player player)
    {
        Player.Instance.DisableDush();
        Debug.Log($"Hai un nuovo super potere: {Player.Instance.shellManager.currentShell.shellPower}");
    }

    public static void JumpBoostOff (Player player)
    {
        Player.Instance.DisableDoubleJump();
        Debug.Log($"Hai un nuovo super potere: {Player.Instance.shellManager.currentShell.shellPower}");
    }

    public static void LuminescenzaOff(Player player)
    {
        var light = player.luminescentLight;
        if (light == null) return;

        if (luminescenceCoroutine != null)
            player.StopCoroutine(luminescenceCoroutine);

        light.enabled = false;

    }

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

}
