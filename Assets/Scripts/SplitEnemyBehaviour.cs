using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitEnemyBehaviour : EnemyBehaviour
{
    public GameObject spawnedEnemyPrefab;
    public int numSpawns;

    public override void Die()
    {
        for (int i = 0; i < numSpawns; i++)
        {
            GameObject temp = Instantiate(spawnedEnemyPrefab, transform.position + new Vector3(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1)), transform.rotation);
            SpawnedEnemyBehaviour newEnemy = temp.GetComponent<SpawnedEnemyBehaviour>();
            newEnemy.waypoints = waypoints;
            newEnemy.currWaypointIndex = currWaypointIndex;
        }
        Destroy(gameObject);
    }
}
