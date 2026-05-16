using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2D : Singleton<Camera2D>
{
    private enum CameraMode
    {
        Update,
        FixedUpdate,
        LateUpdate
    }

    public Transform target;
    public Vector2 Offset { get; set; }
    public Vector2 PlayerOffset => offset;
    
    [Header("Offset")] 
    [SerializeField] private Vector2 offset;

    [Header("Mode")] 
    [SerializeField] private CameraMode cameraMode = CameraMode.Update;

    protected override void Awake()
    {
        base.Awake();
        Offset = offset;
    }

    private void Update()
    {
        if (cameraMode == CameraMode.Update)
        {
            FollowTarget();
        }
    }

    private void FixedUpdate()
    {
        if (cameraMode == CameraMode.FixedUpdate)
        {
            FollowTarget();
        }
    }

    private void LateUpdate()
    {
        if (cameraMode == CameraMode.LateUpdate)
        {
            FollowTarget();
        }
    }

    private void FollowTarget()
    {
        Vector3 desiredPosition = new Vector3(target.position.x + Offset.x, target.position.y + Offset.y, transform.position.z);
        transform.position = desiredPosition;
    }
}
