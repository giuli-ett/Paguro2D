using UnityEngine;

public class Rete : MonoBehaviour
{
    [SerializeField] private Animator mammaAnimator;

    public void OnPesca()
    {
        mammaAnimator.SetBool("mamma", true);
    }

    public void OnSali()
    {
        mammaAnimator.SetBool("mamma", false);
        gameObject.GetComponent<Animator>().SetBool("rete", false);
        Destroy(mammaAnimator.gameObject);
    }
}
