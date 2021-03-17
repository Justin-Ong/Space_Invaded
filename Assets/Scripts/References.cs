using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class References : MonoBehaviour
{
    // Player stuff
    public static GameObject player;

    // Defence Point stuff
    public static GameObject defencePointObject;

    // Pathfinding Stuff
    public static LevelGenerator levelGrid;

    // Turret stuff
    public static string[] turretTypes = new string[] { "basic", "fast", "slow" };
    public static int numTurrets = turretTypes.Length - 1;

    // Enemy stuff
    public static List<GameObject> enemyTypes = new List<GameObject>();
    public static int numEnemyTypes;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;

    // UI
    public static GameObject canvas;

    private void Awake()
    {
        enemyTypes.Add(enemy1);
        enemyTypes.Add(enemy2);
        enemyTypes.Add(enemy3);
        enemyTypes.Add(enemy4);

        numEnemyTypes = enemyTypes.Count;
    }
}
