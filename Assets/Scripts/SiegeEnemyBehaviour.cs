using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiegeEnemyBehaviour : EnemyBehaviour
{
    private GameObject currTarget;
    private GameObject prevTarget;
    private float damageMod;

    public override void AttackClosestTower(GameObject target)
    {
        currTarget = target;
        if (currTarget != prevTarget)
        {
            damageMod = 0;
        }
        else
        {
            damageMod += 1;
        }
        prevTarget = currTarget;
        base.AttackClosestTower(target);
    }

    public override void Shoot()
    {
        BulletBehaviour newBullet = Instantiate(bulletPrefab, transform.position + transform.forward, transform.rotation).GetComponent<BulletBehaviour>();
        newBullet.speed = 10;
        newBullet.damage = damage + damageMod;
        newBullet.timeToLive = 5;
    }
}
