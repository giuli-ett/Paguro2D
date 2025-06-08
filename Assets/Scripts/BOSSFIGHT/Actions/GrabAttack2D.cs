using System;
using Unity.Properties;
using UnityEngine;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "GrabAttack2D",
        description: "Moves horizontally to grab the player. If close enough, grabs and holds the player for a few seconds.",
        category: "Action/Attack",
        story: "[Hand] chases and grabs [Target] , holding for a duration",
        id: "c7f8e512-2a9d-4b10-9c6f-331b00000123")]
    public partial class GrabAttack2D : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Agent;
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<float> GrabSpeed = new(10f);
        [SerializeReference] public BlackboardVariable<float> GrabRange = new(1f);
        [SerializeReference] public BlackboardVariable<float> HoldDuration = new(2f);

        private enum State { Chasing, Holding, Releasing, Done }
        private State _state;
        private float _timer;

        protected override Status OnStart()
        {
            if (Agent.Value == null || Target.Value == null)
            {
                Debug.LogWarning("GrabAttack2D: Missing references.");
                return Status.Failure;
            }

            _state = State.Chasing;
            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (Agent.Value == null || Target.Value == null)
                return Status.Failure;

            Vector3 agentPos = Agent.Value.transform.position;
            Vector3 targetPos = Target.Value.transform.position;

            switch (_state)
            {
                case State.Chasing:
                    // Muove solo sull'asse X verso il target
                    Vector3 chaseTarget = new Vector3(targetPos.x, agentPos.y, agentPos.z);
                    Agent.Value.transform.position = Vector3.MoveTowards(agentPos, chaseTarget, GrabSpeed.Value * Time.deltaTime);

                    float distance = Mathf.Abs(agentPos.x - targetPos.x);
                    if (distance < GrabRange.Value)
                    {
                        _state = State.Holding;
                        _timer = HoldDuration.Value;

                        // Optional: blocca il movimento del player
                        Target.Value.GetComponent<Player>().canMove = false;
                    }
                    break;

                case State.Holding:
                    // Posiziona il player sopra o vicino alla mano
                    Target.Value.transform.position = Agent.Value.transform.position + Vector3.up * 1f;

                    _timer -= Time.deltaTime;
                    if (_timer <= 0f)
                    {
                        _state = State.Releasing;
                    }
                    break;

                case State.Releasing:
                    // Sblocca il player
                    Target.Value.GetComponent<Player>().canMove = true;

                    _state = State.Done;
                    break;

                case State.Done:
                    return Status.Success;
            }

            return Status.Running;
        }

        protected override void OnEnd()
        {
            // Cleanup finale se necessario
        }
    }
}

