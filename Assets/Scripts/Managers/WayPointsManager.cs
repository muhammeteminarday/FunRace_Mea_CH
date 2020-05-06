using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointsManager : MonoBehaviour
{
    public static WayPointsManager Instance;
    [System.NonSerialized]
    public List<Transform> wayPoints = new List<Transform>();
    void Awake()
    {
        Instance = this;
    }
}
