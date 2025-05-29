using UnityEngine;
using DG.Tweening;
 
public class Fader : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float fadeDuration = .5f;
 
    public void Fade(bool fadeDirection)
    {
        float targetAlpha = fadeDirection ? 1f : 0f;
        spriteRenderer.DOFade(targetAlpha, fadeDuration);
    }
}