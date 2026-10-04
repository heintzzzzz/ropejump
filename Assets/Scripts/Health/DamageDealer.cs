using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ResistanceEntry
{
    public DamageType type;
    [Range(0f, 1f)] 
    [Tooltip("0 = full damage force, 1 = full immunity")]
    public float resistance;
}
 
// ──────────────────────────────────────────────────────────────
//  5. DamageDealer
//     Вспомогательный компонент для нанесения урона через
//     коллайдер (ближний бой, снаряд, ловушка).
//     Добавь на оружие или снаряд — без лишнего кода.
// ──────────────────────────────────────────────────────────────

public class DamageDealer : MonoBehaviour
{
    [Header("Damage parameters")] 
    public int damage = 10;

    public DamageType damageType = DamageType.Physical;
    public float knockbackForce = 0f;

    [Header("Behaviout")] [Tooltip("Destroy object after the 1st collisioon (projectile).")]
    public bool destroyOnHit = false;

    [Tooltip("Hit each target only once until ResetHit() or re-enable (one swing / one projectile life).")]
    public bool hitOnce = true;

    [Tooltip("Target layers.")] public LayerMask targetLayers;

    [Header("Effects")]
    [Tooltip("Spawned at the hit point.")]
    public ParticleSystem hitEffect;

    // владелец (персонаж с оружием) — его не бьём
    private GameObject owner;

    // цели, уже получившие урон в текущей атаке
    private readonly HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

    // объект из пула или снова включённое оружие начинает с чистого списка
    private void OnEnable() => ResetHit();

    // Настройка из данных оружия. Пустые targetLayers / hitEffect не перезаписывают значения из инспектора.
    public void Configure(int newDamage, DamageType newType, float newKnockback,
                          LayerMask newTargetLayers, ParticleSystem newHitEffect, GameObject newOwner)
    {
        damage = newDamage;
        damageType = newType;
        knockbackForce = newKnockback;
        if (newTargetLayers.value != 0) targetLayers = newTargetLayers;
        if (newHitEffect != null) hitEffect = newHitEffect;
        owner = newOwner;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayers.value & (1 << other.gameObject.layer)) == 0) return;
        if (owner != null && other.transform.IsChildOf(owner.transform)) return;

        if (!other.TryGetComponent<IDamageable>(out var target)) return;
        if (hitOnce && hitTargets.Contains(target)) return;

        Vector2 knockDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
        var info = new DamageInfo(damage, damageType, owner != null ? owner : gameObject, knockDir, knockbackForce);
        target.TakeDamage(info);

        hitTargets.Add(target);
        MyLibrary.SpawnParticles(hitEffect, other.ClosestPoint(transform.position));

        if (destroyOnHit)
            Destroy(gameObject);
    }

    public void ResetHit() => hitTargets.Clear();
} 



/*public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            Vector2 direction =
                (other.transform.position - transform.position).normalized;

            DamageData damageData = new DamageData(
                damage,
                transform.position,
                direction,
                gameObject);

            damageable.TakeDamage(damageData);
        }
    }
}*/