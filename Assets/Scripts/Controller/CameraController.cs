using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public enum TargetNames
    {
        Forward,     
        Finish

    }


    public Transform allTargetParent;
    public Dictionary<string, Transform> allTarget = new Dictionary<string, Transform>();
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

  

    public GameObject skinPlayer;
    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        foreach (Transform item in allTargetParent)
        {
            allTarget.Add(item.name, item);
        }
        SetTarget(TargetNames.Forward);

    }

    private Vector3 _velocity;
    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
       // Vector3 smoothedPosition = Vector3.Slerp(transform.localPosition, desiredPosition, smoothSpeed * Time.deltaTime);
        //transform.position = smoothedPosition;
          transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, smoothSpeed);
        transform.LookAt(skinPlayer.transform);

      
    }

    public void SetTarget(TargetNames targetName)
    {
        target = allTarget.FirstOrDefault(x => x.Key == targetName.ToString()).Value;

    }
}
