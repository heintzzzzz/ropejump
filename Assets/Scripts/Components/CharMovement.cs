using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : CharComponents 
{
    [SerializeField] protected float walkSpeed = 2f; 
    public float MoveSpeed { get; set; }
    private readonly int movingParamater = Animator.StringToHash("Moving");

    private CharFlip charFlip; 
    private RubberBandController rbCont; 
    
    
    
    [Header("Damage")]
    private bool isInjured = false;
    private bool isFallen = false;
    
    
    protected override void Start() 
    {
        base.Start();
        charFlip = character.GetComponent<CharFlip>(); 
        // rbCont = character.GetComponent<RubberBandController>(); 
        MoveSpeed = walkSpeed; 
        
        if (!isPlayer)
        {
 		//	enemyBehaviour = GetComponent<EnemyBehaviour>();
        }  
    }

    private void MoveChar()
    {
        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        Vector2 moveInput = movement; 
        Vector2 movementNormalized = moveInput.normalized;
        Vector2 movementSpeed = movementNormalized * MoveSpeed;
        
        controller.SetMovement(movementSpeed);

        /*float timeBtwTrace = 0.1f;

        if (Time.time > nextResTime && (horizontalInput > 0 || verticalInput > 0 || horizontalInput < 0 || verticalInput < 0))
        {
            Vector2 pos = new Vector2(character.transform.position.x, character.transform.position.y); 
            nextResTime = Time.time + timeBtwTrace;
        }*/
        
    }
    
    /*private void MoveChar()
    {
        Vector2 movement = new Vector2(horizontalInput, 0f);
        Vector2 moveInput = movement;
        Vector2 movementNormalized = moveInput.normalized;
        Vector2 movementSpeed = movementNormalized * MoveSpeed;

        controller.SetMovement(movementSpeed);

		// гubberCtrl.HandleBlock(); 
    }*/

    protected override void HandleAbility()
    {
        base.HandleAbility();
        
        if (isPlayer)
        {
            MoveChar();
        }
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
