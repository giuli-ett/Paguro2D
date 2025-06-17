using UnityEngine;

public class Collezionabile : MonoBehaviour
{
    public string nome;
    public Slot slot;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    public void Collect()
    {
        slot.SetCollezionabile(this.GetComponent<SpriteRenderer>().sprite);
        GameManager.Instance.CollectItem(this);
        Destroy(gameObject);
    }

}
