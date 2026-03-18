using System.Collections;
using UnityEngine;

/*
===========================================================
Falling Platform Script
===========================================================

HOW TO USE:

1. Attach this script to your platform GameObject.
2. Make sure the platform has:
   - A Collider (e.g., BoxCollider)
   - A Rigidbody component
3. Set the Rigidbody to:
   - "Is Kinematic" = true (so it doesn't fall immediately)
4. Make sure your Player GameObject is tagged as "Player".
   - Select Player in Hierarchy → Tag dropdown → choose "Player"

WHAT THIS SCRIPT DOES:

- When the player touches the platform:
    → Starts a timer (default: 5 seconds)
- After the timer:
    → Enables gravity so the platform falls
- After falling for a short time:
    → The platform deletes itself from the scene

You can adjust:
- delayBeforeFall (how long before it drops)
- lifetimeAfterFall (how long before it disappears)

===========================================================
*/

public class FallingPlatform : MonoBehaviour
{
    // Time before the platform starts falling after player touches it
    public float delayBeforeFall = 5f;

    // Time after falling before the platform is destroyed
    public float lifetimeAfterFall = 2f;

    // Reference to the Rigidbody component
    private Rigidbody rb;

    // Prevents the trigger from running multiple times
    private bool hasTriggered = false;

    void Start()
    {
        // Get the Rigidbody component attached to this platform
        rb = GetComponent<Rigidbody>();

        // Safety check: make sure Rigidbody exists
        if (rb == null)
        {
            Debug.LogError("No Rigidbody found on platform!");
        }
    }

    // This function runs when something collides with the platform
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object that hit the platform is the player
        if (collision.gameObject.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true; // Prevent retriggering

            // Start the falling sequence
            StartCoroutine(FallSequence());
        }
    }

    // Coroutine that handles the delay, falling, and destruction
    IEnumerator FallSequence()
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delayBeforeFall);

        // Enable physics so the platform falls
        if (rb != null)
        {
            rb.isKinematic = false; // Turns physics ON
            rb.useGravity = true;   // Enables gravity
        }

        // Wait before destroying the platform
        yield return new WaitForSeconds(lifetimeAfterFall);

        // Destroy the platform GameObject
        Destroy(gameObject);
    }
}