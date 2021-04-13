using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarrierEnemyBehaviour : EnemyBehaviour
{
    public GameObject spawnedEnemyPrefab;
    public float spawnTimer;

    private float currSpawnTime;
    private List<GameObject> minionsList = new List<GameObject>();

    public void Update()
    {
        currSpawnTime += Time.deltaTime;
        if (currSpawnTime > spawnTimer && minionsList.Count < 5)
        {
            GameObject temp = Instantiate(spawnedEnemyPrefab, transform.position + new Vector3(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1)), transform.rotation);
            EnemyBehaviour newEnemy = temp.GetComponent<EnemyBehaviour>();
            newEnemy.waypoints = waypoints;
            newEnemy.currWaypointIndex = currWaypointIndex;
            newEnemy.isElite = isElite;
            currSpawnTime = 0;
            minionsList.Add(temp);
        }
        minionsList.RemoveAll(minion => minion == null);
    }

    public override void Die()
    {
        ResourceSystem.money += value;
        GameObject.Find("Money").GetComponent<Text>().text = "Money:" + ResourceSystem.money;
        Destroy(gameObject);
    }
}
