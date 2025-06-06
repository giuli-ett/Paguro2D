using UnityEditor.AssetImporters;
using UnityEngine;

public class Collezionabili : MonoBehaviour
{
    public string nome;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        InterfacciaFineLivello.Instance.CollectItem(this);
        Destroy(gameObject);
    }

}
