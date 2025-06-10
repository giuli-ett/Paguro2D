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

        private Animator agentAnimator;
        [SerializeReference] public BlackboardVariable<string> IsWalkingParamName = new("isWalking");

        protected override Status OnStart()
        {
            if (Agent.Value == null || Target.Value == null)
            {
                Debug.LogWarning("Chase2D: Agent o Target non assegnati");
                return Status.Failure;
            }

            agentAnimator = Agent.Value.GetComponent<Animator>();
            if (agentAnimator == null)
            {
                Debug.LogWarning("Chase2D: Agent non ha l'animator");
                return Status.Failure;
            }

            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (Agent.Value == null || Target.Value == null || agentAnimator == null)
            {
                SetWalkingAnimation(false);
                return Status.Failure;
            }

            Vector2 agentPos = Agent.Value.transform.position;
            Vector2 targetPos = Target.Value.transform.position;
            float distance = Vector2.Distance(agentPos, targetPos);

            if (distance <= StopDistance.Value)
            {
                SetWalkingAnimation(false);
                return Status.Success;
            }

            // Si muove verso il target
            Vector2 direction = (targetPos - agentPos).normalized;
            Agent.Value.transform.position += (Vector3)(direction * Speed.Value * Time.deltaTime);

            if (Speed.Value > 0)
            {
                SetWalkingAnimation(true);
            }
            else
            {
                SetWalkingAnimation(false);
            }

            FlipAgent(direction.x);
            return Status.Running;
        }

        protected override void OnEnd()
        {
            SetWalkingAnimation(false);
        }

        private void SetWalkingAnimation(bool isWalking)
        {
            if (agentAnimator != null && !string.IsNullOrEmpty(IsWalkingParamName.Value))
            {
                foreach (AnimatorControllerParameter param in agentAnimator.parameters)
                {
                    if (param.name == IsWalkingParamName.Value && param.type == AnimatorControllerParameterType.Bool)
                    {
                        agentAnimator.SetBool(IsWalkingParamName.Value, isWalking);
                        return;
                    }
                }
            }
        }

        private void FlipAgent(float directionX)
        {
            if (Agent.Value != null)
            {
                SpriteRenderer sr = Agent.Value.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    if (directionX > 0)
                    {
                        sr.flipX = false; // Guarda a destra
                    }
                    else if (directionX < 0)
                    {
                        sr.flipX = true; // Guarda a sinistra
                    }
                }
            }
        }
    }
}

