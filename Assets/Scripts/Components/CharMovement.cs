using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : CharComponents 
{
    [SerializeField] protected float walkSpeed = 2f; 
    public float MoveSpeed { get; set; }
    private readonly int movingParamater = Animator.StringToHash("Moving");

    private CharFlip charFlip;

    [Header("Damage")]
    private bool isInjured = false;
    private bool isFallen = false;
    
    
    protected override void Start() 
    {
        base.Start();
        charFlip = character.GetComponent<CharFlip>();
        MoveSpeed = walkSpeed;
        
        if (!isPlayer)
        {
 		//	enemyBehaviour = GetComponent<EnemyBehaviour>();
        }  
    }

    private void MoveChar()
    {
        // Vertical position is driven entirely by the RubberBandController cycle.
        // Horizontal speed is scaled down while hovering at a top/bottom extreme point.
        float horizontalMultiplier = гubberCtrl != null ? гubberCtrl.HorizontalSpeedMultiplier : 1f;

        Vector2 movementSpeed = new Vector2(horizontalInput * horizontalMultiplier, 0f) * MoveSpeed;

        controller.SetMovement(movementSpeed);
    }

    protected override void HandleAbility()
    {
        base.HandleAbility();
        MoveChar();
    }

    private void UpdateAnimations()
    {
        if (Mathf.Abs(horizontalInput) > 0.1f || Mathf.Abs(verticalInput) > 0.1f)
        {
            if (isPlayer && character.CharacterAnimator != null)
            { 
                character.CharacterAnimator.SetBool(movingParamater, true);
                character.CharacterAnimator.SetFloat("Horizontal", horizontalInput);
                character.CharacterAnimator.SetFloat("Vertical", verticalInput);
            } 
        }
        else
        { 
            if (isPlayer && character.CharacterAnimator != null)
            { 
                character.CharacterAnimator.SetBool(movingParamater, false);
                character.CharacterAnimator.SetFloat("Vertical", 0);
                character.CharacterAnimator.SetFloat("Horizontal", 0); 
            } 
        }
    }

    public void ResetSpeed()
    {
        MoveSpeed = walkSpeed;
    }

    public void SetHorizontal(float value)
    {
        horizontalInput = value;
    }

    public void SetVertical(float value)
    {
        verticalInput = value;
    }

    public void DisableGravity()
    {
        Debug.Log("DisableGravity");
        rb.gravityScale = 0f;
    }

    public void EnableGravity()
    {
        Debug.Log("EnableGravity");  
        rb.gravityScale = 10f;
    }

    public void ToggleTrigger(bool value)
    {
        boxCollider.isTrigger = value;
    }  
    
    public void ToggleCollider(bool value)
    {
        boxCollider.enabled = value;
    }
}
