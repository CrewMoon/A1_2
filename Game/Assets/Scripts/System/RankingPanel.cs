using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
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

    public TMP_Text RankingContent;

    public GameObject StartPanel;
    void Awake()
    {
        ExitButton.onClick.AddListener(OnExitButtonClick);
        WriteToRanking();
    }

    void OnDestroy()
    {
        ExitButton.onClick.RemoveListener(OnExitButtonClick);
    }

    private void OnExitButtonClick()
    {
        StartPanel.SetActive(true);
        this.gameObject.SetActive(false);
    }
    

    private void WriteToRanking()
    {
        RankingContent.text = null;
        List<GameRecord> gameRecords = ScoreSave.LoadByJson();
        gameRecords.Sort((x, y) => y.score.CompareTo(x.score));

        if (gameRecords.Count >= 1)
        {
            for (int i = 0; i < gameRecords.Count; i++)
            {
                RankingContent.text += $"{i + 1}. Date: {gameRecords[i].date}, Level: {gameRecords[i].level}, Score: {gameRecords[i].score}\n";
            }
        }
        else
        {
            RankingContent.text = "no record";
        }
    }
}
