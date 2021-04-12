using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Attributes")]
    public float maxHealth;
    public float speed;
    public float startSpeed;
    public bool isElite = false;

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
        startSpeed = speed;
        ourBody = GetComponent<Rigidbody>();
        ourHealth = GetComponent<HealthSystem>();
        rotationMod = 0;
        StartCoroutine("UpdateTarget");
        if (isElite) {
            damage *= 1.5f;
            maxHealth *= 1.5f;
            ourHealth.maxHealth *= 1.5f;
            ourHealth.currHealth = ourHealth.maxHealth;
            Material[] mats = gameObject.transform.Find("Model").GetComponent<Renderer>().materials;
            foreach (Material mat in mats)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.red);
            }
        }
    }

    void FixedUpdate()
    {

        currAttackTimer += Time.deltaTime;

        if (range > 0 && target && currAttackTimer > attackTimer)
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
        
        // always reset enemy speed
        speed = startSpeed;
    }

    public virtual IEnumerator UpdateTarget()
    {
        while (true)
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

            yield return new WaitForSeconds(0.5f);
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
        Vector3 waypointPosition = waypoints[currWaypointIndex] - transform.position;
        if (waypointPosition != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(waypoints[currWaypointIndex] - transform.position, transform.up);
            Turn(rotation);
        }
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
        ResourceSystem.money += 10;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }

    public void Slow(float pct)
    {
        speed = startSpeed * (1f - pct);
    }

    public void ResetSpeed()
    {
        speed = startSpeed;
    }
}