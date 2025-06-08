using NUnit.Framework;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.Arm;
using static Unity.Cinemachine.CinemachineFreeLookModifier;


public class BancoPesci : MonoBehaviour
{
    public Transform[] waypoints;
    [SerializeField] float speed = 2f;
    private int currentIndex = 0;

    private Vector3 lastPosition;
    private Vector3 deltaMovement;
    private SpriteRenderer spriteRenderer;

    private bool isActivated = false; 

    void Start()
    {
        lastPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isActivated || waypoints.Length == 0) return;

        lastPosition = transform.position;

        Transform target = waypoints[currentIndex];
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        deltaMovement = transform.position - lastPosition;

        if (deltaMovement.x != 0)
        {
            spriteRenderer.flipX = deltaMovement.x < 0;
        }

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
    }

    public Vector3 GetDeltaMovement()
    {
        return deltaMovement;
    }

    public void ActivateMovement() 
    {
        isActivated = true;
    }
}
