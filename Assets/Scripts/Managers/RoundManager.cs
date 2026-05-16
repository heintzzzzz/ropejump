using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RoundManager : MonoBehaviour
{
    [Header("Fighters")]
    public RubberBandController player1;
    public RubberBandController player2; 

    [Header("Health References")]
    // public CharacterHealth player1Health;  
    // public CharacterHealth player2Health;

    [Header("Round Settings")]
    public float roundDuration = 60f;   // seconds

    [Header("Events")]
    public UnityEvent onRoundStart;
    public UnityEvent<string> onRoundEnd;   // passes winner name

    private float timeRemaining;
    private bool  roundActive;

    // ── Lifecycle ──────────────────────────────────────────────

    private void Start()
    {
        // Hook death events
        // player1Health.onDeath.AddListener(() => EndRound("Player 2 wins!"));
        // player2Health.onDeath.AddListener(() => EndRound("Player 1 wins!"));
    }

    // ── Public API ─────────────────────────────────────────────

    public void StartRound()
    {
        if (roundActive) return;
        roundActive   = true;
        timeRemaining = roundDuration;

        player1.StartCycle();
        // player2.StartCycle();

        onRoundStart?.Invoke();
        StartCoroutine(RoundTimer());
    }

    public void EndRound(string winner)
    {
        if (!roundActive) return;
        roundActive = false;
        StopAllCoroutines();

        player1.StopCycle();
        // player2.StopCycle();

        Debug.Log($"[Round] Over – {winner}");
        onRoundEnd?.Invoke(winner);
    }

    // ── Timer ──────────────────────────────────────────────────

    private IEnumerator RoundTimer()
    {
        while (timeRemaining > 0f && roundActive)
        {
            timeRemaining -= Time.deltaTime;
            yield return null;
        }

        if (roundActive)
        {
            // Determine winner by remaining health
            /*string winner = player1Health.currentHealth >= player2Health.currentHealth
                ? "Player 1 wins (more HP)!"
                : "Player 2 wins (more HP)!";*/ 
           //  EndRound(winner);
        }
    }

    // ── Gizmos ─────────────────────────────────────────────────

    private void OnGUI()
    {
        if (!roundActive) return;
        GUI.Label(new Rect(10, 10, 200, 30), $"Time: {timeRemaining:F1}s");
    }
}