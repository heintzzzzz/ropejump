using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    [SerializeField] private float attackDelay = 0.5f;
    private BoxCollider2D _boxCollider2D;
    private bool _isAttacking = false;
    private readonly int useMeeleWeapon = Animator.StringToHash("UseMeeleWeapon");
    
    private void Start()
    { 
        _boxCollider2D = GetComponent<BoxCollider2D>(); 
    }

    protected override void Update()
    {
        base.Update();
        base.RotateWeapon(); 
    }

    public override void UseWeapon()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        if (_isAttacking == true) yield break;

        _boxCollider2D.enabled = false; 
        _isAttacking = true;

        float faceingRight = WeaponOwner.GetComponent<CharFlip>().FacingRight ? 1 : -1;

        yield return new WaitForEndOfFrame();
        // Даем аниматору время на инициализацию
        yield return null; // ждем следующего кадра
        
        animator.SetTrigger("UseMeeleWeapon"); 
        WeaponOwner.CharacterAnimator.SetTrigger("UseMeeleWeapon");  

        yield return new WaitForSeconds(attackDelay);
        
        _boxCollider2D.enabled = true;
        _isAttacking = false;
    }
    
    /*private IEnumerator Attack2()
    {
        if (_isAttacking == true) yield break;

        _boxCollider2D.enabled = false;
        _isAttacking = true;
        
        // animator.SetTrigger("UseMeeleWeapon");  
        float faceingRight = WeaponOwner.GetComponent<CharFlip>().FacingRight ? 1 : -1;

        Debug.Log("Attack_____" + animator);  
        
            // animator.SetFloat("FaceRight", faceingRight);
        // animator.SetTrigger(useMeeleWeapon);  
        animator.SetTrigger("test");    

        yield return new WaitForSeconds(attackDelay);

        _boxCollider2D.enabled = true;
        _isAttacking = false;
    }*/
}