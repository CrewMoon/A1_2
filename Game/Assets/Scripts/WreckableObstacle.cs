using Pathfinding;
using UnityEngine;

namespace DefaultNamespace
{
    public class WreckableObstacle : MonoBehaviour
    {
        private const int HEALTH = 3;
        private int health;

        private void Awake()
        {
            health = HEALTH;
        }
        
        public void RefreshPosition()
        {
            GameObject parent = GameObject.Find("Map");
            float offset = 20f;
        
            float width = this.GetComponent<RectTransform>().rect.width;
            float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - width / 2 - offset;
        
            Vector2 position = new Vector2()
            {
                x = Random.Range(-rangeRadius, rangeRadius),
                y = Random.Range(-rangeRadius, rangeRadius)
            };
        
            bool hasObstacle = CheckForObstacles(position, width / 2f);
            if (!hasObstacle)
            {
                this.GetComponent<RectTransform>().anchoredPosition  = position;
            }
            else
            {
                RefreshPosition();
            }
        }
        
        private bool CheckForObstacles(Vector3 position, float radius)
        {
            NNInfo nearestNodeInfo = AstarPath.active.GetNearest(position);
            LayerMask obstacleMask = LayerMask.GetMask("Obstacles");
            
            if (nearestNodeInfo.node != null)
            {

                Vector3 nearestNodePosition = nearestNodeInfo.clampedPosition;
                
                Collider2D[] colliders = Physics2D.OverlapCircleAll(nearestNodePosition, radius, obstacleMask);
                
                return colliders.Length > 0;
            }

            return false;
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