using UnityEngine;

public class Collezionabile : MonoBehaviour
{
    //public string nome;
    public Slot slot;
    public Sprite spriteCollezionabile;
    public int idCollezionabile;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayCollezionabile();
            Collect();
        }
    }

    public void Collect()
    {
        slot.SetCollezionabile(this.GetComponent<SpriteRenderer>().sprite);
        GameManager.Instance.CollectItem(this);
        gameObject.SetActive(false);

    }

}
