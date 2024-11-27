using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPoolManager;
using static Unity.Burst.Intrinsics.X86.Avx;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolObject
    {
        public GameObject[] objectPrefabs;
        public int amountOfObjectsToPool;
        public TypesOfPoolObjects typesOfPoolObjects;
        [HideInInspector] public List<GameObject> pooledObjects;
    }
    public enum TypesOfPoolObjects
    {
        Bullet,
        Fire,
        Enemy,
        PowerUp,
    }
    public List<PoolObject> pools;
    private GameObject tmp;
    
    public static ObjectPoolManager Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        CreateObjectsToPool();
    }

    private void CreateObjectsToPool()
    {
        foreach(PoolObject pool in pools)
        {
            pool.pooledObjects = new List<GameObject>();
            for(int i = 0; i < pool.amountOfObjectsToPool; i++)
            {
                for(int j = 0; j<pool.objectPrefabs.Length; j++)
                {
                    tmp = Instantiate(pool.objectPrefabs[j]);
                    pool.pooledObjects.Add(tmp);
                    tmp.SetActive(false);
                }
            }
        }
    }
    private GameObject ActivatePooledObject(int index)
    {
        for (int i = 0; i < pools[index].amountOfObjectsToPool; i++)
        {
            if (!pools[index].pooledObjects[i].activeInHierarchy)
            {
                return pools[index].pooledObjects[i];
            }
        }
        return null;
    }

    public GameObject GetPooledGameObject(TypesOfPoolObjects typesOfPool)
    {
        switch (typesOfPool)
        {
            case TypesOfPoolObjects.Bullet: return ActivatePooledObject((int)TypesOfPoolObjects.Bullet);
            case TypesOfPoolObjects.Fire: return ActivatePooledObject((int)TypesOfPoolObjects.Fire);
            case TypesOfPoolObjects.Enemy: return ActivatePooledObject((int)TypesOfPoolObjects.Enemy);
            case TypesOfPoolObjects.PowerUp: return ActivatePooledObject((int)TypesOfPoolObjects.PowerUp);
            default: return null;
        }
    }
}
