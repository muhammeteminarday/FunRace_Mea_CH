using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class UpDownObstacle : PlatformBase
{
    [Range(.5f, 2f)]
    public float speed = 1f;

    public List<Transform> upDownGo = new List<Transform>();

    public void OnEnable()
    {
        if (GameManager.Instance.allObstacleSpeedChange)
        {
            speed = GameManager.Instance.allObstacleSpeed;
        }

    }
    private void Start()
    {


        foreach (var item in upDownGo)
        {
            MoveUp(item, upDownGo.IndexOf(item),(item.transform.localScale.y));//+1 de komple dışarı çıkıyo o yüzden .5f
        }
    }

    private void MoveUp(Transform upper, int index,float upDownScaleY)
    {
        upper.DOLocalMoveY((upDownScaleY+0.5f), .5f / speed).SetEase(Ease.OutBounce).SetDelay(index / speed + 1 / speed).OnComplete(() => MoveDown(upper, upDownScaleY));
     
    }

    private void MoveDown(Transform upper, float upDownScaleY)
    {
        upper.DOLocalMoveY((-upDownScaleY+1), 1 / speed).SetEase(Ease.InQuart).OnComplete(() => MoveUp(upper, 0, upDownScaleY)).SetDelay(.3f / speed);
   
    }



}
