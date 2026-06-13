using UnityEngine;

public enum DamageType
{
    Physical,   // удар, снаряд
    Fire,       // поджог, лава
    Acid,       // кислота
    Blade,      // режущее
    Poison,     // яд (DoT)
    Fall,       // урон от падения
}
  
// ──────────────────────────────────────────────────────────────
//  2. Контейнер данных об уроне
//     Создаётся источником и передаётся в TakeDamage().
// ──────────────────────────────────────────────────────────────
 
public struct DamageInfo
{
    public int        Amount;       // величина урона
    public DamageType Type;         // тип для иммунитетов / VFX
    public GameObject Source;       // кто нанёс (для логгинга, score)
    public Vector2    KnockbackDir; // направление отброса (Vector2.zero = нет)
    public float      KnockbackForce;
 
    // Быстрый конструктор для простых случаев
    public DamageInfo(int amount, DamageType type = DamageType.Physical,
                      GameObject source = null)
    {
        Amount        = amount;
        Type          = type;
        Source        = source;
        KnockbackDir  = Vector2.zero;
        KnockbackForce = 0f;
    }
 
    // Конструктор с отбросом
    public DamageInfo(int amount, DamageType type, GameObject source,
                      Vector2 knockbackDir, float knockbackForce)
    {
        Amount         = amount;
        Type           = type;
        Source         = source;
        KnockbackDir   = knockbackDir.normalized;
        KnockbackForce = knockbackForce;
    }
}


/*
public enum DamageType
{
    Physical,
    Fire,
    Acid,
    Blade,
    Poison,
    Fall,
}

public struct DamageSystem
{
    public int Amount;
    public DamageType Type; // type for immunity
    public GameObject Source; // who deal damage
    public Vector2 KnockbackDir;
    private float KnockbackForce;

    public DamageInfo(int amount, DamageType type = DamageType.Physical, GameObject source = null)
    {
        Amount = amount;
        Type = type;
        Source = source;
        KnockBackDir = Vector2.zero;
        KnockbackForce = 0f;
    }

    public DamageInfo(int amount, DamageType type, GamgeObject source, Vector2 knockbackDir, float knockbackForce)
    {
        Amount = amount;
        Type = type;
        Source = source;
        KnockBackDir = knockbackDir.normalized;
        KnockbackForce = knockbackForce; 
    }
} 

public interface IDamagable
{
    void TakeDamage(DamageInfo info);
}
*/
