using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarrierEnemyBehaviour : EnemyBehaviour
{
    public GameObject spawnedEnemyPrefab;
    public float spawnTimer;

    private float currSpawnTime;
    private List<GameObject> emptyList = new List<GameObject>();

    public void Update()
    {
        currSpawnTime += Time.deltaTime;
        if (currSpawnTime > spawnTimer)
        {
            GameObject temp = Instantiate(spawnedEnemyPrefab, transform.position + new Vector3(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1)), transform.rotation);
            EnemyBehaviour newEnemy = temp.GetComponent<EnemyBehaviour>();
            newEnemy.waypoints = waypoints;
            newEnemy.currWaypointIndex = currWaypointIndex;
            currSpawnTime = 0;
        }
    }

    // Override target finding so carrier never attacks
    public override List<GameObject> GetNearbyTowers()
    {
        return emptyList;
    }
}
