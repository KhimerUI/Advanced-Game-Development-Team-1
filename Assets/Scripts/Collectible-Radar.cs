using UnityEngine;

public class CollectibleRadar : MonoBehaviour
{
    public float radius = 10f;
    public KeyCode pulseKey = KeyCode.E;
    public LayerMask collectibleLayer;

    private void Update()
    {
        if (Input.GetKeyDown(pulseKey))
        {
            Pulse();
        }
    }

    void Pulse()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, radius, collectibleLayer);

        foreach (Collider hit in hits)
        {
            Debug.Log("Detected collectible: " + hit.name);

            // Optional: highlight effect
            Renderer r = hit.GetComponent<Renderer>();
            if (r != null)
            {
                r.material.color = Color.yellow;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}