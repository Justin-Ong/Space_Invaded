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

        for (int x = 0; x < gridArray.GetLength(0); x++)
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                for(int z = 0; z < gridArray.GetLength(2); z++)
                {
                    debugTextArray[x, y, z] = CreateWorldText(null, gridArray[x, y, z].ToString(), GetWorldPosition(x, y, z) + new Vector3(cellSize, cellSize, cellSize) * .5f, 20, Color.white, TextAnchor.MiddleCenter, TextAlignment.Center);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y + 1, z), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x + 1, y, z), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y, z), GetWorldPosition(x, y, z + 1), Color.white, 100f);
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
        if (x >= 0 && y >= 0 && z >=0 && x < length && y < width && z < height)
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
}