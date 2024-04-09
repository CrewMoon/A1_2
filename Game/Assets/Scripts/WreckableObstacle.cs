using Pathfinding;
using UnityEngine;

namespace DefaultNamespace
{
    public class WreckableObstacle : MonoBehaviour
    {
        private const int HEALTH = 3;
        private int health;
        private int[] gridPos;

        private void Awake()
        {
            health = HEALTH;
        }

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
        
        public int getHealth()
        {
            return health;
        }

        public void setHealth(int health)
        {
            this.health = health;
        }
    }
}