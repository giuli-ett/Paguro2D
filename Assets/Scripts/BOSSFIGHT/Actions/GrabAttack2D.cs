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
        [SerializeReference] public BlackboardVariable<float> MaxHeight;
        [SerializeReference] public BlackboardVariable<float> LiftSpeed = new(2f);
        [SerializeReference] public BlackboardVariable<float> GrabRange = new(1f);
        [SerializeReference] public BlackboardVariable<float> HoldDuration = new(2f);
        [SerializeReference] public BlackboardVariable<Transform> GrabPoint;
        [SerializeReference] public BlackboardVariable<int> RequiredPresses = new(10);
        [SerializeReference] public BlackboardVariable<float> ReturnSpeed = new(2f);

        private enum State { Holding, Releasing, Returning, Done }
        private State _state;
        private float _timer;
        private Rigidbody2D rb;
        private int escapeAttempts = 0;
        private Player player;
        private float startHeight;

        protected override Status OnStart()
        {
            if (Agent.Value == null || Target.Value == null || GrabPoint.Value == null)
            {
                Debug.LogWarning("GrabAttack2D: Missing references.");
                return Status.Failure;
            }

            rb = Target.Value.GetComponent<Rigidbody2D>();
            player = Target.Value.GetComponent<Player>();

            float distance = Mathf.Abs(Agent.Value.transform.position.x - Target.Value.transform.position.x);
            if (distance < GrabRange.Value)
            {
                _state = State.Holding;
                _timer = HoldDuration.Value;

                Target.Value.GetComponent<Player>().canMove = false;
                player.isGrabbed = true;
                player.escapeAttempts = 0;

                startHeight = Agent.Value.transform.position.y;

                Debug.Log("Mano acchiappa paguro!");
            }
            else
            {
                Debug.LogWarning("GrabAttack2D: Target fuori dal range, attacco fallito.");
                return Status.Failure;
            }

            return Status.Running;
        }

        protected override Status OnUpdate()
        {
            if (_state == State.Holding)
            {
                rb.gravityScale = 0f;
                rb.linearVelocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
                Target.Value.transform.position = GrabPoint.Value.position;
                Agent.Value.transform.position += Vector3.up * LiftSpeed.Value * Time.deltaTime;

                if (player.escapeAttempts >= player.requiredPresses) // Rileva input tramite Input System
                {
                    Debug.Log("Il paguro si è liberato!");
                    _state = State.Releasing;
                }

                if (Agent.Value.transform.position.y >= MaxHeight.Value)
                {
                    Target.Value.GetComponent<LifeController>().Die();
                    Debug.Log("Il paguro non è riuscito a liberarsi!");
                    _state = State.Done;
                }
            }
            else if (_state == State.Releasing)
            {
                player.isGrabbed = false;
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.gravityScale = 1f;
                rb.AddForce(Vector2.down * 50f, ForceMode2D.Impulse);
                Target.Value.GetComponent<Player>().canMove = true;
                Debug.Log("Il paguro è stato rilasciato!");

                _state = State.Returning;
            }
            else if (_state == State.Returning)
            {
                Agent.Value.transform.position = Vector3.MoveTowards(Agent.Value.transform.position, new Vector3(Agent.Value.transform.position.x, startHeight, Agent.Value.transform.position.z), ReturnSpeed.Value * Time.deltaTime);

                if (Mathf.Abs(Agent.Value.transform.position.y - startHeight) < 0.05f)
                {
                    Debug.Log("La mano è tornata al livello iniziale.");
                    _state = State.Done;
                }
            }

            return _state == State.Done ? Status.Success : Status.Running;
        }

        protected override void OnEnd()
        {
            player.isGrabbed = false;
        }
    }
}

