using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveObject
{
    public List<int> enemyType;
    public List<int> numEnemiesToSpawn;
    public List<float> timeBetweenSpawns;
    public bool immediateStart;

    public WaveObject(List<int> enemy, List<int> numEnemies, List<float> spawntime, bool start)
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
    public float timeToFirstSpawn;
    public GameObject TracerPrefab;

    private float firstSpawnTimer;
    private float randomSpawnTimer;
    private float spawnTimer;
    private float tracerTimer;
    private int numWaves;
    private int currWave;
    private int numMiniWaves;
    private int miniWaveIndex;
    private int currEnemyInWave;
    private List<Vector3> waypoints = new List<Vector3>();

    void Start()
    {
        GetWaypoints();
        currWave = 0;
        currEnemyInWave = 1;
        miniWaveIndex = 0;
        numWaves = waves.Count;
        numMiniWaves = 0;
        randomSpawnTimer = 2;
        firstSpawnTimer = 0;
        if (numWaves > 0)
        {
            numMiniWaves = waves[currWave].numEnemiesToSpawn.Count;
        }
    }

    void Update()
    {
        firstSpawnTimer += Time.deltaTime;
        tracerTimer += Time.deltaTime;
        if (firstSpawnTimer > timeToFirstSpawn)
        {
            spawnTimer += Time.deltaTime;
            if (numWaves > 0)
            {
                if (currWave < numWaves && spawnTimer > waves[currWave].timeBetweenSpawns[miniWaveIndex])
                {
                    SpawnEnemy();
                    currEnemyInWave++;
                    spawnTimer = 0;
                }
                if (currWave < numWaves && currEnemyInWave > waves[currWave].numEnemiesToSpawn[miniWaveIndex])
                {
                    miniWaveIndex++;
                    currEnemyInWave = 1;
                }
                if (currWave < numWaves && miniWaveIndex >= numMiniWaves)
                {
                    currWave++;
                    miniWaveIndex = 0;
                    if (currWave < numWaves)
                    {
                        numMiniWaves = waves[currWave].numEnemiesToSpawn.Count;
                        if (waves[currWave].immediateStart)
                        {
                            spawnTimer = waves[currWave].timeBetweenSpawns[miniWaveIndex] + 1;
                        }
                    }
                }
                if (currWave >= numWaves && spawnTimer > 1)
                {
                    SpawnRandomEnemy();
                    spawnTimer = 0;
                }
            }
            else if (spawnTimer > randomSpawnTimer)
            {
                SpawnRandomEnemy();
                spawnTimer = 0;
                if (randomSpawnTimer > 0.1)
                {
                    randomSpawnTimer -= 0.01f;
                }
            }
        }
        if (tracerTimer > 3)
        {
            SpawnTracer();
            tracerTimer = 0;
        }
    }

    void GetWaypoints()
    {
        waypoints = References.levelGrid.GetPath(transform.position);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(References.enemyTypes[waves[currWave].enemyType[miniWaveIndex]], transform.position, transform.rotation);
        EnemyBehaviour newEnemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        newEnemyBehaviour.waypoints = waypoints;
    }
    
    void SpawnRandomEnemy()
    {
        GameObject newEnemy = Instantiate(References.enemyTypes[Random.Range(0, References.numEnemyTypes)], transform.position, transform.rotation);
        EnemyBehaviour newEnemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        newEnemyBehaviour.waypoints = waypoints;
    }

    void SpawnTracer()
    {
        GameObject newTracer = Instantiate(TracerPrefab, transform.position, transform.rotation);
        TracerBehaviour newTracerBehaviour = newTracer.GetComponent<TracerBehaviour>();
        newTracerBehaviour.waypoints = waypoints;
    }
}
