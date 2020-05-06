using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolGameObjectInfo
    {
        public string poolGameobjectName;
        public GameObject poolPrefab;
        public int startSize;
    }

    public List<PoolGameObjectInfo> poolsList= new List<PoolGameObjectInfo>();
    public Dictionary<string, Queue<GameObject>> poolDictionaryList;

    #region Singleton
    public static ObjectPooler Instance;
    public void Awake()
    {
        Instance = this;
    }
    #endregion

    void Start()
    {
        poolsList = new List<PoolGameObjectInfo>();
        poolDictionaryList = new Dictionary<string, Queue<GameObject>>();
        PoolGameObjectInfoListAdd();

        foreach (var pool in poolsList)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < pool.startSize; i++)
            {
                GameObject obj = Instantiate(pool.poolPrefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionaryList.Add(pool.poolGameobjectName, objectPool);
        }
    }

    public void PoolGameObjectInfoListAdd()
    {
      
        foreach (var item in Resources.LoadAll<GameObject>("Prefabs"))
        {
            PoolGameObjectInfo newPoolInfo = new PoolGameObjectInfo();
            newPoolInfo.poolGameobjectName = item.gameObject.name;
            newPoolInfo.poolPrefab = item;
            poolsList.Add(newPoolInfo);

        }


    }
    public GameObject SpawnFromPool(string objectName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionaryList.ContainsKey((objectName)))
        {
            return null;
        }

        if (poolDictionaryList[objectName].Count == 0)
        {
            AddObject(1, objectName);
        }

        GameObject objectToSpawn = poolDictionaryList[objectName].Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        IPooledObject pooledObject = objectToSpawn.GetComponent<IPooledObject>();
        if (pooledObject != null)
        {
            pooledObject.OnObjectSpawn
                ();
        }
        return objectToSpawn;
      
    }

    public void ReturnToPool(string tag, GameObject objectReturn)
    {
        if (objectReturn==null)
        {
            return;
        }
        objectReturn.SetActive(false);
        poolDictionaryList[tag].Enqueue(objectReturn);

    }

    public void AddObject(int count, string tag)
    {
        int detectIndex = poolsList.FindIndex(x => x.poolGameobjectName == tag);
        GameObject newObject = Instantiate(poolsList[detectIndex].poolPrefab);
        newObject.SetActive(false);
        poolDictionaryList[tag].Enqueue(newObject);

        //IPooledObjectMea pooledObject = newObject.GetComponent<IPooledObjectMea>();
        //if (pooledObject != null)
        //{
        //    pooledObject.OnObjectSpawn
        //        ();
        //}

    }
}
