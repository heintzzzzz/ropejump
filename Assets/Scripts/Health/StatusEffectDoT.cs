using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectDoT : MonoBehaviour
{
    public DamageType damageType = DamageType.Poison;
    public float dps = 5f; // damage per second
    public float duration = 3f;
    public GameObject source;

    private HealthComponent health;
    private float elapsed;
    private float accumulator;

    private void Awake() => health = GetComponent<HealthComponent>();

    private void Update()
    { 
        if (health == null || !health.IsAlive)
        {
            Destroy(this);
            return;
        }

        elapsed += Time.deltaTime;
        accumulator += Time.deltaTime;

        if (accumulator >= 1f)
        {
            accumulator -= 1f;
            health.TakeDamage(new DamageInfo(Mathf.RoundToInt(dps), damageType, source));
        }

        if (elapsed >= duration) Destroy(this);
    }

    public void Refresh(float newDuration)
    {
        elapsed = 0f; 
        duration = newDuration;
    }
}