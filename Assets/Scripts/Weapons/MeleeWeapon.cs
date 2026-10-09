using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Ближний бой: на префабе нужен trigger BoxCollider2D + DamageDealer (хитбокс)
[RequireComponent(typeof(DamageDealer))]
public class MeleeWeapon : Weapon
{
    [Tooltip("How long the hitbox is active. Taken from WeaponDataItem.AttackDuration if set.")]
    [SerializeField] private float attackDelay = 0.5f;
    private BoxCollider2D _boxCollider2D;
    private DamageDealer _damageDealer;
    private bool _isAttacking = false;
    private readonly int useMeeleWeapon = Animator.StringToHash("UseMeeleWeapon");

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _damageDealer = GetComponent<DamageDealer>();

        // хитбокс выключен, пока не атакуем
        SetHitboxActive(false);
    }

    protected override void ApplyData(WeaponDataItem data)
    {
        base.ApplyData(data);
        if (data.AttackDuration > 0f) attackDelay = data.AttackDuration;
    }

    private void SetHitboxActive(bool active)
    {
        if (_boxCollider2D != null) _boxCollider2D.enabled = active;
    }

    // урон, тип, цели и эффект попадания берутся из данных оружия перед каждым взмахом
    private void ConfigureHitbox()
    {
        if (_damageDealer == null) return;

        if (Data != null)
        {
            _damageDealer.Configure(Data.WeaponDamage, Data.DamageType, Data.KnockbackForce,
                                    TargetLayers, Data.HitEffect, DamageSource);
        }

        // новый взмах — можно снова нанести урон
        _damageDealer.ResetHit();
    }

    protected override void RequestShot()
    {
        // пока идёт взмах, не тратим кулдаун и патроны
        if (_isAttacking) return;
        base.RequestShot();
    }

    protected override void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;

        yield return new WaitForEndOfFrame();
        // Даем аниматору время на инициализацию
        yield return null; // ждем следующего кадра

        Debug.Log(useMeeleWeapon); 
        
        if (animator != null) animator.SetTrigger(useMeeleWeapon);
        if (WeaponOwner != null && WeaponOwner.CharacterAnimator != null)
            WeaponOwner.CharacterAnimator.SetTrigger(useMeeleWeapon);

        ConfigureHitbox();
        SetHitboxActive(true);

        yield return new WaitForSeconds(attackDelay);

        SetHitboxActive(false);
        _isAttacking = false;
    }
}
