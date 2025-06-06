using UnityEditor.AssetImporters;
using UnityEngine;

public class Collezionabile : MonoBehaviour
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
        GameManager.Instance.CollectItem(this);
        Destroy(gameObject);
    }

}
