using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] private GameObject objectPrefub; // prefub for pool
    [SerializeField] private int poolSize = 10; // default object pool size
    [SerializeField] private bool poolCanExpand = true; // can this pool be bigger or not
    [SerializeField] public string title = "Pool";

    private List<GameObject> pooledObjects;
    private GameObject parentObject; 

    public void Start()
    {
        parentObject = new GameObject(title); // what is this??? why
        Refill();
    }

    public void Refill()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            AddObjectToPool();
        }
    }

    public GameObject GetObjectFromPool()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        if (poolCanExpand)
        {
            return AddObjectToPool();
        }

        return null;
    }

    public GameObject AddObjectToPool()
    {
        GameObject newObject = Instantiate(objectPrefub);
        newObject.SetActive(false);
        newObject.transform.parent = parentObject.transform;

        pooledObjects.Add(newObject);
        return newObject;
    }

    public void SetTitle(string value)
    {
        if (value != null) title = value;
    }
}