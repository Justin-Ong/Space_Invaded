using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusEnemyBehaviour : EnemyBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        currAttackTimer += Time.deltaTime;

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
        else if (currWaypointIndex == waypoints.Count - 1 && (waypoints[currWaypointIndex] - transform.position).magnitude < 5) {
            Destroy(gameObject);
        }
    }

    public override void Die()
    {
        ResourceSystem.money += 50;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }
}
