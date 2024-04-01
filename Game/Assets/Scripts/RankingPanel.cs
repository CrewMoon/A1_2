using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GameRecord
{
    public DateTime date;
    public int level;
    public int score;

    public GameRecord(DateTime date, int level, int score)
    {
        this.date = date;
        this.level = level;
        this.score = score;
    }
}
public class RankingPanel : MonoBehaviour
{
    public Button ExitButton;

    public Button RestartButton;

    public TMP_Text RankingContent;

    public GameObject StartPanel;
    void Awake()
    {
        ExitButton.onClick.AddListener(OnExitButtonClick);
        RestartButton.onClick.AddListener(OnRestartButtonClick);
        WriteToRanking();
    }

    void OnDestroy()
    {
        ExitButton.onClick.RemoveListener(OnExitButtonClick);
        RestartButton.onClick.RemoveListener(OnRestartButtonClick);
    }

    private void OnExitButtonClick()
    {
        StartPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
    
    private void OnRestartButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public static Dictionary<string, GameRecord> LoadGameRecord()
    {
        Dictionary<string, GameRecord> allPrefs = new Dictionary<string, GameRecord>();
        
        foreach (string key in PlayerPrefs.GetString("__reservedKeyForPlayerPrefs").Split(';'))
        {
            if (key.Length > 0)
            {
                string json = PlayerPrefs.GetString(key);
                GameRecord value = null;
                if (!string.IsNullOrEmpty(json))
                {
                    value = JsonUtility.FromJson<GameRecord>(json);
                }
                else
                {
                    Debug.LogWarning("No game record found.");
                }
                allPrefs.Add(key, value);
            }
        }
        return allPrefs;
    }

    private void WriteToRanking()
    {
        Dictionary<string, GameRecord> history = LoadGameRecord();
        List<KeyValuePair<string, GameRecord>> sortedRecords = history.ToList();
        sortedRecords.Sort((x, y) => y.Value.score.CompareTo(x.Value.score));
        
        int rank = 1;
        foreach (var record in sortedRecords)
        {
            RankingContent.text += $"{rank}. Date: {record.Value.date}, Level: {record.Value.level}, Score: {record.Value.score}\n";
            rank++;
        }
    }
}
