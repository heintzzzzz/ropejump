using UnityEngine;

// Тип патронов: чем стреляет оружие (пуля, бутылка, граната...)
[CreateAssetMenu(menuName = "Items/Weapon/AmmoDataItem", fileName = "Ammo Data Item")]
public class AmmoDataItem : ScriptableObject
{
    [Header("Identity")]
    public string AmmoId;
    public string AmmoName;
    [Tooltip("Icon for UI.")]
    public Sprite Icon;

    [Header("Projectile")]
    [Tooltip("Prefab with Projectile + Rigidbody2D (Dynamic) + trigger Collider2D.")]
    public Projectile ProjectilePrefab;
    [Tooltip("Projectile sprite. Empty = keep the prefab sprite.")]
    public Sprite ProjectileSprite;
    public float Speed = 15f;
    [Tooltip("Seconds before the projectile disappears if it hits nothing.")]
    public float Lifetime = 3f;
    [Tooltip("0 = flies straight (bullet), >0 = flies in an arc (bottle, grenade).")]
    public float Gravity = 0f;
    [Tooltip("How many extra targets it passes through. 0 = stops at the first target.")]
    [Min(0)] public int Pierce = 0;

    [Header("Damage")]
    [Tooltip("Added to the weapon damage.")]
    public int BonusDamage = 0;
    [Tooltip("Use the ammo damage type instead of the weapon one (e.g. fire bullets).")]
    public bool OverrideDamageType = false;
    public DamageType DamageType = DamageType.Physical;

    [Header("Effects")]
    [Tooltip("Spawned where the projectile hits. Empty = use the weapon HitEffect.")]
    public ParticleSystem HitEffect;
}
