using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZoneScript : MonoBehaviour
{
    public PlayerMovement playerMove;
    private Transform player;
    private GameObject[] spawnPoints;
    private float respawnDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (spawnPoints == null)
            {
                spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            }

            playerMove.lives -= 1;

            int rand = Random.Range(0, spawnPoints.Length);
            Transform spawnPos = spawnPoints[rand].transform;

            other.transform.position = spawnPos.position;
            other.transform.rotation = spawnPos.rotation;
        }
    }
}
