using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    Grid grid;
    int value;
    private void Start()
    {
        grid = new Grid(10, 10, 10, 10f, new Vector3(20, 0));
        Debug.Log("succeed");
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("press");
            grid.SetValue(Camera.main.ScreenToWorldPoint(Input.mousePosition), 10);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetValue(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
        }
    }
}