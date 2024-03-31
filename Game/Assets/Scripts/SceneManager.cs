using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    /*
    private 
    */
    
    public GameObject Map;
    public GameObject ObstacleParent;
    public GameObject MeleeEnemyParent;
    public GameObject RangedEnemyStationParent;
    
    public GameObject HeroPrefab;
    public GameObject NondestructibleObstaclePrefab;
    public GameObject WreckableObstaclePrefab;
    public GameObject MeleeEnemyPrefab;
    public GameObject RangedEnemyStationPrefab;
    
    // Start is called before the first frame update
    void Awake()
    {
        InitScene();
    }

    private void InitScene()
    {
        InitFirstScene();
    }

    private void InitFirstScene()
    {
        RefreshBox(HeroPrefab, 1, Map);
        RefreshBox(NondestructibleObstaclePrefab, 15, ObstacleParent);
        RefreshBox(WreckableObstaclePrefab, 15, ObstacleParent);
        RefreshCircle(MeleeEnemyPrefab, 10, MeleeEnemyParent);
        AstarPath.active.Scan();
    }

    private void InitSecondScene()
    {
        RefreshBox(HeroPrefab, 1, Map);
        RefreshBox(NondestructibleObstaclePrefab, 15, ObstacleParent);
        RefreshBox(WreckableObstaclePrefab, 15, ObstacleParent);
        RefreshCircle(MeleeEnemyPrefab, 15, MeleeEnemyParent);
        RefreshBox(RangedEnemyStationPrefab, 5, RangedEnemyStationParent);
        AstarPath.active.Scan();
    }

    private void RefreshBox(GameObject prefab, int num, GameObject parent)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 position = RefreshBoxPosition(prefab, parent);
            GameObject Obstacle = Instantiate(prefab, position + this.transform.position, Quaternion.identity, parent.transform);
        }
    }

    private void RefreshCircle(GameObject prefab, int num, GameObject parent)
    {
        for (int i = 0; i < num; i++)
        {
            Vector3 position = RefreshCirclePosition(prefab, parent);
            GameObject Obstacle = Instantiate(prefab, position + this.transform.position, Quaternion.identity, parent.transform);
        }
        
    }

    public Vector3 RefreshBoxPosition(GameObject refreshObject, GameObject parent)
    {
        float width = refreshObject.GetComponent<RectTransform>().rect.width;
        
        float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - width / 2;
        
        Vector3 position = new Vector3()
        {
            x = Random.Range(-rangeRadius, rangeRadius),
            y = Random.Range(-rangeRadius, rangeRadius),
            z = 0
        };

        if (Physics.OverlapBox(position, Vector3.one * width).Length > 0)
        {
            position = RefreshBoxPosition(refreshObject, parent);
        }

        return position;
    }

    public Vector3 RefreshCirclePosition(GameObject refreshObject, GameObject parent)
    {
        float radius = refreshObject.GetComponent<CircleCollider2D>().radius;
        float rangeRadius = parent.GetComponent<RectTransform>().rect.width / 2 - radius;
        
        Vector3 position = new Vector3()
        {
            x = Random.Range(-rangeRadius, rangeRadius),
            y = Random.Range(-rangeRadius, rangeRadius),
            z = 0
        };

        if (Physics.OverlapSphere(position, radius).Length > 0)
        {
            position = RefreshCirclePosition(refreshObject, parent);
        }

        return position;
    }
}
