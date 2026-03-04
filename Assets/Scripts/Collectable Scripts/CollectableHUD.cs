using UnityEngine;
using TMPro;          
using UnityEngine.UI;


/// Simple HUD that listens to CollectableManager events and updates UI counters.
/// Attach to a Canvas GameObject. Wire up the TMP / Image fields in the Inspector.

public class CollectableHUD : MonoBehaviour
{
    [Header("Notes UI")]
    public TextMeshProUGUI noteText;
    public Image noteIcon;

    [Header("Jigsaws UI")]
    public TextMeshProUGUI jigsawText;
    public Image jigsawIcon;

    [Header("Jigsaw Celebration")]
    public GameObject celebrationBanner;   // "Jigsaw Piece Get!" panel
    public float bannerDisplayTime = 3f;

    private float _bannerTimer;

    // ── Unity lifecycle ──────────────────────────────────────────────
    private void Start()
    {
        if (celebrationBanner != null)
            celebrationBanner.SetActive(false);

        // Subscribe once the manager is ready
        if (CollectableManager.Instance != null)
            Subscribe();
        else
            // Small delay if manager initialises on same frame
            Invoke(nameof(Subscribe), 0.1f);
    }

    private void Update()
    {
        if (celebrationBanner != null && celebrationBanner.activeSelf)
        {
            _bannerTimer -= Time.deltaTime;
            if (_bannerTimer <= 0f)
                celebrationBanner.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (CollectableManager.Instance == null) return;
        CollectableManager.Instance.OnNotesChanged          -= UpdateNotes;
        CollectableManager.Instance.OnJigsawsChanged        -= UpdateJigsaws;
        CollectableManager.Instance.OnJigsawCelebrationEvent -= ShowCelebration;
    }

    // ── Helpers ──────────────────────────────────────────────────────
    private void Subscribe()
    {
        if (CollectableManager.Instance == null)
        {
            Debug.LogWarning("[CollectableHUD] No CollectableManager found in scene.");
            return;
        }
        CollectableManager.Instance.OnNotesChanged          += UpdateNotes;
        CollectableManager.Instance.OnJigsawsChanged        += UpdateJigsaws;
        CollectableManager.Instance.OnJigsawCelebrationEvent += ShowCelebration;

        // Initialise display
        UpdateNotes(CollectableManager.Instance.NoteCount);
        UpdateJigsaws(CollectableManager.Instance.JigsawCount);
    }

    private void UpdateNotes(int count)
    {
        if (noteText != null)
            noteText.text = $"{count} / {CollectableManager.Instance.NotesRequired}";
    }

    private void UpdateJigsaws(int count)
    {
        if (jigsawText != null)
            jigsawText.text = $"{count} / {CollectableManager.Instance.JigsawsInLevel}";
    }

    private void ShowCelebration()
    {
        if (celebrationBanner == null) return;
        celebrationBanner.SetActive(true);
        _bannerTimer = bannerDisplayTime;
    }
}
