using UnityEngine;

public class Mamma : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float mamaSpeed;
    [SerializeField] private float xOffset = 3.0f;

    private void FollowTarget()
    {
        Vector3 targetPosition = new Vector3 (target.position.x + xOffset, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * mamaSpeed);
    }

    void Update()
    {
        FollowTarget();
    }
}
