using System.Collections;
using UnityEngine;

/// <summary>
/// Rubber Band / Bungee Cord Controller
/// Handles the full fall → inertia → delay → ascent → inertia → delay cycle
/// for a 2D vertical tunnel fighter game.
/// </summary>
public class RubberBandController : MonoBehaviour
{
    // ─────────────────────────────────────────────
    //  Inspector Settings
    // ─────────────────────────────────────────────
 
    [Header("Anchor Points")]
    [Tooltip("Starting position (top of the bungee).")]
    public Transform pointA;
 
    [Tooltip("End position (bottom of the bungee).")]
    public Transform pointB;
 
    [Header("Fall / Ascent")]
    [Tooltip("Base speed of the fall / ascent along the Y axis (units per second).")]
    public float travelSpeed = 13f;

    [Header("Inertia Overshoot")]
    [Tooltip("Overshoot percentage at both bounce points (0.10 = 10 %).")]
    [Range(0f, 0.5f)]
    public float overshootPercent = 0.10f;
 
    [Tooltip("Speed multiplier during the overshoot phase.")]
    public float overshootSpeedMultiplier = 0.6f;
 
    [Header("Delay Phases")]
    [Tooltip("Seconds the player lingers at the bottom point.")]
    public float bottomDelay = 5f;
 
    [Tooltip("Seconds the player lingers at the top point.")]
    public float topDelay = 5f;

    [Header("Boundary Penalty")]
    [Tooltip("Horizontal speed multiplier while hovering at a top/bottom extreme point (0 = fully blocked, 1 = no penalty).")]
    [Range(0f, 1f)]
    public float boundaryHorizontalMultiplier = 1f / 3f;

    // ─────────────────────────────────────────────
    //  Public State (read from other scripts)
    // ─────────────────────────────────────────────
 
    public enum Phase
    {
        Idle,
        Falling,
        BottomOvershoot,
        BottomDelay,
        Ascending,
        TopOvershoot,
        TopDelay
    }
 
    public Phase CurrentPhase { get; private set; } = Phase.Idle;
 
    /// <summary>True when the player is allowed to attack.</summary>
    public bool CanAttack => CurrentPhase != Phase.BottomDelay && CurrentPhase != Phase.Idle;

    /// <summary>
    /// Horizontal speed multiplier for the current phase: full speed while actively
    /// falling/ascending (including overshoot) or before the cycle starts; reduced to
    /// <see cref="boundaryHorizontalMultiplier"/> while hovering at a top/bottom extreme
    /// point. The Character Controller reads this to scale horizontal movement.
    /// </summary>
    public float HorizontalSpeedMultiplier =>
        (CurrentPhase == Phase.BottomDelay || CurrentPhase == Phase.TopDelay)
            ? boundaryHorizontalMultiplier
            : 1f;

    /// <summary>
    /// The Y position the cycle currently wants the character at. CharController reads
    /// this every FixedUpdate and combines it with horizontal input into a single
    /// MovePosition call, so vertical travel and horizontal input never fight over the
    /// Rigidbody2D (MovePosition calls don't stack - the last one before a physics step
    /// wins - so only one script may call it).
    /// </summary>
    public float TargetY { get; private set; }

    // ─────────────────────────────────────────────
    //  Private State
    // ─────────────────────────────────────────────
 
    private Rigidbody2D rb;
    private float stretchLength;   // distance A → B
    private float overshootDist;   // stretchLength * overshooftPercent
    private float yBottom;         // pointB.y
    private float yTop;            // pointA.y
    private float yBottomOver;     // yBottom  - overshootDist
    private float yTopOver;        // yTop     + overshootDist
 
    private bool cycleActive = false;
 
    // ─────────────────────────────────────────────
    //  Unity Messages
    // ─────────────────────────────────────────────
 
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f; // We drive movement manually
        TargetY = rb.position.y;
    }

    private void Start()
    {
        // Cache geometry once
        yTop    = pointA.position.y;
        yBottom = pointB.position.y;
        stretchLength = Mathf.Abs(yTop - yBottom);
        overshootDist = stretchLength * overshootPercent;
        yBottomOver   = yBottom - overshootDist;
        yTopOver      = yTop    + overshootDist;
    }
 
    // ─────────────────────────────────────────────
    //  Public API
    // ─────────────────────────────────────────────
 
    /// <summary>
    /// Call this to start the rubber-band cycle (e.g. round start).
    /// </summary>
    public void StartCycle()
    {
        if (cycleActive) return;
        rb.position = new Vector2(rb.position.x, yTop);
        TargetY = yTop;
        cycleActive = true;
        StartCoroutine(RunCycle());
    }
 
    /// <summary>
    /// Stops the cycle immediately (e.g. game over / timer expired).
    /// </summary>
    public void StopCycle()
    {
        cycleActive = false;
        StopAllCoroutines();
        CurrentPhase = Phase.Idle;
        rb.linearVelocity = Vector2.zero;
    }
 
    // ─────────────────────────────────────────────
    //  Core Coroutine
    // ─────────────────────────────────────────────
 
    private IEnumerator RunCycle()
    {
        while (cycleActive)
        {
            // 1. FALL  (A → B)
            yield return StartCoroutine(TravelVertical(yBottom, Phase.Falling));
 
            // 2. BOTTOM OVERSHOOT  (B → B - 10%)
            yield return StartCoroutine(TravelVertical(yBottomOver, Phase.BottomOvershoot, overshootSpeedMultiplier));
 
            // 3. BOTTOM DELAY at the extreme low point (B - 10%)
            //    Hovering: horizontal speed is reduced (boundaryHorizontalMultiplier), attacking is disabled.
            yield return StartCoroutine(DelayPhase(bottomDelay, Phase.BottomDelay));
 
            // 4. RETURN from extreme low back to B
            yield return StartCoroutine(TravelVertical(yBottom, Phase.BottomOvershoot, overshootSpeedMultiplier));
 
            // 5. ASCENT  (B → A)
            yield return StartCoroutine(TravelVertical(yTop, Phase.Ascending));
 
            // 6. TOP OVERSHOOT  (A → A + 10%)
            yield return StartCoroutine(TravelVertical(yTopOver, Phase.TopOvershoot, overshootSpeedMultiplier));
 
            // 7. TOP DELAY at the extreme high point (A + 10%)
            //    Hovering: horizontal speed is reduced (boundaryHorizontalMultiplier), attacking is allowed.
            yield return StartCoroutine(DelayPhase(topDelay, Phase.TopDelay));
 
            // 8. RETURN from extreme high back to A
            yield return StartCoroutine(TravelVertical(yTop, Phase.TopOvershoot, overshootSpeedMultiplier));
        }
    }
 
    // ─────────────────────────────────────────────
    //  Helpers
    // ───────────────────────────────────────────── ///////
 
    /// <summary>
    /// Smoothly moves the character to targetY along the vertical axis.
    /// </summary>
    private IEnumerator TravelVertical(float targetY, Phase phase, float speedMult = 1f)
    {
        CurrentPhase = phase;

        float speed = travelSpeed * speedMult;
 
        while (cycleActive)
        {
            float step = speed * Time.deltaTime;
            TargetY = Mathf.MoveTowards(TargetY, targetY, step);

            if (Mathf.Approximately(TargetY, targetY))
                break;

            yield return null;
        }
    }
 
    /// <summary>
    /// Holds position for <duration> seconds while hovering at an extreme point.
    /// </summary>
    private IEnumerator DelayPhase(float duration, Phase phase)
    {
        CurrentPhase = phase;

        float elapsed = 0f;
        while (elapsed < duration && cycleActive)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
