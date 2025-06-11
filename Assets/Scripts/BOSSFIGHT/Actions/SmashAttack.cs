using System;
using Unity.Properties;
using UnityEngine; 

namespace Unity.Behavior {
    
    [Serializable, GeneratePropertyBag]
    [NodeDescription(name: "SmashAttack2D",
    description: "Raises the hand by a vertical offset, waits, then slams down near the player.",
    category: "Action/Attack",
    story: "[Hand] rises by height and slams down near [Target]",
    id: "6e5bb81b-3d57-4cd0-a3f6-233d97089bc9")]

    public partial class SmashAttack2D : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Agent;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<float> OffsetFromTarget = new(1f);
        [SerializeReference] public BlackboardVariable<string> IsSmashingParamName = new("isSmashing");

        private Animator agentAnimator;
        private float _animationDuration;
        private bool _isSmashingActive;

        protected override Status OnStart()
        {
            if (Agent.Value == null || Target.Value == null)
            {
                Debug.LogWarning("SmashAttack2D: Missing references.");
                return Status.Failure;
            }

            agentAnimator = Agent.Value.GetComponent<Animator>();
            if (agentAnimator == null)
            {
                Debug.LogWarning("SmashAttack2D: Agent does not have an animator.");
                return Status.Failure;
            }

            // Ottiene la durata dell'animazione
            _animationDuration = GetAnimationClipDuration("Schiacciata");

            // Setta il parametro per avviare l'animazione
            agentAnimator.SetBool(IsSmashingParamName.Value, true);
            _isSmashingActive = true;

            Debug.Log("Mano schiaccia paguro");
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (Agent.Value == null || Target.Value == null || agentAnimator == null)
                return Status.Failure;

            if (_isSmashingActive)
            {
                _animationDuration -= Time.deltaTime;
                if (_animationDuration <= 0f)
                {
                    // Termina l'attacco e resetta il parametro
                    agentAnimator.SetBool(IsSmashingParamName.Value, false);
                    return Status.Success;
                }
            }

            return Status.Running;
        }

        protected override void OnEnd()
        {
            // Resetta il parametro alla fine
            if (agentAnimator != null)
            {
                agentAnimator.SetBool(IsSmashingParamName.Value, false);
            }
        }

        private float GetAnimationClipDuration(string clipName)
        {
            if (agentAnimator != null)
            {
                foreach (AnimationClip clip in agentAnimator.runtimeAnimatorController.animationClips)
                {
                    if (clip.name == clipName)
                    {
                        return clip.length;
                    }
                }
            }
            Debug.LogWarning("SmashAttack2D: Animation clip not found.");
            return 1f; // Default duration in caso di errore
        }
    }
}

