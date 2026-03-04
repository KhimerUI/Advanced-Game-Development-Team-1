using UnityEngine;

public enum MovementPattern { Stationary, Patrol, Chase, Wander, JumpPatrol }

public abstract class EnemyMovementBase : MonoBehaviour, IEnemyMovement
{
    [Header("General")]
    public MovementPattern pattern = MovementPattern.Patrol;
    public float speed = 3f;
    public float rotationSpeed = 10f;

    [Header("Patrol")]
    public Transform[] waypoints;
    public bool loopWaypoints = true;
    public float waypointStopDistance = 0.5f;
    public float waypointWaitTime = 1f;

    [Header("Chase")]
    public string playerTag = "Player";
    public float chaseRange = 8f;

    [Header("Wander")]
    public float wanderRadius = 5f;
    public float wanderInterval = 3f;

    [Header("Jump")]
    public float jumpForce = 5f;
    public LayerMask groundMask;

    protected Rigidbody rb;
    protected Transform player;

    protected virtual void StartBase()
    {
        rb = GetComponent<Rigidbody>();
        var p = GameObject.FindGameObjectWithTag(playerTag);
        if (p) player = p.transform;
    }

    public MovementPattern GetPattern() => pattern;
    public void SetPattern(MovementPattern p) => pattern = p;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (waypoints[i] == null) continue;
                Gizmos.DrawSphere(waypoints[i].position, 0.25f);
                if (i + 1 < waypoints.Length && waypoints[i + 1] != null) Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, wanderRadius);
    }
}
