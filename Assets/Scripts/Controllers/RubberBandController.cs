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
    public float travelSpeed = 8f;
 
    [Tooltip("Horizontal movement speed while falling or ascending.")]
    public float horizontalSpeed = 5f;
 
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
 
    [Header("Bottom-Delay Penalty")]
    [Tooltip("Movement speed multiplier while in the bottom delay phase.")]
    [Range(0f, 1f)]
    public float bottomSlowMultiplier = 0.5f;
 
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
 
    /// <summary>Current horizontal speed modifier exposed for UI / other systems.</summary>
    public float CurrentHorizontalMultiplier { get; private set; } = 1f;
 
    // ─────────────────────────────────────────────
    //  Private State
    // ─────────────────────────────────────────────
 
    private Rigidbody2D rb;
    private float stretchLength;   // distance A → B
    private float overshootDist;   // stretchLength * overshootPercent
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
 
    private void Update()
    {
        if (!cycleActive) return; 
		//HandleHorizontalInput();
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
        transform.position = new Vector3(transform.position.x, yTop, transform.position.z);
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
        CurrentHorizontalMultiplier = 1f;
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
            //    Slow movement, no attack allowed.
            yield return StartCoroutine(DelayPhase(bottomDelay, Phase.BottomDelay, bottomSlowMultiplier));
 
            // 4. RETURN from extreme low back to B
            yield return StartCoroutine(TravelVertical(yBottom, Phase.BottomOvershoot, overshootSpeedMultiplier));
 
            // 5. ASCENT  (B → A)
            yield return StartCoroutine(TravelVertical(yTop, Phase.Ascending));
 
            // 6. TOP OVERSHOOT  (A → A + 10%)
            yield return StartCoroutine(TravelVertical(yTopOver, Phase.TopOvershoot, overshootSpeedMultiplier));
 
            // 7. TOP DELAY at the extreme high point (A + 10%)
            //    Full speed, can attack.
            yield return StartCoroutine(DelayPhase(topDelay, Phase.TopDelay, 1f));
 
            // 8. RETURN from extreme high back to A
            yield return StartCoroutine(TravelVertical(yTop, Phase.TopOvershoot, overshootSpeedMultiplier));
        }
    }
 
    // ─────────────────────────────────────────────
    //  Helpers
    // ─────────────────────────────────────────────
 
    /// <summary>
    /// Smoothly moves the character to targetY along the vertical axis.
    /// </summary>
    private IEnumerator TravelVertical(float targetY, Phase phase, float speedMult = 1f)
    {
        CurrentPhase = phase;
        CurrentHorizontalMultiplier = 1f;
 
        float speed = travelSpeed * speedMult;
 
        while (cycleActive)
        {
            float currentY = transform.position.y;
            float step = speed * Time.deltaTime;
            float newY = Mathf.MoveTowards(currentY, targetY, step);
 
            // Keep X unchanged (horizontal input handled in Update via rb)
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
 
            if (Mathf.Approximately(newY, targetY))
                break;
 
            yield return null;
        }
    }
 
    /// <summary>
    /// Holds position for <duration> seconds, applying a movement multiplier.
    /// </summary>
    private IEnumerator DelayPhase(float duration, Phase phase, float hMultiplier)
    {
        CurrentPhase = phase;
        CurrentHorizontalMultiplier = hMultiplier;
 
        float elapsed = 0f;
        while (elapsed < duration && cycleActive)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
 
    /// <summary>
    /// Reads horizontal axis input and moves the character accordingly.
    /// Respects the current horizontal multiplier.
    /// </summary>
    private void HandleHorizontalInput()
    {
        //float h = Input.GetAxisRaw("Horizontal");
        //float speed = horizontalSpeed * CurrentHorizontalMultiplier;
        //Vector2 vel = rb.linearVelocity;
        //vel.x = h * speed;
        //rb.linearVelocity = vel; 
    }

	public void HandleBlock() {
		// Debug.Log("STOPPPPPPPPPPPPPPPPPPPP");
	}
}
