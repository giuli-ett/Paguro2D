using UnityEngine;

[CreateAssetMenu(fileName = "NuovoLivello", menuName = "Dati Gioco/Livello")]
public class LivelloData : ScriptableObject
{
    public string nomeLivello;
    public bool isCompleted = false;
    public CollezionabiliLivello collezionabiliLivello;
}
