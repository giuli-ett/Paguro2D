using UnityEngine;

[CreateAssetMenu(fileName = "New Shell", menuName = "Shell System/New Shell")]
public class Shell : ScriptableObject
{
    public string shellName;
    public string shellPower;
    public ShellPower power;
    
    public void PowerOn(Player player)
    {
        PowerLibrary.ActivatePower(power, player);
    }

    public void PowerOff(Player player)
    {
        PowerLibrary.DisactivatePower(power, player);
    }
}
