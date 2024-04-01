using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
  
    public GameObject Hero;
    public GameObject NondestructibleObstacles;
    public GameObject WreckableObstacles;
    public GameObject MeleeEnemies;
    public GameObject RangedEnemies;
    
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
        RefreshBackground();
        Hero.GetComponent<Hero>().RefreshPosition();
        for (int i = 0; i < 10; i++)
        {
            Transform MeleeEnemy = MeleeEnemies.transform.GetChild(i);
            MeleeEnemy.GameObject().SetActive(true);
            MeleeEnemy.GetComponentInChildren<MeleeEnemy>().RefreshPosition();
        }
        AstarPath.active.Scan();
    }

    private void InitSecondScene()
    {
        RefreshBackground();
        Hero.GetComponent<Hero>().RefreshPosition();
        for (int i = 0; i < 15; i++)
        {
            Transform MeleeEnemy = MeleeEnemies.transform.GetChild(i);
            MeleeEnemy.GameObject().SetActive(true);
            MeleeEnemy.GetComponentInChildren<MeleeEnemy>().RefreshPosition();
        }

        for (int i = 0; i < 5; i++)
        {
            Transform RangedEnemy = RangedEnemies.transform.GetChild(i);
            RangedEnemy.GameObject().SetActive(true);
            RangedEnemy.GetComponent<RangedEnemy>().RefreshPosition();
        }
        AstarPath.active.Scan();
    }

    private void RefreshBackground()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform NondestructibleObstacle =  NondestructibleObstacles.transform.GetChild(i);
            NondestructibleObstacle.GetComponent<NondestructibleObstacle>().RefreshPosition();
        }

        for (int i = 0; i < 15; i++)
        {
            Transform WreckableObstacle =  WreckableObstacles.transform.GetChild(i);
            WreckableObstacle.GetComponent<WreckableObstacle>().RefreshPosition();
        }
    }
    
}
