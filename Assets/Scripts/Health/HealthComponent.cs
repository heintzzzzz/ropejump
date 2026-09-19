using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IDamageable
{
    void TakeDamage(DamageInfo info);
} 

public class HealthComponent : MonoBehaviour, IDamageable
{ 
    [Header("Health")] 
    public int maxHealth = 100;

    [SerializeField] private int currentHealth;

    [Header("Immunity")] 
	[Tooltip("Types of damage, which this object totaly ignores")] 

    public DamageType[] immunities = { };

    [Header("Resistance (0 = no, 1 = full immunity")]
    public ResistanceEntry[] resistances = { };

    [Header("Временная неуязвимость (i-frames)")]
    [Tooltip("Секунды неуязвимости после получения урона. 0 = выкл.")]
    public float invincibilityDuration = 0f; 

    [Header("Events")] 
    public UnityEvent<DamageInfo>  onDamaged;   // передаёт полный DamageInfo
    public UnityEvent<int>         onHealthChanged; // текущий HP
    public UnityEvent              onDeath;
    public UnityEvent<int>         onHealed; 

     
    // --- State ----------------------------------------------------

    public int CurrentHealth => currentHealth;
    public bool IsAlive => currentHealth > 0;
    public bool IsInvincible { get; private set; }
    
    // --- LifeCycle ----------------------------------------------------
    private void Awake() => currentHealth = maxHealth;

    public void TakeDamage(DamageInfo info)
    {
        if (!IsAlive || IsInvincible) return;
        
        // Immunity
        if (IsImmuneTo(info.Type)) return;
        
        // Resistance
        int finalAmount = ApplyResistance(info.Amount, info.Type);
        if (finalAmount <= 0) return;

        currentHealth = Mathf.Max(0, currentHealth - finalAmount);
        
        // Knockback
        if (info.KnockbackForce > 0f)
        {
          //   var rb = GetComponent<Rigidbody2D>();
          //   if (rb != null) rb.AddForce(info.KnockbackDir * info.KnockbackForce, ForceMode2D.Impulse);
        }

        onDamaged?.Invoke(info); 
        onHealthChanged?.Invoke(currentHealth);

        if (currentHealth <= 0)
            onDeath?.Invoke();
        else if (invincibilityDuration > 0f)
            StartCoroutine(InvincibilityFrames()); 
    }
    
    // --- Cure ----------------------------------------------------

    public void Heal(int amount)
    {
        if (!IsAlive) return;
        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        onHealthChanged?.Invoke(currentHealth);
        onHealed?.Invoke(amount); 
    }
    
    // --- Untilities ----------------------------------------------------

    private bool IsImmuneTo(DamageType type)
    {
        foreach (var t in immunities) 
            if (t == type) return true;
        return false;
    }

    private int ApplyResistance(int amount, DamageType type)
    {
        foreach (var r in resistances)
        {
            if (r.type == type) return Mathf.RoundToInt(amount * (1f - Mathf.Clamp01(r.resistance)));
            return amount;
        }
		return amount;
    }

    private IEnumerator InvincibilityFrames()  
    {
        IsInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        IsInvincible = false;
    }

}





















