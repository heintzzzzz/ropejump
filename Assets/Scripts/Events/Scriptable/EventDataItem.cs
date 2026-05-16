using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/eventDataItem", fileName = "Enemy Data Item")]
public class EventDataItem : ScriptableObject
{  
    public enum EventTypes
    {
        EnemySpawn,
        ItemSpawn, 
    }

    public MyLibrary.EventTypes type; 
    public bool multiFired = true;
    public float fireTimeout = 20f;
    public GameObject item;
    public GameObject pos;
}