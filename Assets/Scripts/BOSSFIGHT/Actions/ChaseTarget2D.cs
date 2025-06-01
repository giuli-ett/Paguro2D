using System;
using Unity.Properties;
using UnityEngine;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "Chase2D",
        description: "Chases a target GameObject in 2D space unless within StopDistance.",
        category: "Action/Navigation",
        story: "[Agent] chases [Target] until it gets close",
        id: "cfb7452e-2fc1-4c27-bc89-1ebed9861a00")]
    internal partial class ChaseTarget2D : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Agent;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<float> Speed = new(2f);
        [SerializeReference] public BlackboardVariable<float> StopDistance = new(1.5f); // distanza di stop

        protected override Status OnStart()
        {
            if (Agent.Value == null || Target.Value == null)
            {
                Debug.LogWarning("Chase2D: Agent or Target not assigned.");
                return Status.Failure;
            }
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (Agent.Value == null || Target.Value == null)
                return Status.Failure;

            Vector2 agentPos = Agent.Value.transform.position;
            Vector2 targetPos = Target.Value.transform.position;
            float distance = Vector2.Distance(agentPos, targetPos);

            if (distance <= StopDistance.Value)
            {
                // Troppo vicino, si ferma
                return Status.Success;
            }

            // Si muove verso il target
            Vector2 direction = (targetPos - agentPos).normalized;
            Agent.Value.transform.position += (Vector3)(direction * Speed.Value * Time.deltaTime);

            return Status.Running;
        }

        protected override void OnEnd()
        {
            // Nessuna pulizia necessaria
        }
    }
}

