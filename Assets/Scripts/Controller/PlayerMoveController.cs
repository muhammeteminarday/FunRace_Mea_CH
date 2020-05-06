using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMoveController : MonoBehaviour
{
    public static PlayerMoveController Instance;
    private GameManager gameManager;
    private WayPointsManager wayPointsManager;
    private CameraController cameraController;

    public enum AnimList
    {
        Idle,
        Run
    }
    public AnimList animNowSelect;
  

    //[System.NonSerialized]
    //public List<Transform> wayPoints = new List<Transform>();

    [System.NonSerialized]
    public int nowPointIndex;
    //public float moveSpeed = 3.0f;

    [System.NonSerialized]
    public float rotateSpeed = 0.1f;

    private float wayPointNear = 0.1f;
    private Animator anim;

    [System.NonSerialized]
    public Vector3 startPos, deadPos;

    public void Awake()
    {
        Instance = this;
    }
    void Start()
    {
     
        gameManager = GameManager.Instance;
        cameraController = CameraController.Instance;
        wayPointsManager = WayPointsManager.Instance;
        anim = GetComponent<Animator>();
        RagdollOn(false);

    }



    void Update()
    {
        if (GameManager.Instance.isDead)
        {
            return;
        }
        if (GameManager.Instance.isFinish)
        {
            PlayAnim(AnimList.Idle);//Dance Animasyonu olsaydı katılabilirdi
            return;
        }
        if (wayPointsManager.wayPoints.Count == 0)
        {
          
            return;
        }

        if (Input.GetMouseButton(0))
        {
            LookControl();
            PlayAnim(AnimList.Run);
            transform.position = Vector3.MoveTowards(transform.position, wayPointsManager.wayPoints[nowPointIndex].position, Time.deltaTime * GameManager.Instance.playerMoveSpeed);

            if (Vector3.Distance(this.transform.position, wayPointsManager.wayPoints[nowPointIndex].position) < wayPointNear)
            {
                if (nowPointIndex + 1 < wayPointsManager. wayPoints.Count)
                    nowPointIndex++;

            }
        }

        if (Input.GetMouseButtonUp(0))
        {

            PlayAnim(AnimList.Idle);
        }
     


    }
    private Vector3 _rotVelocity;
    public void LookControl()
    {


        transform.LookAt(Vector3.SmoothDamp(transform.position + transform.forward, wayPointsManager.wayPoints[nowPointIndex].position, ref _rotVelocity, rotateSpeed, 20));

    }
    public void OnTriggerEnter(Collider other)
    {


        switch (other.gameObject.tag)
        {
            case "Enemy":
                IsDead();
                break;

            case "Finish":

                FinishGame();
                break;
            case "MovingPlatform":
                gameObject.transform.parent = other.transform;
                break;


            default:
                break;
        }
    }
    public void FinishGame()
    {
        startPos = transform.position;
        TileManagers.Instance.CreateLevel();
        GameManager.Instance.NextLevel();
        GameManager.Instance.isFinish = true;
        AiController.Instance.LoseGame(startPos);
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


    public void PlayAnim(AnimList animName)
    {
      

        if (animNowSelect==animName)
        {
            return;
        }

        foreach (AnimList item in (AnimList[])Enum.GetValues(typeof(AnimList)))
    
        {
            anim.SetBool(item.ToString(), item == animName);
        }
        animNowSelect = animName;


    }

    public void IsDead()
    {
        GameManager.Instance.Dead();
        deadPos = transform.position;
        gameManager.isDead = true;
        RagdollOn(true);

        Invoke("RespawnOrRestrartGameControl", 1.5f);
       
    }
    public void RespawnOrRestrartGameControl()
    {
        gameObject.transform.parent = null;
        if (GameManager.Instance.rightToLife<=0)
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
        gameManager.isDead = false;
        PlayAnim(AnimList.Idle);
        Vector3 calculate = deadPos + new Vector3(0, 0, -5);
        calculate.z = Mathf.Clamp((calculate.z), 0, calculate.z);
        calculate.y = 0;
       

        transform.position = calculate;

    }
    public void RestartGame()
    {
        RagdollOn(false);
        gameManager.isDead = false;
        PlayAnim(AnimList.Idle);
        transform.position = startPos;
        nowPointIndex = 0;
        GameManager.Instance.RightToLife = 3;
    }





    private void RagdollOn(bool value)
    {
        GetComponent<CapsuleCollider>().enabled = !value;
        GetComponent<Animator>().enabled = !value;
        GetComponent<Rigidbody>().isKinematic = value;

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            rb.isKinematic = !value;

    }

}
