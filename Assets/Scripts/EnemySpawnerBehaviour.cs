using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveObject
{
    public int enemyType;
    public int numEnemiesToSpawn;
    public float timeBetweenSpawns;
    public bool immediateStart;

    public WaveObject(int enemy, int numEnemies, float spawntime, bool start)
    {
        enemyType = enemy;
        numEnemiesToSpawn = numEnemies;
        timeBetweenSpawns = spawntime;
        immediateStart = start;
    }
}

public class EnemySpawnerBehaviour : MonoBehaviour
{
    public GameObject defencePoint;
    public List<WaveObject> waves;

    private float spawnTimer;
    private int numWaves;
    private int currWave;
    private int currEnemyInWave;
    private List<Vector3> waypoints = new List<Vector3>();

    void Start()
    {
        GetWaypoints();
        currWave = 0;
        currEnemyInWave = 1;
        numWaves = waves.Count;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (currWave < numWaves && spawnTimer > waves[currWave].timeBetweenSpawns)
        {
            SpawnEnemy();
            currEnemyInWave++;
            spawnTimer = 0;
        }
        if (currWave < numWaves && currEnemyInWave > waves[currWave].numEnemiesToSpawn)
        {
            currWave++;
            currEnemyInWave = 1;
            if (currWave < numWaves && waves[currWave].immediateStart)
            {
                spawnTimer = 999;
            }
        }
    }

    void GetWaypoints()
    {
        waypoints = References.levelGrid.GetPath(transform.position);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(References.enemyTypes[waves[currWave].enemyType], transform.position, transform.rotation);
        EnemyBehaviour newEnemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        newEnemyBehaviour.waypoints = waypoints;
    }
}
