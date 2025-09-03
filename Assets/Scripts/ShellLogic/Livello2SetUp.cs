using UnityEngine;

public class Livello2SetUp : MonoBehaviour
{
    public ShellManager shellManager;
    public ShellPicker[] shellPickersDaEquipaggiare;

    private void Start()
    {
        Player.Instance.isInLevel2 = true;

        foreach (var shellPicker in shellPickersDaEquipaggiare)
        {
            Shell shell = shellPicker.shell;

            shellManager.inventario.AggiungiGuscio(shell, shellPicker);
        }

        shellManager.WearShell(shellPickersDaEquipaggiare[0].shell, shellPickersDaEquipaggiare[0]);

        Player.Instance.isInLevel2 = false;
    }
}
