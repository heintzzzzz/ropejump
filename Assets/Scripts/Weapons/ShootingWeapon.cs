using UnityEngine;

// Стрелковое оружие: чем стрелять, сколько снарядов и с каким разбросом — из WeaponDataItem и его AmmoDataItem
[RequireComponent(typeof(WeaponAmmo))]
public class ShootingWeapon : Weapon
{
    [Tooltip("Where projectiles and muzzle effects spawn. Empty = weapon position.")]
    [SerializeField] private Transform firePoint;

    protected override Transform EffectPoint => firePoint != null ? firePoint : transform;

    protected override void Attack()
    {
        if (Data == null || Data.Ammo == null || Data.Ammo.ProjectilePrefab == null)
        {
            Debug.LogWarning($"{name}: ShootingWeapon needs a WeaponDataItem with Ammo and Ammo.ProjectilePrefab.", this);
            return;
        }

        AmmoDataItem ammo = Data.Ammo;
        DamageType type = ammo.OverrideDamageType ? ammo.DamageType : Data.DamageType;
        var damage = new DamageInfo(Data.WeaponDamage + ammo.BonusDamage, type, DamageSource,
                                    Vector2.zero, Data.KnockbackForce);
        ParticleSystem hitEffect = ammo.HitEffect != null ? ammo.HitEffect : Data.HitEffect;

        Vector2 aim = GetAimDirection();
        int count = Mathf.Max(1, Data.ProjectilesPerShot);
        float halfSpread = Data.SpreadAngle * 0.5f;

        for (int i = 0; i < count; i++)
        {
            // один снаряд — случайное отклонение, несколько — веером (дробовик)
            float offset = count == 1
                ? Random.Range(-halfSpread, halfSpread)
                : Mathf.Lerp(-halfSpread, halfSpread, i / (float)(count - 1));

            Vector2 direction = Quaternion.Euler(0f, 0f, offset) * aim;

            Projectile projectile = Instantiate(ammo.ProjectilePrefab, EffectPoint.position, Quaternion.identity);
            projectile.Launch(ammo, damage, direction, TargetLayers, DamageSource, hitEffect);
        }
    }

    private Vector2 GetAimDirection()
    {
        // игрок стреляет в сторону мыши
        if (isPlayerWeapon && WeaponAim != null)
        {
            float angle = WeaponAim.CurrentAimAngleAbsolute * Mathf.Deg2Rad;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        }

        // враги — туда, куда смотрят
        bool facingRight = OwnerFlip == null || OwnerFlip.FacingRight;
        return facingRight ? Vector2.right : Vector2.left;
    }
}
