using UnityEngine;

/// <summary>
/// Equivalent to the musical notes in Banjo-Kazooie (think coins in sm64)
/// registers itself with CollectableManager on pickup.
/// </summary>

public class NoteCollectable : Collectable
{
    [Header("Note Settings")]
    [Tooltip("Each note is worth this many points toward the level note total.")]
    public int noteWorth = 1;

    protected override void Start()
    {
        collectableName = "Musical Note";
        value = noteWorth;
        base.Start();
    }

    protected override void OnCollected(Transform collector)
    {
        CollectableManager.Instance?.AddNotes(noteWorth);
        Debug.Log($"[Note] Collected! Total notes: {CollectableManager.Instance?.NoteCount}");
    }
}
