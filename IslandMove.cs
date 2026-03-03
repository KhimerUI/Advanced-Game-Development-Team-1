using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandMove : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;
    private bool moveTowardsB = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPosition = moveTowardsB ? pointB.position : pointA.position;

        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if(Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            moveTowardsB = !moveTowardsB;
        }
    }
}
