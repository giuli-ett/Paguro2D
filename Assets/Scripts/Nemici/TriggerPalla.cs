using UnityEngine;

public class TriggerPalla : MonoBehaviour
{
    public Palla palla;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (palla.flag == false) 
                palla.StartRolling();
            else
            {
                if (!palla.gameObject.activeSelf)
                {
                    palla.ResetPosition();
                    palla.StartRolling();
                }
                    
            }
        }
    }
}
