using System.Collections;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    //boolean variables for each powerup, each starts as false, but when the player interacts with the specific
    //Powerup the corresponding boolean is set to true.

    public bool _isMultiShotActive = false;

    //Powerup timelimit can be increased by collecting more PowerUps
    private float powerUpTimeLimit = 10.0f;

    // OnTrigger Collision: When the player touches the PowerUp, it will be destroyed.
    // - Powerups are only collectible by the Player tag
    // - Destroy powerup after pickup

    public void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player")) 
        {
            Destroy(this.gameObject);
        }
    }

    //Below, we can add methods for various powerups. Right now, I have added a Multishot.
    //MultishotActive becomes true, and then a coroutine is started for the powerup's timelimit
    public void MultishotActive() 
    {
        //Note to self: add a way to detect which PowerUp is which
        _isMultiShotActive = true;

        //start courutine
        StartCoroutine(MultiShotPowerDownRoutine());
    }

    IEnumerator MultiShotPowerDownRoutine()
    {
        // wait x amount of seconds
        yield return new WaitForSeconds(powerUpTimeLimit);
        // turn of the multi-shot
        _isMultiShotActive = false;
    }

    //The Multishot method can be copied and used for the other powerups. Need projectile code to start expanding
    // upon PowerUps

    
}
