using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SiegeEnemyBehaviour : EnemyBehaviour
{
    private GameObject currTarget;
    private GameObject prevTarget;
    private float damageMod;

    void FixedUpdate()
    {
        currAttackTimer += Time.deltaTime;

        if (range > 0 && target && currAttackTimer > attackTimer)
        {
            ourBody.velocity = Vector3.zero;
            Attack();
            speed = 1;
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
            if (!target)
            {
                speed = startSpeed;
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

    public override IEnumerator UpdateTarget()
    {
        while (true)
        {
            prevTarget = currTarget;
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

            if (target)
            {
                currTarget = target.gameObject;
            }
            if (currTarget != prevTarget || currTarget == null)
            {
                damageMod = 0;
            }
            else
            {
                damageMod += 1f;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void Shoot()
    {
        fireSound.Play();
        BulletBehaviour newBullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponent<BulletBehaviour>();
        newBullet.speed = 10;
        newBullet.damage = damage + damageMod;
        newBullet.timeToLive = 5;
    }

    public override void Die()
    {
        ResourceSystem.money += value;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }
}
