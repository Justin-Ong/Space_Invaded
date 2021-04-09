using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracerBehaviour : MonoBehaviour
{
    public float speed;

    [Header("Pathing")]
    public List<Vector3> waypoints;
    public int currWaypointIndex = 0;

    [Header("Obstacle Avoidance")]
    public float raycastOffset;
    public LayerMask obstaclesMask;
    public float rayDistance;
    public float rotationSpeed = 2f;

    private Rigidbody ourBody;
    private float rotationMod;

    // Start is called before the first frame update
    void Start()
    {
        ourBody = GetComponent<Rigidbody>();
        rotationMod = 0;
    }

    void FixedUpdate()
    {
        {
            Vector3 newDirection;
            if (DetectObstacles(out newDirection))
            {
                AvoidObstacles(newDirection);
            }
            else
            {
                LookAtNextWaypoint();
            }
            Move();
            rotationMod += 0.01f;
            if (currWaypointIndex < waypoints.Count - 1 && (waypoints[currWaypointIndex] - transform.position).magnitude < 5)
            {
                currWaypointIndex++;
                rotationMod = 0;
            }
        }
    }

    private void LookAtNextWaypoint()
    {
        Vector3 waypointPosition = waypoints[currWaypointIndex] - transform.position;
        if (waypointPosition != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(waypoints[currWaypointIndex] - transform.position, transform.up);
            Turn(rotation);
        }
    }

    private void Move()
    {
        ourBody.velocity = transform.forward.normalized * speed;
    }

    private void Turn(Quaternion rotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (rotationSpeed + rotationMod) * Time.deltaTime);
    }

    private bool DetectObstacles(out Vector3 newDirection)
    {
        Vector3 forwardRayPos = transform.position;
        Vector3 leftRayPos = transform.position - transform.right * raycastOffset;
        Vector3 rightRayPos = transform.position + transform.right * raycastOffset;
        Vector3 upRayPos = transform.position + transform.up * Mathf.Min(raycastOffset, 1);
        Vector3 downRayPos = transform.position - transform.up * Mathf.Min(raycastOffset, 1);

        RaycastHit hit;
        bool obstacleDetected = false;
        newDirection = Vector3.zero;
        if (Physics.Raycast(forwardRayPos, transform.forward, out hit, rayDistance, obstaclesMask, QueryTriggerInteraction.Collide))
        {
            obstacleDetected = true;
        }
        if (Physics.Raycast(leftRayPos, transform.forward, out hit, rayDistance, obstaclesMask, QueryTriggerInteraction.Collide))
        {
            obstacleDetected = true;
            newDirection += Vector3.right;
        }
        if (Physics.Raycast(rightRayPos, transform.forward, out hit, rayDistance, obstaclesMask, QueryTriggerInteraction.Collide))
        {
            obstacleDetected = true;
            newDirection += Vector3.left;
        }
        if (Physics.Raycast(upRayPos, transform.forward, out hit, rayDistance, obstaclesMask, QueryTriggerInteraction.Collide))
        {
            obstacleDetected = true;
            newDirection += Vector3.down;
        }
        if (Physics.Raycast(downRayPos, transform.forward, out hit, rayDistance, obstaclesMask, QueryTriggerInteraction.Collide))
        {
            obstacleDetected = true;
            newDirection += Vector3.up;
        }
        return obstacleDetected;
    }

    private void AvoidObstacles(Vector3 newDirection)
    {
        Quaternion rotation;
        if (newDirection != Vector3.zero)
        {
            rotation = Quaternion.LookRotation(newDirection, transform.up);
        }
        else
        {
            {
                rotation = Quaternion.LookRotation(Vector3.left, transform.up);
            }
        }
        Turn(rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaypointBehaviour>() != null)
        {
            currWaypointIndex++;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DefencePointBehaviour>() != null)
        {
            Destroy(gameObject);
        }
    }
}
