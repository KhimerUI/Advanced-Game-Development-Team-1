using UnityEngine;

public class ShockJumpPad : MonoBehaviour
{
    [Header("Launch Settings")]
    [SerializeField] private float launchForce = 18f;      // Tweak this for feel
    [SerializeField] private bool overrideYVelocity = true; // Kills downward momentum before launching

    [Header("Feedback (Optional — hook these up later)")]
    [SerializeField] private AudioClip launchSound;
    [SerializeField] private ParticleSystem launchParticles; // wind or sparkles (or both)

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // If your PlayerController holds a reference to the Rigidbody,
        //    you could also do: other.GetComponent<PlayerController>().rb
        //    For now we grab it directly from the colliding object.
        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null) return;

        Launch(rb);
    }

    private void Launch(Rigidbody rb)
    {
        if (overrideYVelocity)
        {
            // Preserve horizontal momentum, wipe vertical so launch height is always consistent
            Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.linearVelocity = flatVelocity;
        }

        rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse);

        PlayFeedback();
    }

    private void PlayFeedback() //sfx (maybe a woosh idk up to whomever does sounds for this)
    {
        if (launchSound != null && audioSource != null)
            audioSource.PlayOneShot(launchSound);

        if (launchParticles != null)
            launchParticles.Play();
    }
}