using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public override void Die()
    {
        ResourceSystem.money += 30;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }
}
