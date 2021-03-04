using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Grid
{
    private int length;
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private int[,,] gridArray;
    private Vector3[,,] parentArray;
    private Vector3Int[,,] costArray;
    private TextMesh[,,] debugTextArray;

    public Grid(int length, int width, int height, float cellSize, Vector3 originPosition)
    {
        this.length = length;
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new int[length, width, height];
        debugTextArray = new TextMesh[length, width, height];
        parentArray = new Vector3[length, width, height];
        costArray = new Vector3Int[length, width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for (int z = 0; z < gridArray.GetLength(2); z++)
                {
                    debugTextArray[x, y, z] = CreateWorldText(null, gridArray[x, y, z].ToString(), GetWorldPosition(x, y, z) + new Vector3(cellSize, cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.white, 100f);
                    parentArray[x, y, z] = new Vector3(0, 0, 0);
                    costArray[x, y, z] = new Vector3Int(0, 0, 0);
                }
            }
        }

        for (int x = 0; x <= length; x++)
        {
            Debug.DrawLine(GetWorldPosition(x, width, 0), GetWorldPosition(x, width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(x, 0, height), GetWorldPosition(x, width, height), Color.white, 100f);
        }
        for (int y = 0; y <= width; y++)
        {
            Debug.DrawLine(GetWorldPosition(length, y, 0), GetWorldPosition(length, y, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(0, y, height), GetWorldPosition(length, y, height), Color.white, 100f);
        }
        for (int z = 0; z <= height; z++)
        {
            Debug.DrawLine(GetWorldPosition(0, width, z), GetWorldPosition(length, width, z), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(length, 0, z), GetWorldPosition(length, width, z), Color.white, 100f);
        }

    }

    private Vector3 GetWorldPosition(int x, int y, int z)
    {
        return new Vector3(x, y, z) * cellSize + originPosition;
    }

    public void SetValue(int x, int y, int z, int value)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < length && y < width && z < height)
        {
            gridArray[x, y, z] = value;
            debugTextArray[x, y, z].text = gridArray[x, y, z].ToString();
        }
    }

    public void SetValue(Vector3 worldPosition, int value)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        SetValue(x, y, z, value);
    }

    public int GetValue(int x, int y, int z)
    {
        if (x >= 0 && y >= 0 && z >= 0 && x < length && y < width && z < height)
        {
            return gridArray[x, y, z];
        }
        else
        {
            return 0;
        }
    }

    public int GetValue(Vector3 worldPosition)
    {
        int x, y, z;
        GetXYZ(worldPosition, out x, out y, out z);
        return GetValue(x, y, z);
    }
    public void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
        z = Mathf.FloorToInt((worldPosition - originPosition).z / cellSize);
    }

    public TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment)
    {
        GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
        Transform transform = gameObject.transform;
        transform.SetParent(parent, false);
        transform.localPosition = localPosition;
        TextMesh textMesh = gameObject.GetComponent<TextMesh>();
        textMesh.anchor = textAnchor;
        textMesh.alignment = textAlignment;
        textMesh.text = text;
        textMesh.fontSize = fontSize;
        textMesh.color = color;
        return textMesh;
    }

    public void SetParent(Vector3 cell, Vector3 parent)
    {
        parentArray[(int)cell.x, (int)cell.y, (int)cell.z] = parent;
    }

    public Vector3 GetParent(Vector3 cell)
    {
        return parentArray[(int)cell.x, (int)cell.y, (int)cell.z];
    }

    public int GetLength() {
        return length;
    }
    
    public int GetHeight() {
        return height;
    }
    
    public int GetWidth() {
        return width;
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    public int GetFCost(Vector3 cell)
    {
        return GetGCost(cell) + GetHCost(cell);
    }
    
    public int GetGCost(Vector3 cell)
    {
        return costArray[(int)cell.x, (int)cell.y, (int)cell.z].y;
    }
    
    public int GetHCost(Vector3 cell)
    {
        return costArray[(int)cell.x, (int)cell.y, (int)cell.z].z;
    }

    public void SetGCost(Vector3 cell, int value)
    {
        costArray[(int)cell.x, (int)cell.y, (int)cell.z].y = value;
    }
    public void SetHCost(Vector3 cell, int value)
    {
        costArray[(int)cell.x, (int)cell.y, (int)cell.z].z = value;
    }
}
