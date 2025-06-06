using System;
using Unity.Properties;
using UnityEngine;

namespace Unity.Behavior
{
    [Serializable, GeneratePropertyBag]
    [NodeDescription(
        name: "SetAnimatorBool",
        description: "Sets a bool parameter on an Animator component.",
        category: "Action/Animation",
        story: "[Target] sets [BoolName] to [Value]",
        id: "7c156e17-2ab1-4494-bc84-fcb15723f000")]
    internal partial class SetAnimatorBool : Action
    {
        [SerializeReference] public BlackboardVariable<GameObject> Target;
        [SerializeReference] public BlackboardVariable<string> BoolName = new("");
        [SerializeReference] public BlackboardVariable<bool> Value = new(false);

        protected override Status OnStart()
        {
            if (Target.Value == null)
            {
                Debug.LogWarning("SetAnimatorBool: Target is null.");
                return Status.Failure;
            }

            Animator animator = Target.Value.GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogWarning("SetAnimatorBool: Animator not found on target.");
                return Status.Failure;
            }

            animator.SetBool(BoolName.Value, Value.Value);
            return Status.Success;
        }

        protected override Status OnUpdate() => Status.Success;
        protected override void OnEnd() { }
    }
}

