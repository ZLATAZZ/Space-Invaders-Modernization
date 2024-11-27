using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [SerializeField] private Projectile bulletPrefab;
    [SerializeField] private int amountOfObjectsToPool;
    private List<GameObject> pooledObjects;
    
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
        GameObject tmp;
        pooledObjects = new List<GameObject>();

        for(int i = 0; i < amountOfObjectsToPool; i++)
        {
            tmp = Instantiate(bulletPrefab.gameObject);
            pooledObjects.Add(tmp);
            tmp.SetActive(false);
        }
    }

    public GameObject GetPooledGameObject()
    {
        for (int i = 0; i < amountOfObjectsToPool; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        return null;
    }
    
}
