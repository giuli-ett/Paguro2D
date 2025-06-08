using Unity.VisualScripting;
using UnityEngine;

public class TrascinamentoPesci : MonoBehaviour
{
    [SerializeField] private BancoPesci fishGroup;

    private GameObject playerOnTop;
    private bool movementActivated = false; 

    private void Update()
    {
        if (playerOnTop != null && fishGroup != null)
        {
            Vector3 delta = fishGroup.GetDeltaMovement();
            playerOnTop.transform.position += delta;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)
                {
                    playerOnTop = collision.gameObject;

                    if (!movementActivated)
                    {
                        fishGroup.ActivateMovement(); 
                        movementActivated = true;
                    }

                    break;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject == playerOnTop)
        {
            playerOnTop = null;
        }
    }
}