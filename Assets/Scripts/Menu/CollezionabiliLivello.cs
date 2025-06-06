using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollezionabiliLivello", menuName = "Scriptable Objects/CollezionabiliLivello")]
public class CollezionabiliLivello : ScriptableObject
{
    public int livello;
    public List<Collezionabile> collezionabili;
}
