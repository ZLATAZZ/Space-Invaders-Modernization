using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ObjectPoolManager;
using static Unity.Burst.Intrinsics.X86.Avx;

[DefaultExecutionOrder(-2)]
public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class PoolObject
    {
        public GameObject _objectPrefab;
        public int _amountOfObjectsToPool;
        public TypesOfPoolObjects _typesOfPoolObjects;
        [HideInInspector] public List<GameObject> _pooledObjects;
    }
    public enum TypesOfPoolObjects
    {
        BULLET,
        FIRE,
        ENEMY,
        POWER_UP,
        EXPLOSION,
        HIT_VFX,
        POWER_UP_VFX
    }
    public List<PoolObject> _pools;
    private GameObject _tmp;
    
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
        foreach(PoolObject pool in _pools)
        {
            pool._pooledObjects = new List<GameObject>();
            for(int i = 0; i < pool._amountOfObjectsToPool; i++)
            {
                _tmp = Instantiate(pool._objectPrefab);
                pool._pooledObjects.Add(_tmp);
                _tmp.SetActive(false);
            }
        }
    }
    private GameObject PooledObject(int index)
    {
        for (int i = 0; i < _pools[index]._amountOfObjectsToPool; i++)
        {
            if (!_pools[index]._pooledObjects[i].activeInHierarchy)
            {
                return _pools[index]._pooledObjects[i];
            }
        }
        return null;
    }

    public GameObject GetPooledGameObject(TypesOfPoolObjects typesOfPool)
    {
        switch (typesOfPool)
        {
            case TypesOfPoolObjects.BULLET: return PooledObject((int)TypesOfPoolObjects.BULLET);
            case TypesOfPoolObjects.FIRE: return PooledObject((int)TypesOfPoolObjects.FIRE);
            case TypesOfPoolObjects.ENEMY: return PooledObject((int)TypesOfPoolObjects.ENEMY);
            case TypesOfPoolObjects.POWER_UP: return PooledObject((int)TypesOfPoolObjects.POWER_UP);
            case TypesOfPoolObjects.EXPLOSION: return PooledObject((int)TypesOfPoolObjects.EXPLOSION);
            case TypesOfPoolObjects.HIT_VFX: return PooledObject((int)TypesOfPoolObjects.HIT_VFX);
            case TypesOfPoolObjects.POWER_UP_VFX: return PooledObject((int)TypesOfPoolObjects.POWER_UP_VFX);
            default: return null;
        }
    }
    /// <summary>
    /// Activates a pooled GameObject by setting its position and making it active
    /// </summary>
    public void ActivatePooledGameObject(GameObject pooledObject, Transform pooledObjectPosition)
    {
        if (pooledObject != null)
        {
            pooledObject.transform.position = pooledObjectPosition.position;
            pooledObject.SetActive(true);

        }
    }

    /// <summary>
    /// For game over: deactivate all objects, except Explosion
    /// </summary>
    public void DeactivatePooledObjects()
    {
        foreach (PoolObject pool in _pools)
        {
            foreach (GameObject pooledObject in pool._pooledObjects)
            {
                if(pool._typesOfPoolObjects != TypesOfPoolObjects.EXPLOSION)
                {
                    pooledObject.SetActive(false);
                }
            }
        }
    }
}
