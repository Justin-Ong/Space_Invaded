using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{
    private Transform target;

    [Header("General")]
    public float range = 15f;
    public float damage = 5f;

    [Header("Use Bullets (default)")]
    public GameObject bulletPrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    [Header("Use Laser")]
    public bool userLaser = false;
    public int damageOverTime = 10;
    public LineRenderer lineRenderer;

    [Header("Use Laser Slow (No Damager)")]
    public bool useSlowLaser = false;
    public float slowPercentage = 0.8f;

    [Header("Unity Setup Fields")]
    public string enemyTag = "Enemy";

    public Transform partToRotate;
    public float turnSpeed = 10f;

    public Transform firePoint;

    public Node node;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.5f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach(GameObject enemy in enemies)
        {
            // update which is the nearest enemy
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        // update if target is within range
        if (nearestEnemy != null && shortestDistance <= range) 
        {
            target = nearestEnemy.transform;
        } else 
        {
            target = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) 
        {
            if (userLaser || useSlowLaser)
            {
                if (lineRenderer.enabled)
                    lineRenderer.enabled = false;
            }
            return;
        }

        LockOnTarget();

        if (userLaser)
        {
            Laser();
        }
        else if (useSlowLaser){
            SlowLaser();
        }
        else
        {
            // control turret shooting interval
            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }
            fireCountdown -= Time.deltaTime;
        }

    }

    void SlowLaser()
    {
        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;

        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        target.GetComponent<EnemyBehaviour>().Slow(slowPercentage);
    }

    void Laser()
    {
        if (!lineRenderer.enabled)
            lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePoint.position);
        lineRenderer.SetPosition(1, target.position);

        target.GetComponent<HealthSystem>().TakeDamage(damageOverTime * Time.deltaTime);
    }

    void LockOnTarget()
    {
        // rotate turret towards enemy
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(rotation.x, rotation.y, 0f); // rotate only y-axis
    }

    void Shoot() 
    {
        GameObject bulletGO = (GameObject) Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        bullet.damage = damage;

        if (bullet != null) {
            bullet.Seek(target);
        }
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    public void Die()
    {
        node.RemoveTurret();
        Destroy(gameObject.transform.root.gameObject);
    }
}
