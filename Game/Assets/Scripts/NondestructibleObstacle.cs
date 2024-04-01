using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class NondestructibleObstacle : MonoBehaviour
{   
    public void RefreshPosition()
    {
        GameObject parent = GameObject.Find("Map");
        float offset = 20f;
            
        float width = this.GetComponent<RectTransform>().rect.width;
        float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - width / 2 - offset;
        Vector2 position = new Vector2();
        bool walkable = false;
    
        
        while (!walkable)
        {
            position.x = Random.Range(-rangeRadius, rangeRadius);
            position.y = Random.Range(-rangeRadius, rangeRadius);
            this.GetComponent<RectTransform>().anchoredPosition = position;
            position = this.transform.position;
        
            Vector2 leftUp = new Vector2(position.x - width / 2, position.y + width / 2);
            Vector2 rightUp = new Vector2(position.x + width / 2, position.y + width / 2);
            Vector2 leftDown = new Vector2(position.x - width / 2, position.y - width / 2);
            Vector2 rightDown = new Vector2(position.x + width / 2, position.y - width / 2);
            if (AstarPath.active.GetNearest(leftUp).node.Walkable &&
                AstarPath.active.GetNearest(rightUp).node.Walkable &&
                AstarPath.active.GetNearest(leftDown).node.Walkable &&
                AstarPath.active.GetNearest(rightDown).node.Walkable)
            {
                walkable = true;
            }
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
}
