using System;
using UnityEngine;


/// My awesome cool big-ahh manager that tracks all collectables for the current level
/// Fires events that UI, audio, and level-complete systems can listen to

public class CollectableManager : MonoBehaviour
{
    // ── Singleton ────────────────────────────────────────────────────
    public static CollectableManager Instance { get; private set; }

    // ── State ────────────────────────────────────────────────────────
    public int NoteCount     { get; private set; }
    public int JigsawCount   { get; private set; }
    public int NotesRequired  = 100;   // Tweak per level
    public int JigsawsInLevel = 10;

    // ── Events (subscribe from UI, audio, achievements, etc.) ────────
    public event Action<int> OnNotesChanged;
    public event Action<int> OnJigsawsChanged;
    public event Action       OnJigsawCelebrationEvent;
    public event Action<int>  OnHealthPickedUp;
    public event Action       OnAllNotesCollected;
    public event Action       OnAllJigsawsCollected;

    // ── Unity lifecycle ──────────────────────────────────────────────
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // Optionally persist across scenes:
        // DontDestroyOnLoad(gameObject);
    }

    // ── Public API ───────────────────────────────────────────────────
    public void AddNotes(int amount)
    {
        NoteCount += amount;
        OnNotesChanged?.Invoke(NoteCount);
        Debug.Log($"[Manager] Notes: {NoteCount}/{NotesRequired}");

        if (NoteCount >= NotesRequired)
            OnAllNotesCollected?.Invoke();
    }

    public void AddJigsaws(int amount)
    {
        JigsawCount += amount;
        OnJigsawsChanged?.Invoke(JigsawCount);
        Debug.Log($"[Manager] Jigsaws: {JigsawCount}/{JigsawsInLevel}");

        if (JigsawCount >= JigsawsInLevel)
            OnAllJigsawsCollected?.Invoke();
    }

    public void OnJigsawCelebration()
    {
        OnJigsawCelebrationEvent?.Invoke();
    }

    public void OnHealthCollected(int amount)
    {
        OnHealthPickedUp?.Invoke(amount);
    }

    public void ResetLevel()
    {
        NoteCount   = 0;
        JigsawCount = 0;
    }
}
