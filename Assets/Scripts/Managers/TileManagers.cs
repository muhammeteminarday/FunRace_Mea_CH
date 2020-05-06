using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class TileManagers : MonoBehaviour
{
    public static TileManagers Instance;
    private ObjectPooler objectPooler;
    public GameObject currentTile;
    public List<GameObject> platformPrefabs = new List<GameObject>();
    public List<GameObject> obstaclePrefabs = new List<GameObject>();
    private bool obstacleSpawnControl;
    public GameObject finishPlatformPrefab, startPlatformPrefab;
    public List<GameObject> activeAllPlatform = new List<GameObject>();

    public void Awake()
    {
        Instance = this;
    }
    public void Start()
    {
        objectPooler = ObjectPooler.Instance;
        //  CreateLevel();

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            CreateLevel();

        }


    }
    public void CreateLevel()
    {
        startPlatformPrefab.transform.position = finishPlatformPrefab.transform.position;
        currentTile = startPlatformPrefab;
        ClearOldLevelList();
        Invoke("ActiveAllPlatformPool", 2);
        Invoke("StartCreateAnim", 3);
      
        //StartCreateAnim();


    }
    public void StartCreateAnim()
    {
        activeAllPlatform.Clear();
        for (int i = 0; i < GameManager.Instance.levelLenght; i++)
        {
            SpawnTile();

        }

        FinishPrefabTransform();
        PlayerMoveController.Instance.nowPointIndex = 0;
        Invoke("StartGame", 2);
    }
    public void StartGame()
    {
        AiController.Instance.isFinished = false;//Bir Ai olduğu için Singleton yaptım. 
        GameManager.Instance.isFinish = false;
    }


    public void ClearOldLevelList()
    {      

        for (int i = (activeAllPlatform.Count - 1); i >= 0; i--)
        {

            activeAllPlatform[i].transform.DOMoveY(activeAllPlatform[i].transform.position.y - 20, 1).SetDelay((activeAllPlatform.Count-i) * .3f).SetEase(Ease.InQuart);

        }

        WayPointsManager.Instance.wayPoints.Clear();
    }
    public void ActiveAllPlatformPool()
    {
        foreach (var item in activeAllPlatform)
        {
            objectPooler.ReturnToPool(item.name, item);
        }
    }


    public void FinishPrefabTransform()
    {

        finishPlatformPrefab.transform.position = currentTile.GetComponent<PlatformBase>().upPosTarget.position;
        finishPlatformPrefab.transform.DOLocalMoveY(-1, 2.5f / 1).SetEase(Ease.OutBounce);
        foreach (Transform item in finishPlatformPrefab.GetComponent<PlatformBase>().waypointParent)
        {
            WayPointsManager.Instance.wayPoints.Add(item.transform);
        }
    }
    public void SpawnTile()
    {

        if (GameManager.Instance.obstacleSpawnRate>Random.value)
        {
            ObstacleSpawn();
        }
        else
        {
            NormalPlatformSpawn();
         
        }


    }
    public void NormalPlatformSpawn()
    {
        int randomIndex = Random.Range(0, platformPrefabs.Count);
        if (obstacleSpawnControl)
        {
            randomIndex = 1;
            obstacleSpawnControl = false;
        }

        Spawn(platformPrefabs[randomIndex]);
    }
    public void ObstacleSpawn()
    {

        int randomIndex = Random.Range(0, obstaclePrefabs.Count);
        Spawn(obstaclePrefabs[randomIndex]);
        obstacleSpawnControl = true;

    }
    public void Spawn(GameObject spawnPrefab)
    {

        GameObject newObj = objectPooler.SpawnFromPool(spawnPrefab.name, Vector3.zero, Quaternion.identity);//Pool
        newObj.name = spawnPrefab.name;
        PlatformBase newObjBase = newObj.GetComponent<PlatformBase>();
        if (newObjBase.Type == PoolTag.Up)
        {
            newObj.transform.position = currentTile.GetComponent<PlatformBase>().upPosTarget.position;

        }
        if (newObjBase.Type == PoolTag.Left)
        {
            newObj.transform.position = currentTile.GetComponent<PlatformBase>().leftPosTarget.position;
        }

        foreach (Transform item in newObjBase.waypointParent)
        {
            WayPointsManager.Instance.wayPoints.Add(item.transform);
        }


        newObj.transform.position = new Vector3(newObj.transform.position.x, 15, newObj.transform.position.z);




        currentTile = newObj;

        activeAllPlatform.Add(currentTile);
        newObj.transform.DOMoveY(-1, 1).SetDelay(activeAllPlatform.IndexOf(currentTile) * .3f).SetEase(Ease.InQuart);


    }






    //public IEnumerator ShowInstantiate(GameObject GO)
    //{
    //    float exitTime = 0;
    //    Vector3 oldPos = new Vector3(GO.transform.position.x, -1, GO.transform.position.z);
    //    GO.transform.position += new Vector3(0, Random.Range(1, 10), 0);
    //    while (exitTime < 5)
    //    {
    //        GO.transform.position = Vector3.Slerp(GO.transform.position, oldPos, 2 * Time.deltaTime);
    //        exitTime += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //    GO.transform.position = oldPos;

    //}
}
