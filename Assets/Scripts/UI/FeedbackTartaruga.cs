using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackTartaruga : MonoBehaviour
{
    [Header("ANIMAZIONE")]


    [Header("MOVIMENTO")]
    public float swimDuration = 5f;
    public float pauseDuration = 2f;
    public float exitOffset = 200f;
    public Vector2 targetPosition;

    [Header("RIFERIMENTI")]
    public RectTransform canvasRect;
    public RectTransform rectTransform;
    private Sequence swimSequence;
    private Animator animator;



    void Awake()
    {
        animator = GetComponent<Animator>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void StartSwim()
    {
        swimSequence?.Kill();

        float canvasWidth = canvasRect.rect.width;
        float turtleWidth = rectTransform.rect.width;

        Vector2 startPos = new Vector2(canvasWidth / 2 + turtleWidth + exitOffset, targetPosition.y);
        Vector2 finalPos = new Vector2(-canvasWidth / 2 - turtleWidth - exitOffset, targetPosition.y);

        rectTransform.anchoredPosition = startPos;

        animator.SetBool("isMoving", true);

        swimSequence = DOTween.Sequence();
        swimSequence.Append(rectTransform.DOAnchorPos(targetPosition, swimDuration / 2f).SetEase(Ease.Linear));
        swimSequence.AppendCallback(() =>
        {
            animator.SetBool("isMoving", false); 
        });
        swimSequence.AppendInterval(pauseDuration);
        swimSequence.AppendCallback(() =>
        {
            animator.SetBool("isMoving", true); 
        });
        swimSequence.Append(rectTransform.DOAnchorPos(finalPos, swimDuration / 2f).SetEase(Ease.Linear));

        swimSequence.OnComplete(() =>
        {
            animator.SetBool("isMoving", false); 
        });
    }
}
