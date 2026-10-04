using UnityEngine;

// Вся настройка оружия в одном ассете: Weapon читает отсюда, а не из своих полей
[CreateAssetMenu(menuName = "Items/Weapon/WeaponDataItem", fileName = "Weapon Data Item")]
public class WeaponDataItem : ScriptableObject
{
    [Header("Identity")]
    public string WeaponName;
    public string WeaponId;
    [Tooltip("Icon for UI (weapon block, inventory).")]
    public Sprite WeaponSprite;
    [Tooltip("Sprite in the character's hand. Empty = keep the prefab sprite.")]
    public Sprite WorldSprite;
    [Tooltip("Prefab spawned on equip. Must have MeleeWeapon or ShootingWeapon, matching Kind.")]
    public Weapon WeaponToEquip;
    public WeaponKind Kind = WeaponKind.Melee;

    [Header("Damage")]
    public int WeaponDamage = 10;
    public DamageType DamageType = DamageType.Physical;
    public float KnockbackForce = 0f;

    [Header("Timing")]
    [Tooltip("Seconds between attacks / shots.")]
    public float TimeBtwShots = 0.5f;
    [Tooltip("Melee: how long the hitbox is active during a swing.")]
    public float AttackDuration = 0.3f;

    [Header("Ammo")]
    public bool UseMagazine;
    public int MagazineSize;
    [Tooltip("Max reserve ammo.")]
    public int AmountSizeLimit;
    public ReloadType ReloadType = ReloadType.Manual;
    [Tooltip("Shooting: what this weapon fires.")]
    public AmmoDataItem Ammo;

    [Header("Shooting")]
    [Min(1)] public int ProjectilesPerShot = 1;
    [Tooltip("Spread in degrees. Several projectiles fan out evenly; a single one gets a random offset.")]
    public float SpreadAngle = 0f;

    [Header("Effects")]
    [Tooltip("Muzzle flash / swing effect, spawned on each attack.")]
    public ParticleSystem AttackEffect;
    [Tooltip("Spawned where the weapon hits a target.")]
    public ParticleSystem HitEffect;
    public AudioClip AttackSound;

    [Header("Character")]
    public MyLibrary.FlipMode flipMode = MyLibrary.FlipMode.WeaponDirection;
    public bool isItem;
    public int weaponMode;
}
