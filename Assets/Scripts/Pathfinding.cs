using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    //public GameObject spawner;
    //public GameObject defencePoint;

    private Grid grid;
    private List<Vector3> waypoints = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        grid = References.grid;
    }

    Vector3 GetVector3ByCellPosition(Vector3 worldPosition)
    {
        grid.GetXYZ(worldPosition, out int x, out int y, out int z);
        return new Vector3(x, y, z);
    }

    int GetDistance(Vector3 nodeA, Vector3 nodeB)
    {
        int dstX = (int)Mathf.Abs(nodeA.x - nodeB.x);
        int dstY = (int)Mathf.Abs(nodeA.y - nodeB.y);
        int dstZ = (int)Mathf.Abs(nodeA.z - nodeB.z);

        if (dstX == dstY && dstY == dstZ)
        {
            return dstX * 17;
        }
        else
        {
            int largestVal = Mathf.Max(Mathf.Max(dstX, dstY), dstZ);
            int smallestVal = Mathf.Min(Mathf.Min(dstX, dstY), dstZ);
            int middleVal = dstX + dstY + dstZ - largestVal - smallestVal;

            return 17 * smallestVal + 14 * (middleVal - smallestVal) + 10 * (largestVal - middleVal);
        }
    }

    List<Vector3> RetracePath(Vector3 startNode, Vector3 targetNode)
    {
        Vector3 currentNode = targetNode;
        float cellSize = grid.GetCellSize();

        while (currentNode != startNode)
        {
            Vector3 coords = new Vector3(currentNode.x * cellSize, currentNode.y * cellSize, currentNode.z * cellSize);
            waypoints.Add(coords);
            currentNode = grid.GetParent(currentNode);
        }
        waypoints.Reverse();

        foreach (Vector3 point in waypoints)
        {
            Debug.Log(point);
        }

        return waypoints;
    }

    public List<Vector3> FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        Vector3 startNode = GetVector3ByCellPosition(startPosition);
        Vector3 targetNode = GetVector3ByCellPosition(endPosition);

        List<Vector3> openSet = new List<Vector3>();
        List<Vector3> path = new List<Vector3>();
        HashSet<Vector3> closedSet = new HashSet<Vector3>();
        openSet.Add(startNode);
        waypoints.Add(endPosition);

        while (openSet.Count > 0)
        {
            Vector3 currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (grid.GetFCost(openSet[i]) < grid.GetFCost(currentNode) || grid.GetFCost(openSet[i]) == grid.GetFCost(currentNode) && grid.GetHCost(openSet[i]) < grid.GetHCost(currentNode))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                path = RetracePath(startNode, targetNode);
                break;
            }

            for (int neighbourX = -1; neighbourX < 2; neighbourX++)
            {
                for (int neighbourY = -1; neighbourY < 2; neighbourY++)
                {
                    for (int neighbourZ = -1; neighbourZ < 2; neighbourZ++)
                    {
                        if (neighbourX == 0 && neighbourY == 0 && neighbourZ == 0) continue;

                        Vector3 neighbour;
                        int tempX = (int)currentNode.x + neighbourX;
                        int tempY = (int)currentNode.y + neighbourY;
                        int tempZ = (int)currentNode.z + neighbourZ;
                        neighbour.x = tempX;
                        neighbour.y = tempY;
                        neighbour.z = tempZ;
                        if (tempX < 0 || tempX > grid.GetLength() || tempY < 0 || tempY > grid.GetWidth() || tempZ < 0 || tempZ > grid.GetHeight())
                        {
                            continue;
                        }
                        if (closedSet.Contains(neighbour)) {
                            continue;
                        }

                        int newMovementCostToNeighbour = grid.GetGCost(currentNode) + GetDistance(currentNode, neighbour);
                        if (newMovementCostToNeighbour < grid.GetGCost(neighbour) || !openSet.Contains(neighbour))
                        {
                            grid.SetGCost(neighbour, newMovementCostToNeighbour);
                            grid.SetHCost(neighbour, GetDistance(neighbour, targetNode));
                            grid.SetParent(neighbour, currentNode);

                            if (!openSet.Contains(neighbour))
                                openSet.Add(neighbour);
                        }
                    }
                }
            }
        }
        return path;
    }
}
