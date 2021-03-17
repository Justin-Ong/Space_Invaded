using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EpPathFinding3D.cs;

public class LevelGenerator : MonoBehaviour
{
    public List<GridPos> resultPathList;
    public int width;
    public int length;
    public int height;
    public GameObject defencePoint;
    public GameObject node;

    JumpPointParam jpParam;
    BaseGrid searchGrid;
    GridPos startPos;
    GridPos endPos;

    void Awake()
    {
        endPos = new GridPos((int)Mathf.Floor(defencePoint.transform.position.x), (int)Mathf.Floor(defencePoint.transform.position.y), (int)Mathf.Floor(defencePoint.transform.position.z));

        CreateGrid();
        CreateNodes();
        References.levelGrid = this;
    }

    void CreateGrid()
    {
        bool[][][] movableMatrix = new bool[width][][];
        for (int widthTrav = 0; widthTrav < width; widthTrav++)
        {
            movableMatrix[widthTrav] = new bool[length][];
            for (int lengthTrav = 0; lengthTrav < length; lengthTrav++)
            {
                movableMatrix[widthTrav][lengthTrav] = new bool[height];
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    movableMatrix[widthTrav][lengthTrav][heightTrav] = true;
                }
            }
        }

        searchGrid = new StaticGrid(width, length, height, movableMatrix);

        for (int i = 0; i < Obstacles.numObstacles; i++)
        {
            Debug.Log(i.ToString() + " " + Obstacles.obstacleList[i, 0].ToString() + " " + Obstacles.obstacleList[i, 1].ToString() + " " + Obstacles.obstacleList[i, 2].ToString());
            if (CheckWithinBounds(Obstacles.obstacleList[i, 0], Obstacles.obstacleList[i, 1], Obstacles.obstacleList[i, 2])) {
                searchGrid.SetWalkableAt(Obstacles.obstacleList[i, 0], Obstacles.obstacleList[i, 1], Obstacles.obstacleList[i, 2], false);
                Debug.Log(searchGrid.IsWalkableAt(Obstacles.obstacleList[i, 0], Obstacles.obstacleList[i, 1], Obstacles.obstacleList[i, 2]));
            }
        }
    }

    bool CheckWithinBounds(int x, int y, int z)
    {
        if (x >= 0 && x < width && y >= 0 && y < length && z >= 0 && z < height)
        {
            return true;
        }
        return false;
    }

    void CreateNodes()
    {
        GameObject[] objCreated = new GameObject[width * length * height];
        int count = 0;
        for (int widthTrav = 0; widthTrav < width; widthTrav++)
        {
            for (int lengthTrav = 0; lengthTrav < length; lengthTrav++)
            {
                for (int heightTrav = 0; heightTrav < height; heightTrav++)
                {
                    objCreated[count] = Instantiate(node, new Vector3(widthTrav, lengthTrav, heightTrav), new Quaternion());
                    count++;
                }
            }
        }
        StaticBatchingUtility.Combine(objCreated, gameObject);
    }

    public List<Vector3> GetPath(Vector3 startLocation)
    {
        startPos = new GridPos((int)Mathf.Floor(startLocation.x), (int)Mathf.Floor(startLocation.y), (int)Mathf.Floor(startLocation.z));
        if (jpParam != null)
        {
            jpParam.Reset(startPos, endPos);
        }
        else
        {
            jpParam = new JumpPointParam(searchGrid, startPos, endPos, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Always, HeuristicMode.EUCLIDEAN);
        }
        resultPathList = JumpPointFinder.FindPath(jpParam);

        List<Vector3> posList = new List<Vector3>();
        foreach (GridPos pos in resultPathList)
        {
            posList.Add(new Vector3(pos.x, pos.y, pos.z));
        }

        return posList;
    }
}
