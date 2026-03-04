using UnityEngine;

public class PageCollectable
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the Player (using its Tag)
        if (other.CompareTag("Player"))
        {
            // Get the PlayerController script from the colliding object and call CollectItem()
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.CollectItem();
                Destroy(gameObject); // Destroy the collected item
            }
        }
    }
}
