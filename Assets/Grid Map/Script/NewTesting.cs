using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EpPathFinding3D.cs;

public class NewTesting : MonoBehaviour
{
    public List<GridPos> resultPathList;

    GridPos startPos = new GridPos(0, 0, 0);
    GridPos endPos = new GridPos(32, 48, 63);
    JumpPointParam jpParam;
    BaseGrid searchGrid;

    void Start()
    {
        int width = 64;
        int length = 64;
        int height = 64;

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
        jpParam = new JumpPointParam(searchGrid, startPos, endPos, EndNodeUnWalkableTreatment.ALLOW, DiagonalMovement.Always, HeuristicMode.EUCLIDEAN);
        resultPathList = JumpPointFinder.FindPath(jpParam);
    }

    public List<Vector3> GetPath()
    {
        List<Vector3> posList = new List<Vector3>();

        foreach (GridPos pos in resultPathList) {
            posList.Add(new Vector3(pos.x, pos.y, pos.z));
            Debug.Log(pos.ToString());
        }

        return posList;
    }
}
