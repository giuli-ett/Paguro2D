using UnityEngine;
public class Pendolo : MonoBehaviour
{
    public float swingAngle = 30f; // Maximum swing angle
    public float swingSpeed = 1f;  // Speed of the swinging
    public float swingFrequency = 1f; // Frequency of the swing motion
    private float currentAngle;
    private float time;
    public float AngularVelocity { get; private set; }
    public Transform climbTopLimit;
    public Transform climbBottomLimit;

    void Start()
    {
        // Ensure the rod is set at the initial position
        currentAngle = 0f;
        time = 0f;
    }
   void Update()
    {
        time += Time.deltaTime * swingSpeed;

        float prevAngle = currentAngle;
        currentAngle = swingAngle * Mathf.Sin(time * swingFrequency);
        AngularVelocity = (currentAngle - prevAngle) / Time.deltaTime;

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }
}
