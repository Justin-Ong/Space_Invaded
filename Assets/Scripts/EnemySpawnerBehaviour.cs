using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerBehaviour : MonoBehaviour
{
    public GameObject defencePoint;
    public float spawnTimer;

    private float currTimer;
    private List<Vector3> waypoints = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        waypoints = References.levelGrid.GetPath(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        currTimer += Time.deltaTime;
        if (currTimer > spawnTimer)
        {
            GameObject newEnemy = Instantiate(References.enemyTypes[Random.Range(0, References.numEnemyTypes)], transform.position, transform.rotation);
            EnemyBehaviour newEnemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
            HealthSystem newEnemyHealthSystem = newEnemy.GetComponent<HealthSystem>();
            newEnemyBehaviour.waypoints = waypoints;
            currTimer = 0;
        }
    }
}
