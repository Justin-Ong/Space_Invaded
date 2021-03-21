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

    private float firstSpawnTimer;
    private float randomSpawnTimer;
    private float spawnTimer;
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
        if (firstSpawnTimer > timeToFirstSpawn)
        {
            spawnTimer += Time.deltaTime;
            if (numWaves > 0)
            {
                if (currWave < numWaves && spawnTimer > waves[currWave].timeBetweenSpawns[miniWaveIndex])
                {
                    Debug.Log("Normal spawn");
                    SpawnEnemy();
                    currEnemyInWave++;
                    spawnTimer = 0;
                }
                if (currWave < numWaves && currEnemyInWave > waves[currWave].numEnemiesToSpawn[miniWaveIndex])
                {
                    Debug.Log("Increment miniWaveIndex");
                    miniWaveIndex++;
                    currEnemyInWave = 1;
                }
                if (currWave < numWaves && miniWaveIndex >= numMiniWaves)
                {
                    Debug.Log("New wave");
                    currWave++;
                    miniWaveIndex = 0;
                    if (currWave < numWaves)
                    {
                        numMiniWaves = waves[currWave].numEnemiesToSpawn.Count;
                        Debug.Log("Meow");
                        if (waves[currWave].immediateStart)
                        {
                            Debug.Log("Immediate start");
                            spawnTimer = waves[currWave].timeBetweenSpawns[miniWaveIndex] + 1;
                            Debug.Log("A");
                        }
                    }
                }
                if (currWave >= numWaves && spawnTimer > 1)
                {
                    Debug.Log("No more waves");
                    SpawnRandomEnemy();
                    spawnTimer = 0;
                }
            }
            else if (spawnTimer > randomSpawnTimer)
            {
                Debug.Log("No wave set");
                SpawnRandomEnemy();
                spawnTimer = 0;
                randomSpawnTimer -= 0.01f;
            }
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
}
