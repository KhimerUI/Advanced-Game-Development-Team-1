using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public string targetTag = "Collectable";
    public float updateInterval = 0.5f;
    public TMP_Text radarText;
    public TMP_Text pageText;
    public TMP_Text lifeText;
    public TMP_Text objText;
    public float jumpForce = 5f;
    public int collected = 0;
    public int lives = 3;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public Transform enemy;
    public float projectileSpeed = 20f;
    private bool isGrounded = true;
    private Rigidbody rb;
    private float nearestDistance = Mathf.Infinity;
    private Quaternion originalRotation;
    private Transform player;
    private GameObject[] spawnPoints;
    private float respawnDelay = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(UpdateNearestDistance), 0f, updateInterval);
        rb = GetComponent<Rigidbody>();
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (lives != 0)
        {
            Vector3 position = transform.position;

            if (Input.GetKey(KeyCode.A))
            {
                position.x -= speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D))
            {
                position.x += speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W))
            {
                position.z += speed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.S))
            {
                position.z -= speed * Time.deltaTime;
            }
            if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                ResetRotation();
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                FireAtEnemy();
            }

            transform.position = position;

            if (collected == 1)
            {
                pageText.text = "1/5";
            }
            else if (collected == 2)
            {
                pageText.text = "2/5";
            }
            else if (collected == 3)
            {
                pageText.text = "3/5";
            }
            else if (collected == 4)
            {
                pageText.text = "4/5";
            }
            else if (collected == 5)
            {
                pageText.text = "5/5";
                objText.text = "You Win";
            }

            if (lives == 3)
            {
                lifeText.text = "3/3";
            }
            else if (lives == 2)
            {
                lifeText.text = "2/3";
            }
            else if (lives == 1)
            {
                lifeText.text = "1/3";
            }
            else if (lives == 0)
            {
                lifeText.text = "0/3";
                objText.text = "You Lose";
            }
        }
        else if (collected == 5)
        {
            if (collected == 1)
            {
                pageText.text = "1/5";
            }
            else if (collected == 2)
            {
                pageText.text = "2/5";
            }
            else if (collected == 3)
            {
                pageText.text = "3/5";
            }
            else if (collected == 4)
            {
                pageText.text = "4/5";
            }
            else if (collected == 5)
            {
                pageText.text = "5/5";
                objText.text = "You Win";
            }

            if (lives == 3)
            {
                lifeText.text = "3/3";
            }
            else if (lives == 2)
            {
                lifeText.text = "2/3";
            }
            else if (lives == 1)
            {
                lifeText.text = "1/3";
            }
            else if (lives == 0)
            {
                lifeText.text = "0/3";
            }
        }
        else if (lives == 0)
        {
            if (collected == 1)
            {
                pageText.text = "1/5";
            }
            else if (collected == 2)
            {
                pageText.text = "2/5";
            }
            else if (collected == 3)
            {
                pageText.text = "3/5";
            }
            else if (collected == 4)
            {
                pageText.text = "4/5";
            }
            else if (collected == 5)
            {
                pageText.text = "5/5";
            }

            if (lives == 3)
            {
                lifeText.text = "3/3";
            }
            else if (lives == 2)
            {
                lifeText.text = "2/3";
            }
            else if (lives == 1)
            {
                lifeText.text = "1/3";
            }
            else if (lives == 0)
            {
                lifeText.text = "0/3";
                objText.text = "You Lose";
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (spawnPoints == null)
            {
                spawnPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
            }

            lives -= 1;

            int rand = Random.Range(0, spawnPoints.Length);
            Transform spawnPos = spawnPoints[rand].transform;

            transform.position = spawnPos.position;
            transform.rotation = spawnPos.rotation;
        }
    }

    void UpdateNearestDistance()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);

        if (targets.Length == 0)
        {
            nearestDistance = Mathf.Infinity;
            radarText.text = "N/A";
            return;
        }

        float minDist = Mathf.Infinity;
        Vector3 playerPos = transform.position;

        foreach (GameObject target in targets)
        {
            if (target == null) continue;

            float dist = Vector3.Distance(playerPos, target.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
            }
        }

        nearestDistance = minDist;
        radarText.text = "" + nearestDistance + "";
    }

    private void ResetRotation()
    {
        transform.rotation = originalRotation;
    }

    void FireAtEnemy()
    {
        Vector3 direction = (enemy.position - firePoint.position).normalized;

        float offsetDistance = 1.0f;
        Vector3 offsetPosition = firePoint.position + direction * offsetDistance;

        GameObject projectile = Instantiate(projectilePrefab, offsetPosition, Quaternion.identity);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        projectile.transform.forward = direction;
    }
}
