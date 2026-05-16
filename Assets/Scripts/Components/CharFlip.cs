using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharFlip : CharComponents
{
    [SerializeField] public MyLibrary.FlipMode flipMode = MyLibrary.FlipMode.MovementDirection;
    [SerializeField] private float threshold = 0.1f;
    
    public bool FacingRight { get; set; } 

    [SerializeField] private Transform fieldOfView;

    [SerializeField] public StateController stc;
    
    // public Weapon currentWeapon;

    private void Awake()
    {
        FacingRight = true;
 
        stc = GetComponent<StateController>();  


        // currentWeapon = GetComponent<CharWeapon>().CurrentWeapon;  
    }
    
    protected override void HandleAbility()
    {
        base.HandleAbility();
        if (flipMode == MyLibrary.FlipMode.MovementDirection)  
        {
            if (isPlayer)
            {
                FlipToMoveDirection();
            }
            else
            {
                NpcFlipToMoveDirection();
            }
        }
        else
        {
            if (isPlayer)
            {
                FlipToWeaponDirection();
            }
            else
            {
                /*if (charMovement.isPatrolling)
                {
                    NpcFlipToMoveDirection(); 
                }
                else
                {
                    NpcFlipToWeaponDirection(); 
                }*/
            } 

        }
    } 
    
    private void FlipToMoveDirection()
    {
        if (controller.CurrentMovement.normalized.magnitude > threshold)
        {
            if(controller.CurrentMovement.normalized.x > 0) 
            {
                FaceDirection(1);
            }
            else 
            {
                FaceDirection(-1);
            }
        }
    }
    
    private void NpcFlipToMoveDirection() 
    { 

        /*if (charMovement.aiPath.canMove && charMovement.aiPath.velocity.sqrMagnitude > 0.01f)
        {
            bool movingRight = charMovement.aiPath.velocity.x > 0f;

            if (movingRight != FacingRight)
            {
                FacingRight = movingRight;
                character.CharacterSprite.GetComponent<SpriteRenderer>().flipX = !FacingRight;
                setViewFieldDirection();
            }
        } */
    }

    private void FlipToWeaponDirection()
    {
        
        if (charWeapon != null)
        {
            float weaponAngle = charWeapon.WeaponAim.CurrentAimAngleAbsolute;
            
            if (weaponAngle > 90 || weaponAngle < -90)
            {
                FaceDirection(-1);
            }
            else
            {
                FaceDirection(1);
            }
        }
        
        /*if (charWeapon != null)
        {
            float weaponAngle = charWeapon.WeaponAim.CurrentAimAngleAbsolute;
            
            if (weaponAngle > 90 || weaponAngle < -90)
            {
                FaceDirection(-1);
            }
            else
            {
                FaceDirection(1);
            }
        }*/
    }

    private void NpcFlipToWeaponDirection()
    {
        if (charWeapon != null)
        {
            float weaponAngle = charWeapon.WeaponAim.CurrentAimAngleAbsolute;
            
            if (weaponAngle > 90 || weaponAngle < -90)
            {
                FaceDirection(-1);
            }
            else
            {
                FaceDirection(1);
            }
        }
    }
    
    /*private void RotateToMoveDirection()
    {
        if (controller.CurrentMovement.normalized.magnitude > threshold)
        {
            if(controller.CurrentMovement.normalized.x > 0 || controller.CurrentMovement.normalized.y > 0) 
            {
                VerticalFlip();
            }
            else if(controller.CurrentMovement.normalized.x < 0 || controller.CurrentMovement.normalized.y < 0) 
            {
                VerticalFlip();
            }
        
        } else {
            // ResetMeleeFlip();
        }
    }*/

	public void FaceDirection(int newDirection)
    {
        /*if (newDirection == 1)
        { 
            character.CharacterSprite.transform.localScale = new Vector3(1,1,1);
            FacingRight = true;
        }
        else
        {
            character.CharacterSprite.transform.localScale = new Vector3(-1,1,1);
            FacingRight = false;
        }*/
        Transform chTransform = character.CharacterSprite.GetComponent<Transform>();
        
        if (newDirection == 1) 
        {
            // characterSpriteRenderer.flipX = false;
            chTransform.localScale = new Vector3(1,1,1);
            FacingRight = true;
        }
        else
        {
            // characterSpriteRenderer.flipX = true;  
            chTransform.localScale = new Vector3(-1,1,1); 
            FacingRight = false;
        } 

        setViewFieldDirection();
    }

    private void setViewFieldDirection()
    {
        if (fieldOfView != null) 
        {
            Quaternion viewRotation = fieldOfView.transform.rotation;
            
            float angle = -90f;
            if (FacingRight == false) { 
                angle = 90f;
                fieldOfView.rotation = Quaternion.Euler(fieldOfView.position.x, fieldOfView.position.y, angle);

            } else { 
                angle = -90f;
                fieldOfView.rotation = Quaternion.Euler(fieldOfView.position.x, fieldOfView.position.y, angle);
            }

            Quaternion z = Quaternion.Euler(0, 0, angle);
        }
    }
    
    public void SetFlipMode(int mode) {  
        if (mode == 0) flipMode = MyLibrary.FlipMode.MovementDirection;
        else if (mode == 1) flipMode = MyLibrary.FlipMode.WeaponDirection;
    }
}