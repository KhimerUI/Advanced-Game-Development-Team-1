/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required for accessing UI components

// IMPORTANT NOTE:
// This code and cs was done in unity 6000.1.17 as such it will be uploaded in different script than the actual gun.cs acript
// Since where switching to a different unity engine I will be downloading the new one we are using.
// Also this uses inputed key and it should be changed in player input manager to be using both.


public class Gun : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;

    // New variables for ammo management
    public int maxAmmo = 30; // Maximum bullets the gun can hold
    private int currentAmmo; // Current remaining bullets
    public Text ammoText; // Reference to the UI Text element

    void Start()
    {
        currentAmmo = maxAmmo; // Initialize current ammo on scene start
        UpdateAmmoUI(); // Update UI immediately
    }

    void Update()
    {
        // Change from GetKeyDown to GetButtonDown (or another GetKey, up to you) for continuous holding/shooting if desired
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Check if there is enough ammo to shoot
            if (currentAmmo > 0)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // Instantiate the bullet
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        bullet.GetComponent<Rigidbody>().linearVelocity = bulletSpawnPoint.forward * bulletSpeed;

        // Waste a bullet (decrement ammo count)
        currentAmmo--;

        // Update the UI text
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        // Display current ammo / max ammo
        if (ammoText != null)
        {
            ammoText.text = "Ammo: " + currentAmmo.ToString() + " / " + maxAmmo.ToString();
        }
    }
}
*/