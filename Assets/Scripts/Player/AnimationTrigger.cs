using UnityEngine;
using UnityEngine.Rendering;

public class AnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator reteAnimator;
    [SerializeField] private Animator mamaAnimator;

    public void TriggerNetAnimation()
    {
        reteAnimator.SetBool("rete", true);
    }

    public void TriggerMomAnimation()
    {
        mamaAnimator.SetBool("cammina", true);
    }
}
