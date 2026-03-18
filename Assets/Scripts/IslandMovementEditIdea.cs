/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// IMPORTANT NOTE:
// This code and cs was done in unity 6000.1.17 as such it will be uploaded in different script than the actual IslandMovement.cs acript
// Since where switching to a different unity engine I will be downloading the new one we are using.
// Also this uses inputed key and it should be changed in player input manager to be using both.

public class IslandMove : MonoBehaviour
{
    [Header("Movement Speeds (units per second)")]
    public float speedUp = 2.0f;
    public float speedDown = 2.0f;
    public float speedLeft = 2.0f;
    public float speedRight = 2.0f;

    [Header("Enable/Disable Directions")]
    public bool canMoveUp = true;
    public bool canMoveDown = true;
    public bool canMoveLeft = true;
    public bool canMoveRight = true;

    // You can remove pointA, pointB, speed, and moveTowardsB from the original script
    // if they are no longer needed.
    // public Transform pointA;
    // public Transform pointB;
    // private bool moveTowardsB = true;

    // Start is called before the first frame update
    void Start()
    {
        // Initial setup if needed
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementVector = Vector3.zero;

        // Calculate movement based on enabled directions and specific speeds
        if (canMoveUp)
        {
            // Move up (positive Y axis)
            movementVector += Vector3.up * speedUp;
        }
        if (canMoveDown)
        {
            // Move down (negative Y axis)
            movementVector += Vector3.down * speedDown;
        }
        if (canMoveRight)
        {
            // Move right (positive X axis)
            movementVector += Vector3.right * speedRight;
        }
        if (canMoveLeft)
        {
            // Move left (negative X axis)
            movementVector += Vector3.left * speedLeft;
        }

        // Apply movement scaled by Time.deltaTime to ensure consistent speed across different frame rates
        // The default space is local, which is usually fine, but can be specified to Space.World if needed
        transform.Translate(movementVector * Time.deltaTime, Space.World);
    }
}
*/