using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            newEnemy.isElite = isElite;
        }
        ResourceSystem.money += value;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }
}
