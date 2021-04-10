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

        if (target)
        {
            ourBody.velocity = Vector3.zero;
            transform.LookAt(target.position, Vector3.up);
            if (currAttackTimer > attackTimer)
            {
                Attack();
                currAttackTimer = 0;
            }
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

    public override IEnumerator UpdateTarget()
    {
        while (true)
        {
            prevTarget = currTarget;
            base.UpdateTarget();
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
                damageMod += 2.5f;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    public override void Shoot()
    {
        BulletBehaviour newBullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponent<BulletBehaviour>();
        newBullet.speed = 10;
        newBullet.damage = damage + damageMod;
        newBullet.timeToLive = 5;
    }

    public override void Die()
    {
        ResourceSystem.money += 30;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }
}
