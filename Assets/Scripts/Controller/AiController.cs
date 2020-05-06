using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AiController : MonoBehaviour
{
    public bool isDead, isFinished;
    public static AiController Instance;

    private WayPointsManager wayPointsManager;

    public enum AiAnimList
    {
        Idle,
        Run
    }
    public AiAnimList animNowSelect;





    public int nowPointIndex;
    //public float moveSpeed = 3.0f;

    [System.NonSerialized]
    public float rotateSpeed = 0.1f;

    private float wayPointNear = 0.1f;
    private Animator anim;

    [System.NonSerialized]
    public Vector3 startPos, deadPos;
    private Vector3 offsetWayPoint;

    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {

        offsetWayPoint = transform.position;
        wayPointsManager = WayPointsManager.Instance;

        anim = GetComponent<Animator>();
        RagdollOn(false);

    }



    void Update()
    {
        if (isDead)
        {
            return;
        }
        if (isFinished)
        {
            PlayAnim(AiAnimList.Idle);//Dance Animasyonu olsaydı katılabilirdi
            return;
        }
        if (wayPointsManager.wayPoints.Count == 0)
        {


            return;
        }
        RaycastControl();

        LookControl();


    }
    private RaycastHit hit;
    public GameObject rayTransform;
    private float rayLenght = 3;
    public void RaycastControl()
    {
        Ray enemyRay = new Ray(rayTransform.transform.position, Vector3.forward * rayLenght);
        Debug.DrawRay(rayTransform.transform.position, Vector3.forward * rayLenght, Color.red);
        if (Physics.Raycast(enemyRay, out hit, rayLenght))
        {

            if (hit.collider.gameObject.tag == "Enemy")
            {
                Stop();
            }
            else
            {
                AutoMove();
            }

        }
        else
        {

            AutoMove();
        }

    }
    public void AutoMove()
    {
        PlayAnim(AiAnimList.Run);

        Vector3 wayPointAddOffSetCalculate = wayPointsManager.wayPoints[nowPointIndex].position + offsetWayPoint;
        transform.position = Vector3.MoveTowards(transform.position, wayPointAddOffSetCalculate, Time.deltaTime * GameManager.Instance.aiPlayerMoveSpeed);

        if (Vector3.Distance(this.transform.position, wayPointAddOffSetCalculate) < wayPointNear)
        {
            if (nowPointIndex + 1 < wayPointsManager.wayPoints.Count)
                nowPointIndex++;

        }
    }
    public void Stop()
    {
        PlayAnim(AiAnimList.Idle);
    }

    private Vector3 _rotVelocity;
    public void LookControl()
    {

        Vector3 wayPointAddOffSetCalculate = wayPointsManager.wayPoints[nowPointIndex].position + offsetWayPoint;
        transform.LookAt(Vector3.SmoothDamp(transform.position + transform.forward, wayPointAddOffSetCalculate, ref _rotVelocity, rotateSpeed, 20));

    }
    public void OnTriggerEnter(Collider other)
    {


        switch (other.gameObject.tag)
        {
            case "Enemy":
                IsDead();
                break;

            case "Finish":
                startPos = transform.position;
                isFinished = true;
                //TileManagers.Instance.CreateLevel();
                //GameManager.Instance.NextLevel();

                break;
            case "MovingPlatform":
                gameObject.transform.parent = other.transform;
                break;


            default:
                break;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "MovingPlatform":
                gameObject.transform.parent = null;
                break;
            default:
                break;
        }
    }


    public void PlayAnim(AiAnimList animName)
    {


        if (animNowSelect == animName)
        {
            return;
        }

        foreach (AiAnimList item in (AiAnimList[])Enum.GetValues(typeof(AiAnimList)))

        {
            anim.SetBool(item.ToString(), item == animName);
        }
        animNowSelect = animName;


    }
    private int rightToLife;
    public void IsDead()
    {
        rightToLife -= 1;
        deadPos = transform.position;
        isDead = true;
        RagdollOn(true);

        Invoke("RespawnOrRestrartGameControl", 1.5f);

    }

    public void RespawnOrRestrartGameControl()
    {
        gameObject.transform.parent = null;
        if (rightToLife <= 0)
        {
            RestartGame();
        }
        else
        {
            Respawn();
        }
    }


    public void Respawn()
    {
        RagdollOn(false);
        isDead = false;
        PlayAnim(AiAnimList.Idle);
        Vector3 calculate = deadPos + new Vector3(0, 0, -5);
        calculate.z = Mathf.Clamp((calculate.z), 0, calculate.z);
        calculate.y = 0;
        transform.position = calculate;

    }
    public void RestartGame()
    {
        RagdollOn(false);
        isDead = false;
        PlayAnim(AiAnimList.Idle);
        transform.position = startPos;
        nowPointIndex = 0;
        rightToLife = 3;
    }





    private void RagdollOn(bool value)
    {
        GetComponent<CapsuleCollider>().enabled = !value;
        GetComponent<Animator>().enabled = !value;
        GetComponent<Rigidbody>().isKinematic = value;

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            rb.isKinematic = !value;

    }
    public void LoseGame(Vector3 startPos)
    {
        nowPointIndex = 0;
        Vector3 calculateStartPos = startPos + offsetWayPoint;

        transform.position = calculateStartPos;
    }

}
