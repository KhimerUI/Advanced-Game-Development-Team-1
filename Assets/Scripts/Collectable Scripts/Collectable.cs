using System.Collections;
using UnityEngine;


/// Base class for all collectables. Handles bobbing, spinning, magnetism,
/// pickup effects, and sound — just like a classic 3D platformer.
/// Extend this class to create Notes, Jiggies, Feathers, etc.

public abstract class Collectable : MonoBehaviour
{
    [Header("Collectable Identity")]
    [Tooltip("Display name shown in UI on pickup.")]
    public string collectableName = "Collectable";
    public int value = 1;

    [Header("Bob & Spin")]
    public bool enableBob = true;
    public float bobHeight = 0.3f;
    public float bobSpeed = 2f;

    public bool enableSpin = true;
    public float spinSpeed = 90f; // degrees per second

    [Header("Magnet / Attract")]
    [Tooltip("How close the player must be before the collectable flies toward them.")]
    public float magnetRadius = 4f;
    public float magnetSpeed = 12f;
    [Tooltip("How close to snap-collect once attracted.")]
    public float collectRadius = 0.5f;

    [Header("Pickup Flash")]
    public bool flashOnCollect = true;
    public float flashDuration = 0.15f;
    public Color flashColor = Color.white;

    [Header("Audio")]
    public AudioClip collectSound;
    [Range(0f, 1f)] public float collectVolume = 1f;

    [Header("Particles")]
    public GameObject collectParticlePrefab;

    // ── Internal state ──────────────────────────────────────────────
    protected bool isCollected = false;
    private Vector3 _startPos;
    private float _bobTimer;
    private Transform _playerTransform;
    private Renderer[] _renderers;
    private Color[] _originalColors;

    // ── Unity lifecycle ─────────────────────────────────────────────
    protected virtual void Start()
    {
        _startPos = transform.position;
        _renderers = GetComponentsInChildren<Renderer>();
        CacheOriginalColors();

        // Randomise bob/bounce phase so nearby collectables don't sync
        _bobTimer = Random.Range(0f, Mathf.PI * 2f);
    }

    protected virtual void Update()
    {
        if (isCollected) return;

        HandleBob();
        HandleSpin();
        HandleMagnet();
    }

    // ── Movement ────────────────────────────────────────────────────
    private void HandleBob()
    {
        if (!enableBob) return;
        _bobTimer += Time.deltaTime * bobSpeed;
        float newY = _startPos.y + Mathf.Sin(_bobTimer) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void HandleSpin()
    {
        if (!enableSpin) return;
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }

    private void HandleMagnet()
    {
        if (_playerTransform == null)
        {
            // Lazy-find the player (works with any tag)
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null) _playerTransform = player.transform;
            return;
        }

        float dist = Vector3.Distance(transform.position, _playerTransform.position);

        if (dist <= collectRadius)
        {
            Collect(_playerTransform);
            return;
        }

        if (dist <= magnetRadius)
        {
            // Fly toward player
            transform.position = Vector3.MoveTowards(
                transform.position,
                _playerTransform.position,
                magnetSpeed * Time.deltaTime
            );
        }
    }

    // ── Collect ─────────────────────────────────────────────────────
    /// called when the collectable is picked up.</summary>
    public void Collect(Transform collector)
    {
        if (isCollected) return;
        isCollected = true;

        OnCollected(collector);
        StartCoroutine(CollectRoutine());
    }

    ///Override in subclasses to add score, inventory logic, etc.</summary>
    protected abstract void OnCollected(Transform collector);

    private IEnumerator CollectRoutine()
    {
        // Sound
        if (collectSound != null)
            AudioSource.PlayClipAtPoint(collectSound, transform.position, collectVolume);

        // Particles
        if (collectParticlePrefab != null)
            Instantiate(collectParticlePrefab, transform.position, Quaternion.identity);

        // Flash
        if (flashOnCollect)
            yield return StartCoroutine(FlashRoutine());

        // Scale-pop then destroy
        yield return StartCoroutine(ScalePop());

        Destroy(gameObject);
    }

    // ── Visual FX ───────────────────────────────────────────────────
    private IEnumerator FlashRoutine()
    {
        SetColor(flashColor);
        yield return new WaitForSeconds(flashDuration);
        RestoreColors();
    }

    private IEnumerator ScalePop()
    {
        float t = 0f;
        Vector3 original = transform.localScale;
        Vector3 big = original * 1.4f;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.12f;
            transform.localScale = Vector3.Lerp(original, big, Mathf.PingPong(t, 1f));
            yield return null;
        }
        transform.localScale = Vector3.zero;
    }

    private void CacheOriginalColors()
    {
        _originalColors = new Color[_renderers.Length];
        for (int i = 0; i < _renderers.Length; i++)
            _originalColors[i] = _renderers[i].material.color;
    }

    private void SetColor(Color c)
    {
        foreach (var r in _renderers)
            r.material.color = c;
    }

    private void RestoreColors()
    {
        for (int i = 0; i < _renderers.Length; i++)
            _renderers[i].material.color = _originalColors[i];
    }

    // ── Trigger fallback (no magnet needed) ─────────────────────────
    private void OnTriggerEnter(Collider other)
    {
        if (!isCollected && other.CompareTag("Player"))
            Collect(other.transform);
    }
};