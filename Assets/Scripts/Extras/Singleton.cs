using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;
    private static object lockObj = new object(); // why??

    public static T Instance
    {
        get
        {
            if (applicationIsQuitting)
            {
                Debug.LogWarning(
                    $"[Singleton] Instance '{typeof(T)}' already destroyed on application quit. Returning null.");
                return null;
            }

            lock (lockObj)
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();

                    if (FindObjectsOfType<T>().Length > 1)
                    {
                        Debug.LogError(
                            $"[Singleton] Multiple instances of singleton {{typeof(T)}} detected! This is not allowed.");
                        return instance;
                    }

                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject(typeof(T).Name);
                        instance = singletonObject.AddComponent<T>();
                        DontDestroyOnLoad(singletonObject);
                        Debug.LogError($"[Singleton] An instance of {typeof(T)} was created automatically.");
                    }
                    else
                    {
                        DontDestroyOnLoad(instance.gameObject);
                        Debug.Log($"[Singleton] Using already existing instance of {typeof(T)}.");
                    }
                }

                return instance;
            }
        }
    }

    private static bool applicationIsQuitting = false;

    /// <summary>
    /// If object destroyed manualy or when you exit from the app
    /// forbid to recreate singleton
    /// </summary>

    protected virtual void OnDestroy()
    { 
        if (instance == this)
        {
            applicationIsQuitting = true;
        }
    }
    
    protected virtual void Awake()
    {
        instance = this as T;
    }
}

/*public class Singleton<T> : MonoBehaviour where T : Component
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    GameObject newGameObject = new GameObject();
                    instance = newGameObject.AddComponent<T>();
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}*/