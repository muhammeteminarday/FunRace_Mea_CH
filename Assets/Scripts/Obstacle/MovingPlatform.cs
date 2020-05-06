using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPlatform : PlatformBase
{
    [Range(.5f, 2f)]
    public float speed = 1f;
    public Transform platformGO;
    private float platformScaleX;
    public void OnEnable()
    {
        if (GameManager.Instance.allObstacleSpeedChange)
        {
            speed = GameManager.Instance.allObstacleSpeed;
        }

    }
    public void Start()
    {
        LeftGo();
        platformScaleX = platformGO.transform.position.x;

    }
    private void LeftGo()
    {
        platformGO.DOLocalMoveX(-platformScaleX, 2.5f / speed).SetEase(Ease.Linear).OnComplete(RightGo);
    }

    private void RightGo()
    {
        platformGO.DOLocalMoveX(platformScaleX, 2.5f / speed).SetEase(Ease.Linear).OnComplete(LeftGo);
    }
}
