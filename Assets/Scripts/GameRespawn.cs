using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRespawn : MonoBehaviour
{
    //threshold is for when the player falls into a pit.
    // the threshold has to be a negative number or the
    // player can't move.
    public float threshold;

    // playerPosition is to get the player's position at their spawn point. Checkpoint will change that
    // position when the player hits it and the position of the checkpoint will be saved in newSpawn
    public Vector3 playerPosition;
    [SerializeField] List<GameObject> checkpoint;
    [SerializeField] Vector3 newSpawn;

    void Update()
    {
        if(transform.position.y < threshold) 
        {
            GetComponent<PlayerController>().enabled = false;
            transform.position = new Vector3(playerPosition.x, playerPosition.y, playerPosition.z);
            GetComponent<PlayerController>().enabled = true;
            // Add health system here

        }
    }

    // function for checkpoints. If the player collides with a checkpoint it sets their new spawnpoint 
    // to the checkpoint
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Checkpoint"))
        {
            newSpawn = other.transform.position;
            playerPosition = newSpawn;
        }
    }
}
