using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class SqueezingObstacle : PlatformBase
{
    [Range(.5f, 3f)]
    public float speed = 1f;

    public Transform leftPlatform, rightPlatform;
    public void OnEnable()
    {
        if (GameManager.Instance.allObstacleSpeedChange)
        {
            speed = GameManager.Instance.allObstacleSpeed;
        }

    }
    private void Start()
    {
        Close();
    }
    private void Close()
    {
        leftPlatform.DOLocalMoveX(-(leftPlatform.transform.localScale.x*.5f), .5f / speed).SetEase(Ease.OutBounce).SetDelay(.2f / speed).OnComplete(Open);
        rightPlatform.DOLocalMoveX((leftPlatform.transform.localScale.x * .5f), .5f / speed).SetEase(Ease.OutBounce).SetDelay(.2f / speed);
    }

    private void Open()
    {
        leftPlatform.DOLocalMoveX(-leftPlatform.transform.localScale.x, 2.5f / speed).SetEase(Ease.OutQuart).SetDelay(.4f / speed).OnComplete(Close);
        rightPlatform.DOLocalMoveX(rightPlatform.transform.localScale.x, 2.5f / speed).SetEase(Ease.OutQuart).SetDelay(.4f / speed);
    }
}
