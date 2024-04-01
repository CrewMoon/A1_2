using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
  
    public GameObject Hero;
    public GameObject NondestructibleObstacles;
    public GameObject WreckableObstacles;
    public GameObject MeleeEnemies;
    public GameObject RangedEnemies;
    public GameObject FinishPanel;

    private int PresentLevel = 1;
    
    // Start is called before the first frame update
    void Start()
    {
        InitScene();
    }

    public void InitScene()
    {
        InitFirstLevel();
    }

    public void InitFirstLevel()
    {
        PresentLevel = 1;
        RefreshBackground();
        Hero.GetComponent<Hero>().RefreshPosition();
        for (int i = 0; i < 10; i++)
        {
            Transform MeleeEnemy = MeleeEnemies.transform.GetChild(i);
            MeleeEnemy.GameObject().SetActive(true);
            MeleeEnemy.GetComponentInChildren<MeleeEnemy>().RefreshPosition();
        }
        // AstarPath.active.Scan();
    }

    public void InitSecondLevel()
    {
        PresentLevel = 2;
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
    }

    private void RefreshBackground()
    {
        for (int i = 0; i < 15; i++)
        {
            Transform NondestructibleObstacle =  NondestructibleObstacles.transform.GetChild(i);
            NondestructibleObstacle.GetComponent<NondestructibleObstacle>().RefreshPosition();
            AstarPath.active.Scan();
        }
        for (int i = 0; i < 15; i++)
        {
            Transform WreckableObstacle =  WreckableObstacles.transform.GetChild(i);
            WreckableObstacle.GetComponent<WreckableObstacle>().RefreshPosition();
            AstarPath.active.Scan();
        }
    }

    public void Fail(int score)
    {
        Time.timeScale = 0f;
        FinishPanel.SetActive(true);
        SaveRecord(score);
        FinishPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Fail";
    }

    public void Success(int score)
    {
        Time.timeScale = 0f;
        FinishPanel.SetActive(true);
        SaveRecord(score);
        FinishPanel.transform.GetChild(1).GetComponent<TMP_Text>().text = "Success";
    }

    private void SaveRecord(int score)
    {
        GameRecord record = new GameRecord(System.DateTime.Now, PresentLevel, score);
        string json = JsonUtility.ToJson(record);
        PlayerPrefs.SetString("GameRecord", json);
        PlayerPrefs.Save();
    }

    public int GetPresentLevel()
    {
        return PresentLevel;
    }
}
