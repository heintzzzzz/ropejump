using UnityEngine;

// Настройка опасного препятствия (огонь, кислота, шипы, пила...).
// Назначь ассет в HazardZone.data — значения из инспектора зоны будут заменены этими.
[CreateAssetMenu(menuName = "Items/Hazard/HazardDataItem", fileName = "Hazard Data Item")]
public class HazardDataItem : ScriptableObject
{
    [Header("Identity")]
    public string HazardName;

    [Header("Damage")]
    public DamageType DamageType = DamageType.Fire;
    [Tooltip("Instant damage when a target enters (0 = off).")]
    public int EntryDamage = 0;
    [Tooltip("Damage every tick while the target stays inside.")]
    public int DamagePerTick = 10;
    [Tooltip("Seconds between ticks.")]
    public float TickInterval = 0.5f;

    [Header("Knockback")]
    public bool ApplyKnockback = false;
    public float KnockbackForce = 6f;

    [Header("Status effect after exit (burning, bleeding, acid)")]
    public bool ApplyStatusOnExit = false;
    public float StatusDamagePerSecond = 5f;
    public float StatusDuration = 3f;

    [Header("Slow inside")]
    public bool ApplySlowInside = false;
    [Range(0.1f, 1f)] public float SlowMultiplier = 0.5f;

    [Header("Visuals")]
    [Tooltip("Hazard sprite. Empty = keep the sprite on the object.")]
    public Sprite Sprite;
    [Tooltip("Sprite tint. White = original sprite colors.")]
    public Color IdleColor = Color.white;
    public Color FlashColor = new Color(1f, 0.4f, 0.4f, 1f);
    public float FlashDuration = 0.1f;
    public bool PulseWhenActive = false;
    public float PulseSpeed = 2f;
    [Range(0f, 1f)] public float PulseAmplitude = 0.3f;
    public bool ScaleOnTick = false;
    public float ScalePunch = 0.12f;
    public bool ContinuousShake = false;
    public float ShakeIntensity = 0.04f;

    [Header("Particles")]
    [Tooltip("Looping effect attached to the hazard while it exists (flames, bubbles, sparks).")]
    public ParticleSystem AmbientParticles;
    [Tooltip("Spawned on the target on every damage tick.")]
    public ParticleSystem TickParticles;
    [Tooltip("Spawned on the target when it enters.")]
    public ParticleSystem EntryParticles;

    [Header("Sound")]
    public AudioClip DamageSound;
}
