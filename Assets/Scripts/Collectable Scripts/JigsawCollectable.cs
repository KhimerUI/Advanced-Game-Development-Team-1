using UnityEngine;

/// <summary>
/// Jigsaw Piece — the main objective collectable, equivalent to a Jiggy.
/// Triggers a celebration sequence and registers with the manager.
/// </summary>
public class JigsawCollectable : Collectable
{
    [Header("Jigsaw Settings")]
    public bool triggerCelebration = true;

    [Header("Celebration")]
    [Tooltip("Optional fanfare clip played on top of the normal collect sound.")]
    public AudioClip fanfareClip;
    public GameObject celebrationVFXPrefab;

    protected override void Start()
    {
        collectableName = "Jigsaw Piece";
        value = 1;
        // Jiggies spin faster and bob more dramatically
        spinSpeed = 120f;
        bobHeight = 0.45f;
        magnetRadius = 5f;
        base.Start();
    }

    protected override void OnCollected(Transform collector)
    {
        CollectableManager.Instance?.AddJigsaws(1);
        Debug.Log($"[Jigsaw] Collected! Total jigsaws: {CollectableManager.Instance?.JigsawCount}");

        if (triggerCelebration)
            PlayCelebration();
    }

    private void PlayCelebration()
    {
        if (fanfareClip != null)
            AudioSource.PlayClipAtPoint(fanfareClip, transform.position, 1f);

        if (celebrationVFXPrefab != null)
            Instantiate(celebrationVFXPrefab, transform.position + Vector3.up, Quaternion.identity);

        // You could hook into a UI manager here to show a "Jigsaw Get!" banner
        CollectableManager.Instance?.OnJigsawCelebration();
    }
}
