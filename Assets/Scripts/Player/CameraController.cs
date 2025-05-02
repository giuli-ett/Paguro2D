using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header ("RIFERIMENTI")]
    [SerializeField] private Transform target;

    [Header ("MOVIMENTO")]
    [SerializeField] private float cameraMovementSpeed = 2.0f;
    [SerializeField] private float zOffset = -16.0f;

    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3 (target.position.x, transform.position.y, zOffset);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * cameraMovementSpeed);
    }

    private void Update()
    {
        FollowTarget();
    }
}
