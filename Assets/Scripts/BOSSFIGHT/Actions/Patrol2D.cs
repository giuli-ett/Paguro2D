using System;
using System.Collections.Generic;
using Unity.Properties;
using UnityEngine;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Patrol2D",
        description: "Moves a 2D GameObject between a list of waypoints (Transforms).",
        category: "Action/Navigation",
        story: "[Agent] patrols through [Waypoints] in 2D",
        id: "2d9c44ba-6c4d-43c3-a1aa-2b1775bbf2a1")]
    internal partial class PatrolAction2D : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Agent;
        [SerializeReference] public BlackboardVariable<List<GameObject>> Waypoints;
        [SerializeReference] public BlackboardVariable<float> Speed = new(2f);
        [SerializeReference] public BlackboardVariable<float> DistanceThreshold = new(0.1f);
        [SerializeReference] public BlackboardVariable<float> WaitTimeAtWaypoint = new(0.5f);

        private int currentWaypointIndex = -1;
        private Vector2 currentTarget;
        private float waitTimer = 0f;
        private bool waiting = false;

        protected override Status OnStart()
        {
            if (Agent.Value == null || Waypoints.Value == null || Waypoints.Value.Count == 0)
            {
                Debug.LogWarning("Patrol2D: Agent or Waypoints missing.");
                return Status.Failure;
            }

            waitTimer = 0f;
            waiting = false;
            MoveToNextWaypoint();

            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (Agent.Value == null || Waypoints.Value == null) return Status.Failure;

            if (waiting)
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    waiting = false;
                    MoveToNextWaypoint();
                }
                return Status.Running;
            }

            Vector2 agentPos = Agent.Value.transform.position;
            float distance = Vector2.Distance(agentPos, currentTarget);

            if (distance <= DistanceThreshold.Value)
            {
                waiting = true;
                waitTimer = WaitTimeAtWaypoint.Value;
            }
            else
            {
                Vector2 direction = (currentTarget - agentPos).normalized;
                Agent.Value.transform.position += (Vector3)(direction * Speed.Value * Time.deltaTime);
            }

            return Status.Running;
        }

        protected override void OnEnd()
        {
            // Optional: any cleanup
        }

        private void MoveToNextWaypoint()
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % Waypoints.Value.Count;
            currentTarget = Waypoints.Value[currentWaypointIndex].transform.position;
        }
    }
}

