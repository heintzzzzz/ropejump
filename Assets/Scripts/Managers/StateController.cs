using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [Header("State")] 
    [SerializeField] private AIState currentState;

    [SerializeField] private AIState remainState; 

    [Header("Field of view")] 
    [SerializeField] private UnityEngine.Rendering.Universal.Light2D fieldView;
    public Transform Target { get; set; }
    public CharMovement CharMovement { get; set; }
    
    public CharWeapon CharWeapon { get; set; }
    public CharFlip CharFlip { get; set; }

    public Vector3 TargetPoint { get; set; }

    public UnityEngine.Rendering.Universal.Light2D FieldOfView => fieldView; 

    public Transform Player { get; set; }

    public Collider2D Collider2D { get; set; }
 
    private void Awake()
    {
        CharMovement = GetComponent<CharMovement>();
        CharFlip = GetComponent<CharFlip>();
        CharWeapon = GetComponent<CharWeapon>();
        
        Collider2D = GetComponent<Collider2D>();

        Player = GameObject.FindWithTag("Player").transform;
    }
    
    private void Update()
    {
        if (currentState != null) currentState.EvaluateState(this);
    }

    public void TransitionToState(AIState nextState) 
    {
        if (nextState != remainState)
        {
            if (nextState != null) currentState = nextState;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Player != null) {
            Gizmos.DrawLine(transform.position, Player.position);
        }
    }
}





































































