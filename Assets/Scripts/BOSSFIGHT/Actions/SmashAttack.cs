using System;
using Unity.Properties;
using UnityEngine;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "SmashAttack2D",
        description: "Raises the hand by a vertical offset, waits, then slams down near the player.",
        category: "Action/Attack",
        story: "[Hand] rises by height and slams down near [Target]",
        id: "6e5bb81b-3d57-4cd0-a3f6-233d97089bc9")]
    public partial class SmashAttack2D : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Agent;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<float> RiseHeight = new(3f);
        [SerializeReference] public BlackboardVariable<float> RiseSpeed = new(3f);
        [SerializeReference] public BlackboardVariable<float> SmashSpeed = new(8f);
        [SerializeReference] public BlackboardVariable<float> DelayBeforeSmash = new(0.5f);
        [SerializeReference] public BlackboardVariable<float> OffsetFromTarget = new(1f);

        private enum State { GoingUp, Waiting, GoingDown, Done }
        private State _state;
        private float _timer;
        private Vector3 _smashTarget;
        private Vector3 _riseTarget;

        protected override Status OnStart()
        {
            if (Agent.Value == null || Target.Value == null)
            {
                Debug.LogWarning("SmashAttack2D: Missing references.");
                return Status.Failure;
            }

            // Calcola il punto a cui alzarsi (sulla Y) in base a RiseHeight
            var startPos = Agent.Value.transform.position;
            _riseTarget = new Vector3(startPos.x, startPos.y + RiseHeight.Value, startPos.z);

            _state = State.GoingUp;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (Agent.Value == null || Target.Value == null)
                return Status.Failure;

            Vector3 agentPos = Agent.Value.transform.position;

            switch (_state)
            {
                case State.GoingUp:
                    Agent.Value.transform.position = Vector3.MoveTowards(agentPos, _riseTarget, RiseSpeed.Value * Time.deltaTime);

                    if (Vector3.Distance(agentPos, _riseTarget) < 0.05f)
                    {
                        _timer = DelayBeforeSmash.Value;
                        _state = State.Waiting;
                    }
                    break;

                case State.Waiting:
                    _timer -= Time.deltaTime;
                    if (_timer <= 0f)
                    {
                        Vector3 targetPos = Target.Value.transform.position;
                        _smashTarget = new Vector3(targetPos.x + OffsetFromTarget.Value, targetPos.y, agentPos.z);
                        _state = State.GoingDown;
                    }
                    break;

                case State.GoingDown:
                    Agent.Value.transform.position = Vector3.MoveTowards(agentPos, _smashTarget, SmashSpeed.Value * Time.deltaTime);
                    if (Vector3.Distance(agentPos, _smashTarget) < 0.05f)
                    {
                        _state = State.Done;
                    }
                    break;

                case State.Done:
                    return Status.Success;
            }

            return Status.Running;
        }

        protected override void OnEnd()
        {
            // Optional cleanup
        }
    }
}

