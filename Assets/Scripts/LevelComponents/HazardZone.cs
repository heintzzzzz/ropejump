using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Универсальная опасная зона для 2D-игры.
/// Поддерживает: огонь, кислоту, режущие поверхности и любой кастомный тип.
/// 
/// Функционал:
///  - Периодический урон всем игрокам внутри зоны
///  - Вспышка цвета / пульсация зоны
///  - Спавн частиц (брызги, искры, дым)
///  - Knockback — отброс игрока от центра зоны
///  - Статус-эффекты: поджог, замедление, кровотечение (DoT вне зоны)
///  - Предупреждающая анимация перед активацией
///  - Возможность включать/выключать зону из кода
///  - UnityEvents для подключения внешней логики без кода
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class HazardZone : MonoBehaviour
{
    // ─────────────────────────────────────────────────────────────
    //  Тип опасности
    // ─────────────────────────────────────────────────────────────

    public enum HazardType { Fire, Acid, Blade, Custom }

    [Header("Тип опасности")]
    public HazardType hazardType = HazardType.Fire;

    // ─────────────────────────────────────────────────────────────
    //  Урон
    // ─────────────────────────────────────────────────────────────

    [Header("Урон")]
    [Tooltip("Урон за каждый тик.")]
    public int damagePerTick = 10;

    [Tooltip("Интервал между тиками урона (секунды).")]
    public float tickInterval = 0.5f;

    [Tooltip("Мгновенный урон при входе в зону (0 = отключено).")]
    public int entryDamage = 0;

    [Tooltip("Слои, которым наносится урон.")]
    public LayerMask targetLayers;

    // ─────────────────────────────────────────────────────────────
    //  Knockback (отброс)
    // ─────────────────────────────────────────────────────────────

    [Header("Knockback")]
    [Tooltip("Отбрасывать игрока при входе в зону?")]
    public bool applyKnockback = false;

    [Tooltip("Сила отброса от центра зоны.")]
    public float knockbackForce = 6f;

    // ─────────────────────────────────────────────────────────────
    //  Статус-эффект (DoT вне зоны)
    // ─────────────────────────────────────────────────────────────

    [Header("Статус-эффект после выхода")]
    [Tooltip("Применять DoT-эффект после выхода из зоны (поджог / кровотечение / кислота)?")]
    public bool applyStatusOnExit = false;

    [Tooltip("Урон в секунду от статус-эффекта.")]
    public float statusDamagePerSecond = 5f;

    [Tooltip("Длительность статус-эффекта (секунды).")]
    public float statusDuration = 3f;

    [Tooltip("Замедлять игрока внутри зоны?")]
    public bool applySlowInside = false;

    [Tooltip("Множитель скорости внутри зоны (0.5 = 50% скорости).")]
    [Range(0.1f, 1f)]
    public float slowMultiplier = 0.5f;

    // ─────────────────────────────────────────────────────────────
    //  Визуальные эффекты — цвет / пульсация
    // ─────────────────────────────────────────────────────────────

    [Header("Визуал — цвет")]
    [Tooltip("Цвет зоны в спокойном состоянии.")]
    public Color idleColor = new Color(1f, 0.3f, 0f, 0.4f);

    [Tooltip("Цвет вспышки при нанесении урона.")]
    public Color flashColor = new Color(1f, 1f, 0f, 0.9f);

    [Tooltip("Длительность вспышки (секунды).")]
    public float flashDuration = 0.1f;

    [Tooltip("Пульсировать цветом постоянно?")]
    public bool pulseWhenActive = true;

    [Tooltip("Скорость пульсации.")]
    public float pulseSpeed = 2f;

    [Tooltip("Амплитуда пульсации (0–1).")]
    [Range(0f, 1f)]
    public float pulseAmplitude = 0.3f;

    // ─────────────────────────────────────────────────────────────
    //  Визуальные эффекты — масштаб / дрожание
    // ─────────────────────────────────────────────────────────────

    [Header("Визуал — масштаб")]
    [Tooltip("Анимировать масштаб зоны при тике?")]
    public bool scaleOnTick = true;

    [Tooltip("Насколько увеличивается зона при тике (относительно исходного).")]
    public float scalePunch = 0.12f;

    [Tooltip("Постоянное дрожание зоны (для лезвий, огня).")]
    public bool continuousShake = false;

    [Tooltip("Интенсивность дрожания.")]
    public float shakeIntensity = 0.04f;

    // ─────────────────────────────────────────────────────────────
    //  Частицы
    // ─────────────────────────────────────────────────────────────

    [Header("Частицы")]
    [Tooltip("Система частиц для тика урона (брызги / искры / дым).")]
    public ParticleSystem tickParticles;

    [Tooltip("Система частиц при входе в зону (опционально).")]
    public ParticleSystem entryParticles;

    // ─────────────────────────────────────────────────────────────
    //  Предупреждение перед активацией
    // ─────────────────────────────────────────────────────────────

    [Header("Предупреждение")]
    [Tooltip("Мигать перед активацией?")]
    public bool showWarning = false;

    [Tooltip("Длительность предупреждающего мигания (секунды).")]
    public float warningDuration = 1.5f;

    // ─────────────────────────────────────────────────────────────
    //  Активация / деактивация зоны
    // ─────────────────────────────────────────────────────────────

    [Header("Активация")]
    [Tooltip("Зона активна с самого начала?")]
    public bool startActive = true;

    [Tooltip("Циклически включать/выключать зону?")]
    public bool cyclicActivation = false;

    [Tooltip("Время активного состояния (секунды).")]
    public float activeTime = 3f;

    [Tooltip("Время неактивного состояния (секунды).")]
    public float inactiveTime = 2f;

    // ─────────────────────────────────────────────────────────────
    //  События
    // ─────────────────────────────────────────────────────────────

    [Header("События")]
    public UnityEvent<GameObject> onPlayerEnter;
    public UnityEvent<GameObject> onPlayerExit;
    public UnityEvent<GameObject, int> onDamageDealt;   // target, amount

    // ─────────────────────────────────────────────────────────────
    //  Приватное состояние
    // ─────────────────────────────────────────────────────────────

    private SpriteRenderer sr;
    private Collider2D col;
    private Vector3 baseScale;
    private Color baseColor;

    private bool isActive = false; 
    private bool isFlashing = false;

    // Список игроков внутри зоны + их таймеры тиков
    private Dictionary<Health, float> targetsInside = new Dictionary<Health, float>();

    // ─────────────────────────────────────────────────────────────
    //  Unity Messages
    // ─────────────────────────────────────────────────────────────

    private void Awake()
    {
        sr  = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        baseScale = transform.localScale;

        ApplyHazardPreset();

        baseColor = idleColor;
        if (sr != null) sr.color = baseColor;
    }

    private void Start()
    {
        if (cyclicActivation)
            StartCoroutine(CyclicActivationLoop());
        else
            SetActive(startActive);
    }

    private void Update()
    {
        if (!isActive) return;

        TickDamageAll();

        if (pulseWhenActive && sr != null && !isFlashing)
            AnimatePulse();

        if (continuousShake) AnimateShake();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        if (!IsTargetLayer(other.gameObject)) return;

        var health = other.GetComponent<Health>();
        if (health == null || targetsInside.ContainsKey(health)) return;

        targetsInside[health] = 0f;

        // Мгновенный урон при входе
        if (entryDamage > 0)
            DealDamage(health, entryDamage);

        // Частицы входа
        if (entryParticles != null)
            entryParticles.Play();

        // Knockback
        if (applyKnockback)
            ApplyKnockback(other.attachedRigidbody);

        // Замедление
        if (applySlowInside)
            ApplySlow(other.gameObject, slowMultiplier);

        onPlayerEnter?.Invoke(other.gameObject);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!IsTargetLayer(other.gameObject)) return;

        var health = other.GetComponent<Health>();
        if (health == null || !targetsInside.ContainsKey(health)) return;

        targetsInside.Remove(health);

        // Снять замедление
        if (applySlowInside)
            RemoveSlow(other.gameObject);

        // Статус-эффект после выхода
        if (applyStatusOnExit)
            StartCoroutine(ApplyStatusEffect(health, statusDuration, statusDamagePerSecond));

        onPlayerExit?.Invoke(other.gameObject);
    }

    // ─────────────────────────────────────────────────────────────
    //  Public API
    // ─────────────────────────────────────────────────────────────

    public void SetActive(bool active)
    {
        isActive = active;
        col.enabled = active;

        if (sr != null)
            sr.color = active ? baseColor : new Color(baseColor.r, baseColor.g, baseColor.b, 0.1f);

        if (!active)
        {
            // Убрать замедление со всех кто внутри
            foreach (var kv in targetsInside)
            {
                var go = kv.Key.gameObject;
                if (applySlowInside) RemoveSlow(go);
            }
            targetsInside.Clear();
        }
    }

    public void Activate()   => SetActive(true);
    public void Deactivate() => SetActive(false);

    // ─────────────────────────────────────────────────────────────
    //  Тиковый урон
    // ─────────────────────────────────────────────────────────────

    private void TickDamageAll()
    {
        // Копируем ключи чтобы безопасно итерировать
        var keys = new List<Health>(targetsInside.Keys);

        foreach (var health in keys)
        {
            if (health == null) { targetsInside.Remove(health); continue; }

            targetsInside[health] += Time.deltaTime;

            if (targetsInside[health] >= tickInterval)
            {
                targetsInside[health] = 0f;
                DealDamage(health, damagePerTick);
                PlayTickEffects(health.transform.position);
            }
        }
    }

    private void DealDamage(Health health, int amount)
    {
        health.TakeDamage(amount);
        onDamageDealt?.Invoke(health.gameObject, amount);
    }

    // ─────────────────────────────────────────────────────────────
    //  Визуальные эффекты
    // ─────────────────────────────────────────────────────────────

    private void PlayTickEffects(Vector3 pos)
    {
        // Частицы
        if (tickParticles != null)
        {
            tickParticles.transform.position = pos;
            tickParticles.Play();
        }

        // Вспышка цвета
        if (sr != null && !isFlashing)
            StartCoroutine(Flash());

        // Удар по масштабу
        if (scaleOnTick)
            StartCoroutine(ScalePunch());
    }

    private IEnumerator Flash()
    {
        isFlashing = true;
        sr.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        sr.color = baseColor;
        isFlashing = false;
    }

    private IEnumerator ScalePunch()
    {
        float t = 0f;
        float half = 0.08f;
        Vector3 target = baseScale * (1f + scalePunch);

        while (t < half)
        {
            transform.localScale = Vector3.Lerp(baseScale, target, t / half);
            t += Time.deltaTime;
            yield return null;
        }
        t = 0f;
        while (t < half)
        {
            transform.localScale = Vector3.Lerp(target, baseScale, t / half);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = baseScale;
    }

    private void AnimatePulse()
    {
        float alpha = idleColor.a + Mathf.Sin(Time.time * pulseSpeed) * pulseAmplitude * idleColor.a;
        alpha = Mathf.Clamp01(alpha);
        sr.color = new Color(idleColor.r, idleColor.g, idleColor.b, alpha);
    }

    private void AnimateShake()
    {
        float ox = Mathf.PerlinNoise(Time.time * 30f, 0f) - 0.5f;
        float oy = Mathf.PerlinNoise(0f, Time.time * 30f) - 0.5f;
        transform.localPosition += new Vector3(ox, oy, 0f) * shakeIntensity;
    }

    // ─────────────────────────────────────────────────────────────
    //  Предупреждение перед активацией
    // ─────────────────────────────────────────────────────────────

    private IEnumerator WarningBlink(float duration)
    {
        float elapsed = 0f;
        bool vis = true;
        while (elapsed < duration)
        {
            if (sr != null) sr.color = vis
                ? new Color(flashColor.r, flashColor.g, flashColor.b, 0.6f)
                : new Color(baseColor.r, baseColor.g, baseColor.b, 0.1f);
            vis = !vis;
            elapsed += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // ─────────────────────────────────────────────────────────────
    //  Цикличная активация
    // ─────────────────────────────────────────────────────────────

    private IEnumerator CyclicActivationLoop()
    {
        while (true)
        {
            // Активная фаза
            SetActive(true);
            yield return new WaitForSeconds(activeTime);

            // Предупреждение перед деактивацией (опционально)
            // yield return StartCoroutine(WarningBlink(0.6f));

            // Неактивная фаза — предупреждающее мигание перед следующим включением
            SetActive(false);
            if (showWarning)
                yield return StartCoroutine(WarningBlink(warningDuration));
            else
                yield return new WaitForSeconds(inactiveTime);
        }
    }

    // ─────────────────────────────────────────────────────────────
    //  Статус-эффект (DoT вне зоны)
    // ─────────────────────────────────────────────────────────────

    private IEnumerator ApplyStatusEffect(Health health, float duration, float dps)
    {
        float elapsed = 0f;
        float acc = 0f;
        while (elapsed < duration)
        {
            if (health == null) yield break;
            elapsed += Time.deltaTime;
            acc     += Time.deltaTime;

            // Наносим урон раз в секунду
            if (acc >= 1f)
            {
                acc -= 1f;
                health.TakeDamage(Mathf.RoundToInt(dps));
            }
            yield return null;
        }
    }

    // ─────────────────────────────────────────────────────────────
    //  Knockback
    // ─────────────────────────────────────────────────────────────

    private void ApplyKnockback(Rigidbody2D targetRb)
    {
        if (targetRb == null) return;
        Vector2 dir = ((Vector2)targetRb.transform.position - (Vector2)transform.position).normalized;
        if (dir == Vector2.zero) dir = Vector2.up;
        targetRb.AddForce(dir * knockbackForce, ForceMode2D.Impulse);
    }

    // ─────────────────────────────────────────────────────────────
    //  Замедление
    //  Использует простой компонент SlowEffect чтобы не ломать
    //  RubberBandController.horizontalSpeed напрямую.
    // ─────────────────────────────────────────────────────────────

    private void ApplySlow(GameObject target, float multiplier)
    {
        var slow = target.GetComponent<SlowEffect>();
        if (slow == null) slow = target.AddComponent<SlowEffect>();
        slow.Apply(multiplier);
    }

    private void RemoveSlow(GameObject target)
    {
        var slow = target.GetComponent<SlowEffect>();
        if (slow != null) slow.Remove();
    }

    // ─────────────────────────────────────────────────────────────
    //  Пресеты по типу опасности
    // ─────────────────────────────────────────────────────────────

    private void ApplyHazardPreset()
    {
        switch (hazardType)
        {
            case HazardType.Fire:
                idleColor        = new Color(1f, 0.35f, 0f, 0.45f);
                flashColor       = new Color(1f, 0.9f, 0f, 0.95f);
                pulseWhenActive  = true;
                pulseSpeed       = 3f;
                // continuousShake  = true;
                continuousShake  = false;
                shakeIntensity   = 0.03f;
                applyStatusOnExit = true;         // поджог
                statusDuration   = 2f;
                statusDamagePerSecond = 5f;
                break;

            case HazardType.Acid:
                idleColor        = new Color(0.2f, 1f, 0.1f, 0.5f);
                flashColor       = new Color(0.8f, 1f, 0f, 1f);
                pulseWhenActive  = true;
                pulseSpeed       = 1.5f;
                applySlowInside  = true;          // кислота замедляет
                slowMultiplier   = 0.4f;
                applyStatusOnExit = true;          // разъедает после выхода
                statusDuration   = 4f;
                statusDamagePerSecond = 3f;
                break;

            case HazardType.Blade:
                idleColor        = new Color(0.7f, 0.7f, 0.9f, 0.5f);
                flashColor       = new Color(1f, 1f, 1f, 1f);
                pulseWhenActive  = false;
                // continuousShake  = true;
                continuousShake  = false; 
                shakeIntensity   = 0.05f;
                scaleOnTick      = true;
                scalePunch       = 0.2f;
                applyKnockback   = true;          // лезвие отбрасывает
                knockbackForce   = 8f;
                entryDamage      = 15;            // сразу больно при входе
                break;

            case HazardType.Custom:
                // Настраивается вручную в инспекторе
                break;
        }
    }

    // ─────────────────────────────────────────────────────────────
    //  Утилита
    // ─────────────────────────────────────────────────────────────

    private bool IsTargetLayer(GameObject go) =>
        (targetLayers.value & (1 << go.layer)) != 0; 
}


// ══════════════════════════════════════════════════════════════
//  SlowEffect.cs — вспомогательный компонент замедления
//  Добавляется/удаляется HazardZone динамически.
// ══════════════════════════════════════════════════════════════

/// <summary>
/// Хранит текущий множитель замедления.
/// RubberBandController читает SlowEffect.Multiplier при расчёте скорости.
/// </summary>
public class SlowEffect : MonoBehaviour
{
    public float Multiplier { get; private set; } = 1f;
    private bool applied = false;

    public void Apply(float multiplier) 
    {
        Multiplier = multiplier;
        applied = true;
    }

    public void Remove()
    {
        Multiplier = 1f;
        applied = false;
    }
}