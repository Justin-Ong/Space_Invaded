using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaveObject
{
    public List<int> enemyType;
    public List<int> numEnemiesToSpawn;
    public List<float> timeBetweenSpawns;
    public List<bool> isElite;

    public WaveObject(List<int> enemy, List<int> numEnemies, List<float> spawntime, List<bool> elite)
    {
        enemyType = enemy;
        numEnemiesToSpawn = numEnemies;
        timeBetweenSpawns = spawntime;
        isElite = elite;
    }
}

public class EnemySpawnerBehaviour : MonoBehaviour
{
    public GameObject defencePoint;
    public List<WaveObject> waves;
    public GameObject TracerPrefab;
    public bool TriggerBuildMode;
    public bool waveOver;
    public List<GameObject> bonusEnemies;
    public List<int> bonusEnemySpawnWaves;

    private float randomSpawnTimer;
    private float spawnTimer;
    private float tracerTimer;
    private int numWaves;
    private int currWave;
    private int numMiniWaves;
    private int miniWaveIndex;
    private int currEnemyInWave;
    private List<Vector3> waypoints = new List<Vector3>();
    private int numBonusEnemiesSpawned;

    public static EnemySpawnerBehaviour self;

    void Start()
    {
        GetWaypoints(transform.position);
        currWave = 0;
        currEnemyInWave = 0;
        miniWaveIndex = 0;
        numWaves = waves.Count;
        numMiniWaves = 0;
        randomSpawnTimer = 2;
        waveOver = false;
        if (numWaves > 0)
        {
            numMiniWaves = waves[currWave].numEnemiesToSpawn.Count;
        }
        TriggerBuildMode = true;
        numBonusEnemiesSpawned = 0;
        tracerTimer = 999;
    }

    void Update()
    {
        if (!BuildManager.buildModeFlag)
        {
            spawnTimer += Time.deltaTime;
            if (waveOver)
            {
                if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
                {
                    TriggerBuildMode = true;
                    if (currWave == numWaves) {
                        BuildManager.victoryFlag = true;
                    }
                }
            }
            else
            {
                TriggerBuildMode = false;
                tracerTimer = 999;
                if (numBonusEnemiesSpawned < bonusEnemySpawnWaves.Count && currWave == bonusEnemySpawnWaves[numBonusEnemiesSpawned])
                {
                    bonusEnemies[numBonusEnemiesSpawned].SetActive(true);
                    numBonusEnemiesSpawned++;
                }
                if (numWaves > 0)
                {
                    if (currWave < numWaves && spawnTimer > waves[currWave].timeBetweenSpawns[miniWaveIndex])
                    {
                        if (currEnemyInWave < waves[currWave].numEnemiesToSpawn[miniWaveIndex])
                        {
                            SpawnEnemy();
                        }
                        currEnemyInWave++;
                        spawnTimer = 0;
                    }
                    if (currWave < numWaves && currEnemyInWave > waves[currWave].numEnemiesToSpawn[miniWaveIndex])
                    {
                        miniWaveIndex++;
                        currEnemyInWave = 0;
                    }
                    if (currWave < numWaves && miniWaveIndex >= numMiniWaves)
                    {
                        waveOver = true;
                        currWave++;
                        miniWaveIndex = 0;
                        if (currWave < numWaves)
                        {
                            numMiniWaves = waves[currWave].numEnemiesToSpawn.Count;
                        }
                    }
                    if (currWave >= numWaves && spawnTimer > randomSpawnTimer)
                    {
                        SpawnRandomEnemy();
                        spawnTimer = 0;
                        if (randomSpawnTimer > 0.1)
                        {
                            randomSpawnTimer -= 0.01f;
                        }
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
        }
        else
        {
            tracerTimer += Time.deltaTime;
            if (tracerTimer > 3)
            {
                SpawnTracer();
                tracerTimer = 0;
            }

        }
    }

    public void GetWaypoints(Vector3 pos)
    {
        waypoints = References.levelGrid.GetPath(pos);
    }

    void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(References.enemyTypes[waves[currWave].enemyType[miniWaveIndex]], transform.position, transform.rotation);
        EnemyBehaviour newEnemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        newEnemyBehaviour.waypoints = waypoints;
        if (waves[currWave].isElite[miniWaveIndex])
        {
            newEnemyBehaviour.isElite = true;
        }
    }
    
    void SpawnRandomEnemy()
    {
        GameObject newEnemy = Instantiate(References.enemyTypes[Random.Range(0, References.numEnemyTypes)], transform.position, transform.rotation);
        EnemyBehaviour newEnemyBehaviour = newEnemy.GetComponent<EnemyBehaviour>();
        if (Random.Range(0, 2) == 0)
        {
            newEnemyBehaviour.isElite = true;
        }
        newEnemyBehaviour.waypoints = waypoints;
    }

    void SpawnTracer()
    {
        GameObject newTracer = Instantiate(TracerPrefab, transform.position, transform.rotation);
        TracerBehaviour newTracerBehaviour = newTracer.GetComponent<TracerBehaviour>();
        newTracerBehaviour.waypoints = waypoints;
    }
}
