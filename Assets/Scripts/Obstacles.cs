using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacles : MonoBehaviour
{
    public static int[,] obstacleList;
    public static int numObstacles;

    void Awake()
    {
        numObstacles = transform.childCount;
        obstacleList = new int[numObstacles, 3];

        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.CompareTag("Obstacle"))
            {
                obstacleList[count, 0] = (int)Mathf.Floor(child.transform.position.x);
                obstacleList[count, 1] = (int)Mathf.Floor(child.transform.position.y);
                obstacleList[count, 2] = (int)Mathf.Floor(child.transform.position.z);
            }
            else
            {
                obstacleList[count, 0] = -1;
                obstacleList[count, 1] = -1;
                obstacleList[count, 2] = -1;
            }
            count++;
        }
    }
}
