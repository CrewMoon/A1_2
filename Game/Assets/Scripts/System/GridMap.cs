using System;
using Pathfinding;
using UnityEngine;
using Random = UnityEngine.Random;

public class GridMap : Singleton<GridMap>
{
    public int gridSizeX = 21;
    public int gridSizeY = 21;
    public float cellSize = 20f;

    private bool[,] gridOccupied;

    public void InitializeGrid()
    {
        gridOccupied = new bool[gridSizeX, gridSizeY];
    }

    public int[] OccupyGrid()
    {
        if (gridOccupied == null)
        {
            InitializeGrid();
        }
        int GridX = Random.Range(0, 20);
        int GridY = Random.Range(0, 20);
        while (gridOccupied[GridX, GridY])
        {
            GridX = Random.Range(0, 20);
            GridY = Random.Range(0, 20);
        }

        gridOccupied[GridX, GridY] = true;

        int[] GridPos = new[] { GridX, GridY };
        return GridPos;
    }

    public Vector2 ConvertGridPosToWorldPos(int x, int y)
    {
        Vector2 position = new Vector2(-210 + x * cellSize + (cellSize / 2), -210 + y * cellSize + (cellSize / 2));
        return position;
    }

    public void ReleaseGrid(int x, int y)
    {
        gridOccupied[x, y] = false;
    }
}