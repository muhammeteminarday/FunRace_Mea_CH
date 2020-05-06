using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObstacle : PlatformBase
{
    public bool isClockWise;

    [Range(.5f, 3f)]
    public float speed = 1f;

    public Transform rotatorGO;


    public void OnEnable()
    {
        isClockWise = (Random.value < 0.5);
        if (GameManager.Instance.allObstacleSpeedChange)
        {
            speed = GameManager.Instance.allObstacleSpeed;
        }

    }

    private void Update()
    {
        RotateStart();
    }
    public void RotateStart()
    {
        rotatorGO.eulerAngles += (isClockWise ? 1f : -1f) * Vector3.up * speed;
    }
}

