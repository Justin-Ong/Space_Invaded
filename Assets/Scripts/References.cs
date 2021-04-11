using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour
{
    // Player stuff
    public static GameObject player;

    // Defence Point stuff
    public static DefencePointBehaviour defencePointObject;

    // Pathfinding Stuff
    public static LevelGenerator levelGrid;

    // Enemy stuff
    public static List<GameObject> enemyTypes = new List<GameObject>();
    public static int numEnemyTypes;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;
    public GameObject enemy6;

    // UI
    public static GameObject canvas;

    private void Awake()
    {
        enemyTypes.Add(enemy1);
        enemyTypes.Add(enemy2);
        enemyTypes.Add(enemy3);
        enemyTypes.Add(enemy4);
        enemyTypes.Add(enemy5);
        enemyTypes.Add(enemy6);

        numEnemyTypes = enemyTypes.Count;
    }
}
