using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Attributes")]
    public float maxHealth;
    public float speed;

    [Header("Attacking")]
    public GameObject bulletPrefab;
    public float attackTimer;
    public float range;
    public float damage;
    public string turretTag = "Turret";

    [Header("Pathing")]
    public List<Vector3> waypoints;
    public int currWaypointIndex = 0;

    [Header("Obstacle Avoidance")]
    public float raycastOffset;
    public LayerMask obstaclesMask;
    public float rayDistance;
    public float rotationSpeed = 2f;

    protected Rigidbody ourBody;
    protected HealthSystem ourHealth;
    protected float currAttackTimer;
    protected float rotationMod;
    protected Transform target;

    // Start is called before the first frame update
    void Start()
    {
        ourBody = GetComponent<Rigidbody>();
        ourHealth = GetComponent<HealthSystem>();
        rotationMod = 0;
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void FixedUpdate()
    {
        currAttackTimer += Time.deltaTime;

        if (target && currAttackTimer > attackTimer)
        {
            ourBody.velocity = Vector3.zero;
            Attack();
            currAttackTimer = 0;
        }
        else
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

    void UpdateTarget()
    {
        GameObject[] turrets = GameObject.FindGameObjectsWithTag(turretTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestTurret = null;

        foreach (GameObject turret in turrets)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, turret.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestTurret = turret;
            }
        }

        // update if target is within range
        if (nearestTurret != null && shortestDistance <= range)
        {
            target = nearestTurret.transform;
        }
        else
        {
            target = null;
        }
    }

    public virtual void Attack()
    {
        Quaternion prevRotation = transform.rotation;
        transform.LookAt(target.position, Vector3.up);
        Shoot();
        transform.rotation = prevRotation;
    }

    public virtual void Shoot()
    {
        BulletBehaviour newBullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponent<BulletBehaviour>();
        newBullet.speed = 10;
        newBullet.damage = damage;
        newBullet.timeToLive = 5;
    }

    protected void LookAtNextWaypoint()
    {
        Quaternion rotation = Quaternion.LookRotation(waypoints[currWaypointIndex] - transform.position, transform.up);
        Turn(rotation);
    }

    protected void Move()
    {
        ourBody.velocity = transform.forward.normalized * speed;
    }

    protected void Turn(Quaternion rotation)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, (rotationSpeed + rotationMod) * Time.deltaTime);
    }

    protected bool DetectObstacles(out Vector3 newDirection)
    {
        Vector3 forwardRayPos = transform.position;
        Vector3 leftRayPos = transform.position - transform.right * raycastOffset;
        Vector3 rightRayPos = transform.position + transform.right * raycastOffset;
        Vector3 upRayPos = transform.position + transform.up * Mathf.Min(raycastOffset, 1);
        Vector3 downRayPos = transform.position - transform.up * Mathf.Min(raycastOffset, 1);

        /*
        Debug.DrawRay(forwardRayPos, transform.forward * rayDistance * 0.8f, Color.red);
        Debug.DrawRay(leftRayPos, transform.forward * rayDistance, Color.red);
        Debug.DrawRay(rightRayPos, transform.forward * rayDistance, Color.red);
        Debug.DrawRay(upRayPos, transform.forward * rayDistance, Color.red);
        Debug.DrawRay(downRayPos, transform.forward * rayDistance, Color.red);
        */

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

    protected void AvoidObstacles(Vector3 newDirection)
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

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<WaypointBehaviour>() != null)
        {
            currWaypointIndex++;
        }
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<DefencePointBehaviour>() != null)
        {
            References.defencePointObject.currBaseHealth -= 1;
            ourHealth.TakeDamage(maxHealth);
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }
}