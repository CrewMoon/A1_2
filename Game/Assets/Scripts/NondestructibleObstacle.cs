using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NondestructibleObstacle : MonoBehaviour
{
    private int[] gridPos;
    public void RefreshPosition()
    {
        if (this.gridPos == null)
        {
            gridPos = GridMap.Instance.OccupyGrid();
            GetComponent<RectTransform>().anchoredPosition = GridMap.Instance.ConvertGridPosToWorldPos(gridPos[0], gridPos[1]);
        }
        else
        {
            GridMap.Instance.ReleaseGrid(gridPos[0], gridPos[1]);
            gridPos = GridMap.Instance.OccupyGrid();
            GetComponent<RectTransform>().anchoredPosition = GridMap.Instance.ConvertGridPosToWorldPos(gridPos[0], gridPos[1]);
        }
    }
}
