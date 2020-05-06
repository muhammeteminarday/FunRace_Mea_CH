using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Range(0, 10)]
    public float playerMoveSpeed = 3;

    [Range(0, 10)]
    public float aiPlayerMoveSpeed = 3;

    public bool autoIncreasingDifficulty;

    [Range(0, 1)]
    public float obstacleSpawnRate;

    private float autoObstacleSpawnRateValue = 0.1f; //
    public float ObstacleSpawnRate
    {
        get
        {
            float rate = Mathf.Clamp((obstacleSpawnRate + autoObstacleSpawnRateValue), 0, 1);

            return rate;
        }
        set
        {
            obstacleSpawnRate = value;

        }
    }


    public int levelId;
    public int LevelId
    {
        get
        {
            return levelId + 1;
        }
        set
        {
            levelId = value;
            UiManager.Instance.SetLevelIdText(levelId);
        }
    }


    public int rightToLife;
    public int RightToLife
    {
        get
        {
            return rightToLife - 1;
        }
        set
        {
            rightToLife = value;
            UiManager.Instance.RightToLifeText(rightToLife);
        }
    }

    public int levelLenght;
    public int LevelLenght
    {
        get
        {
            return levelLenght + 1;
        }
        set
        {
            levelLenght = value;

        }
    }

    [System.NonSerialized]
    public bool isFinish, isDead;


    public bool allObstacleSpeedChange;
    [Range(0, 10)]
    public float allObstacleSpeed;


    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        levelId = 0;
        RightToLife = 3;

    }


    public void NextLevel()
    {
        levelId = LevelId;
        UiManager.Instance.SetLevelIdText(levelId);

        if (autoIncreasingDifficulty)
        {
            AutoIncreasingDifficulty();
        }

        UiManager.Instance.SetLevelLenghtText(levelLenght);
    }
    public int LevelIncrease()
    {
        return levelId + 1;
    }


    public void Dead()
    {
        rightToLife = RightToLife;
        UiManager.Instance.RightToLifeText(rightToLife);
    }

    public void AutoIncreasingDifficulty()
    {
        levelLenght = LevelLenght;
        obstacleSpawnRate = ObstacleSpawnRate;
    }



}
