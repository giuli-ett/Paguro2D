using Unity.VisualScripting;
using UnityEngine;

public class PowerLibrary 
{
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
}
