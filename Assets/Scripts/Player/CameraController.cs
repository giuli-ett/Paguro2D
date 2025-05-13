using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("RIFERIMENTI")]
    [SerializeField] private Transform target;

    [Header("MOVIMENTO")]
    [SerializeField] private float cameraMovementSpeed = 5.0f;
    [SerializeField] private float zOffset = -16.0f;
    [SerializeField] private float xOffset = 0.0f;

    [Header("ZONE")]
    [SerializeField] private float verticalZoneHeight = 10.0f;
    [SerializeField] private float yMarginOffset = 2.0f;
    [SerializeField] private float cameraYOffset = 0.0f; 

    private float currentZoneY;
    private int currentZone = 0;

    private void Start()
    {
        currentZoneY = transform.position.y; 
    }

    private void Update()
    {
        CheckZoneChange();
        FollowTarget();
    }

    private void CheckZoneChange()
    {
        float playerY = target.position.y;

        if (playerY > currentZoneY + verticalZoneHeight)
        {
            currentZone++;
            currentZoneY += verticalZoneHeight;
        }
        else if (playerY < currentZoneY - verticalZoneHeight)
        {
            currentZone--;
            currentZoneY -= verticalZoneHeight;
        }

        if (playerY < currentZoneY && playerY > currentZoneY - verticalZoneHeight + yMarginOffset)
        {
            cameraYOffset = -yMarginOffset; 
        }
        else
        {
            cameraYOffset = 0.0f; 
        }
    }

    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3(
            target.position.x + xOffset,
            currentZoneY + cameraYOffset,
            zOffset
        );

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraMovementSpeed);
    }
}

