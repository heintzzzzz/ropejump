using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolCtrl : MonoBehaviour
{
    [SerializeField] private GameObject prefub;
    [SerializeField] private int initialSize = 12;
    private List<GameObject> pool = new List<GameObject>();

    private GameObject parentObject; 
    [SerializeField] public string title = "ItemPool";
    [SerializeField] private bool poolCanExpand = true;
    
    private void Start()
    {
        parentObject = new GameObject(title);
        Refill();
    }
    
    public void Refill()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            AddObjectToPool();
        }
    }
    
    public GameObject AddObjectToPool()
    {
        GameObject obj = Instantiate(prefub);
        obj.SetActive(false);
        obj.transform.parent = parentObject.transform;
        pool.Add(obj);
        return obj; 
    }

    public GameObject GetObject()
    {
        if (pool.Count > 0)
        {
            for (int i = 0; i < pool.Count; i++)
            {
                if (!pool[i].activeInHierarchy)
                {
                    pool[i].SetActive(true);
                    return pool[i];
                }
            }
        }
    
        if (poolCanExpand)
        {
            return AddObjectToPool();
        }

        return null;
    }

    public void ReturnObject(GameObject obj) 
    {
        obj.SetActive(false);
    }
    
    public void SetTitle(string value)
    {
        if (value != null) title = value;
    }
}























