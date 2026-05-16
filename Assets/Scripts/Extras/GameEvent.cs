using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class GameEvent : MonoBehaviour
{
    public static Action<object, EventDataItem> OnEventFired;

    [SerializeField] private MyLibrary.EventTypes eventType;
    [SerializeField] private LayerMask eventLayer;

    public EventDataItem eventData;

    private bool eventFired;
    private bool multiFired;
    private float eventTimeout = 0f;
    private float checkTime;

    private void Awake()
    {
        if (eventData != null)
        {
            eventType = eventData.type;
            multiFired = eventData.multiFired;
            eventTimeout = eventData.fireTimeout;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (MyLibrary.CheckLayer(other.gameObject.layer, eventLayer))
        {
            if (!eventFired)
            {
                OnEventFired?.Invoke(this, eventData);
                eventFired = true;
                if (multiFired && (eventTimeout > 0)) checkTime = Time.time + eventTimeout;
            }
        }
    }

    private void Update()
    {
        if (eventFired && multiFired && (eventTimeout > 0)) 
        {
            if (Time.time > checkTime)  
            {
                eventFired = false;
                checkTime = 0;
            }
        }
    }
}