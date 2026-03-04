
using UnityEngine;


/// Health collectable — restores health to the player on pickup.
/// Works like honeycomb pieces in Banjo-Kazooie.
/// Requires the collector to have a component implementing IHealable.

public class HealthCollectable : Collectable
{
    [Header("Health Settings")]
    public int healAmount = 1;

    protected override void Start()
    {
        collectableName = "Honeycomb";
        value = healAmount;
        bobSpeed = 1.5f;
        spinSpeed = 60f;
        magnetRadius = 3f;
        base.Start();
    }

    protected override void OnCollected(Transform collector)
    {
        IHealable healable = collector.GetComponent<IHealable>();
        if (healable != null)
        {
            healable.Heal(healAmount);
            Debug.Log($"[Health] Healed player for {healAmount}.");
        }
        else
        {
            Debug.LogWarning("[HealthCollectable] Collector has no IHealable component.");
        }

        CollectableManager.Instance?.OnHealthCollected(healAmount);
    }
}


/// Implement this on your PlayerHealth (or similar) component
/// so HealthCollectable can heal without a hard reference.
public interface IHealable
{
    void Heal(int amount);
}
