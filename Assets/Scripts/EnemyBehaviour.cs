using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float maxHealth;
    public float speed;
    public List<Vector3> waypoints;
    public GameObject bulletPrefab;
    public float attackTimer;
    public float range;
    public float damage;

    private Rigidbody ourBody;
    private HealthSystem ourHealth;
    private int currWaypointIndex;
    private float currAttackTimer;

    // Start is called before the first frame update
    void Start()
    {
        ourBody = GetComponent<Rigidbody>();
        ourHealth = GetComponent<HealthSystem>();
        currWaypointIndex = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        currAttackTimer += Time.deltaTime;

        List<GameObject> nearbyTowers = getNearbyTowers();

        if (nearbyTowers.Count > 0)
        {
            if (currAttackTimer > attackTimer)
            {
                ourBody.velocity = Vector3.zero;
                attackClosestTower(nearbyTowers);
                currAttackTimer = 0;
            }
        }
        else
        {
            moveToNextWaypoint();
        }
    }

    private List<GameObject> getNearbyTowers()
    {
        List<GameObject> nearbyTowers = new List<GameObject>();
        Collider[] nearbyTowerColliders = Physics.OverlapSphere(transform.position, range, 1 << 3);    //Layer 3 mask to detect turrets only
        for (int i = 0; i < nearbyTowerColliders.Length; i++)
        {
            nearbyTowers.Add(nearbyTowerColliders[i].gameObject);
        }
        return nearbyTowers;
    }

    private void attackClosestTower(List<GameObject> nearbyTowers)
    {
        int nearestTowerIndex = 0;
        float closestDistance = 50;
        for (int i = 0; i < nearbyTowers.Count; i++)
        {
            float distance = Mathf.Abs((nearbyTowers[i].transform.position - transform.position).magnitude);
            if (distance < closestDistance)
            {
                nearestTowerIndex = i;
            }
        }
        transform.LookAt(2 * transform.position - nearbyTowers[nearestTowerIndex].transform.position, Vector3.up);
        Shoot();
    }

    private void Shoot()
    {
        BulletBehaviour newBullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponent<BulletBehaviour>();
        newBullet.speed = 10;
        newBullet.damage = damage;
        newBullet.timeToLive = 5;
        newBullet.isFriendly = false;
    }

    private void moveToNextWaypoint()
    {
        Vector3 vectorToPlayer = waypoints[currWaypointIndex] - transform.position;
        Vector3 distanceToTravel = vectorToPlayer.normalized * speed;
        ourBody.velocity = distanceToTravel;
        transform.LookAt(waypoints[currWaypointIndex], Vector3.up);
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
            ourHealth.TakeDamage(maxHealth);
        }
    }
}