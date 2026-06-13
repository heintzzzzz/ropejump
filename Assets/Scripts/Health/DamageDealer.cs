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

    [Tooltip("Deael damage only one tile on enter trigger (do not repeat")]
    public bool hitOnce = true;

    [Tooltip("Target layers.")] public LayerMask targetLayers;

    private bool alreadyHit = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hitOnce && alreadyHit) return;
       if ((targetLayers.value & (1 << other.gameObject.layer)) == 0) return; 

        var target = other.GetComponent<IDamageable>();
        if (target == null) return;

        Vector2 knockDir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized; 
        var info = new DamageInfo(damage, damageType, gameObject, knockDir, knockbackForce);
        target.TakeDamage(info);

        alreadyHit = true;

        if (destroyOnHit)
            Destroy(gameObject);

    }

    public void ResetHit() => alreadyHit = false;
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