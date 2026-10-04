using System.Collections.Generic;
using UnityEngine;

// Снаряд: летит, наносит урон целям, исчезает на препятствиях.
// Префаб: Projectile + Rigidbody2D (Dynamic) + trigger Collider2D + SpriteRenderer.
// Все параметры приходят из AmmoDataItem через Launch().
[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Tooltip("Layers that stop the projectile without taking damage (walls, ground).")]
    [SerializeField] private LayerMask obstacleLayers;

    private Rigidbody2D _rb;
    private SpriteRenderer _spriteRenderer;

    private DamageInfo _damage;
    private LayerMask _targetLayers;
    private GameObject _owner;
    private ParticleSystem _hitEffect;
    private int _pierceLeft;
    private bool _alignToVelocity;

    // каждую цель бьём только один раз (важно для пробивающих снарядов)
    private readonly HashSet<IDamageable> _hitTargets = new HashSet<IDamageable>();

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Launch(AmmoDataItem ammo, DamageInfo damage, Vector2 direction,
                       LayerMask targetLayers, GameObject owner, ParticleSystem hitEffect)
    {
        _damage = damage;
        _targetLayers = targetLayers;
        _owner = owner;
        _hitEffect = hitEffect;
        _pierceLeft = ammo.Pierce;
        _alignToVelocity = ammo.Gravity > 0f;

        if (ammo.ProjectileSprite != null && _spriteRenderer != null)
            _spriteRenderer.sprite = ammo.ProjectileSprite;

        direction = direction.normalized;
        transform.right = direction;  // спрайт смотрит по направлению полёта

        _rb.gravityScale = ammo.Gravity;
        _rb.linearVelocity = direction * ammo.Speed;

        Destroy(gameObject, ammo.Lifetime);
    }

    private void Update()
    {
        // бутылка / граната летит по дуге — поворачиваем вдоль траектории
        if (_alignToVelocity && _rb.linearVelocity.sqrMagnitude > 0.01f)
            transform.right = _rb.linearVelocity;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_owner != null && other.transform.IsChildOf(_owner.transform)) return;

        int layerBit = 1 << other.gameObject.layer;

        if ((_targetLayers.value & layerBit) != 0 && other.TryGetComponent<IDamageable>(out var target))
        {
            if (!_hitTargets.Add(target)) return;

            DamageInfo info = _damage;
            info.KnockbackDir = _rb.linearVelocity.normalized;
            target.TakeDamage(info);

            MyLibrary.SpawnParticles(_hitEffect, other.ClosestPoint(transform.position));

            if (_pierceLeft <= 0) Destroy(gameObject);
            else _pierceLeft--;
            return;
        }

        // стены и пол: триггеры (зоны, пикапы) снаряд не останавливают
        if (!other.isTrigger && (obstacleLayers.value & layerBit) != 0)
        {
            MyLibrary.SpawnParticles(_hitEffect, other.ClosestPoint(transform.position));
            Destroy(gameObject);
        }
    }
}
