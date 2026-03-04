using UnityEngine;
using UnityEngine.AI;

// Unified enemy movement: choose to use NavMeshAgent or physics (Rigidbody).
[RequireComponent(typeof(Rigidbody))]
public class EnemyMovementUnified : EnemyMovementBase
{
    [Header("Mode")]
    public bool useNavMesh = true;
    public NavMeshAgent agent;

    int currentWaypoint = 0;
    float waitTimer = 0f;
    float wanderTimer = 0f;
    Vector3 wanderTarget;

    void Start()
    {
        StartBase();
        if (useNavMesh && agent == null) agent = GetComponent<NavMeshAgent>();
        if (!useNavMesh) rb = GetComponent<Rigidbody>();
        ChooseNewWanderTarget();
    }

    void Update()
    {
        switch (pattern)
        {
            case MovementPattern.Stationary:
                StopAgent();
                break;

            case MovementPattern.Patrol:
                PatrolUpdate();
                break;

            case MovementPattern.Chase:
                ChaseUpdate();
                break;

            case MovementPattern.Wander:
                WanderUpdate();
                break;

            case MovementPattern.JumpPatrol:
                JumpPatrolUpdate();
                break;
        }
    }

    void StopAgent()
    {
        if (useNavMesh && agent != null) agent.isStopped = true;
    }

    void PatrolUpdate()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        Vector3 target = waypoints[currentWaypoint].position;
        SetDestination(target);
        float dist = Vector3.Distance(transform.position, target);
        if (dist <= waypointStopDistance)
        {
            if (waitTimer <= 0f) waitTimer = waypointWaitTime;
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    currentWaypoint++;
                    if (currentWaypoint >= waypoints.Length)
                    {
                        if (loopWaypoints) currentWaypoint = 0;
                        else currentWaypoint = waypoints.Length - 1;
                    }
                }
            }
        }
    }

    void ChaseUpdate()
    {
        if (player == null) return;
        float d = Vector3.Distance(transform.position, player.position);
        if (d <= chaseRange) SetDestination(player.position);
        else if (useNavMesh && agent != null) agent.isStopped = true;
    }

    void WanderUpdate()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f)
        {
            ChooseNewWanderTarget();
            wanderTimer = wanderInterval;
        }
        SetDestination(wanderTarget);
    }

    void JumpPatrolUpdate()
    {
        if (waypoints == null || waypoints.Length == 0) return;
        Vector3 target = waypoints[currentWaypoint].position;
        SetDestination(target);
        float dist = Vector3.Distance(transform.position, target);
        if (dist <= waypointStopDistance)
        {
            if (waitTimer <= 0f) waitTimer = waypointWaitTime;
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    if (IsGrounded() && rb != null) rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                    currentWaypoint++;
                    if (currentWaypoint >= waypoints.Length)
                    {
                        if (loopWaypoints) currentWaypoint = 0;
                        else currentWaypoint = waypoints.Length - 1;
                    }
                }
            }
        }
    }

    void SetDestination(Vector3 target)
    {
        if (useNavMesh && agent != null)
        {
            agent.isStopped = false;
            agent.SetDestination(target);
        }
        else MoveTowardsPhysics(target);
    }

    void MoveTowardsPhysics(Vector3 target)
    {
        Vector3 dir = (target - transform.position);
        dir.y = 0f;
        if (dir.sqrMagnitude < 0.0001f) return;
        Vector3 move = dir.normalized * speed;
        if (rb != null)
        {
            Vector3 vel = rb.linearVelocity;
            vel.x = move.x;
            vel.z = move.z;
            rb.linearVelocity = vel;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);
        }
        Quaternion targetRot = Quaternion.LookRotation(new Vector3(dir.x, 0f, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
    }

    void ChooseNewWanderTarget()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * wanderRadius;
        if (useNavMesh && agent != null)
        {
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, wanderRadius, NavMesh.AllAreas)) wanderTarget = hit.position;
            else wanderTarget = transform.position;
        }
        else
        {
            Vector3 rand = Random.insideUnitSphere * wanderRadius;
            rand.y = 0f;
            wanderTarget = transform.position + rand;
            RaycastHit hit;
            if (Physics.Raycast(wanderTarget + Vector3.up * 5f, Vector3.down, out hit, 20f, groundMask)) wanderTarget = hit.point;
        }
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, 0.2f, groundMask);
    }
}
