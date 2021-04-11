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
    public static bool TriggerBuildMode;
    public static bool waveOver;

    private float randomSpawnTimer;
    private float spawnTimer;
    private float tracerTimer;
    private int numWaves;
    private int currWave;
    private int numMiniWaves;
    private int miniWaveIndex;
    private int currEnemyInWave;
    private List<Vector3> waypoints = new List<Vector3>();

    public static EnemySpawnerBehaviour self;

    void Start()
    {
        GetWaypoints(transform.position);
        currWave = 0;
        currEnemyInWave = 1;
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
                }
            }
            else
            {
                TriggerBuildMode = false;
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
                        waveOver = true;
                        currWave++;
                        miniWaveIndex = 0;
                        if (currWave < numWaves)
                        {
                            numMiniWaves = waves[currWave].numEnemiesToSpawn.Count;
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
            newEnemyBehaviour.damage *= 1.5f;
            newEnemyBehaviour.maxHealth *= 1.5f;
            newEnemy.GetComponent<HealthSystem>().maxHealth *= 1.5f;
            newEnemy.GetComponent<HealthSystem>().currHealth = newEnemy.GetComponent<HealthSystem>().maxHealth;
            Material[] mats = newEnemy.transform.Find("Model").GetComponent<Renderer>().materials;
            foreach (Material mat in mats)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", Color.red);
            }
        }
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
