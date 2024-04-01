using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    public Button StartButton;
    public Button RankingButton;
    public Button ExitButton;
    public Button AboutButton;

    public GameObject StartPanel;
    public GameObject RankingPanel;
    public GameObject AboutPanel;
    void Start()
    {
        StartButton.onClick.AddListener(OnStartButtonClick);
        RankingButton.onClick.AddListener(OnRankingButtonClick);
        ExitButton.onClick.AddListener(OnExitButtonClick);
        AboutButton.onClick.AddListener(OnAboutButtonClick);
    }

    private void OnDestroy()
    {
        StartButton.onClick.RemoveListener(OnStartButtonClick);
        RankingButton.onClick.RemoveListener(OnRankingButtonClick);
        ExitButton.onClick.RemoveListener(OnExitButtonClick);
        AboutButton.onClick.RemoveListener(OnAboutButtonClick);
    }

    private void OnStartButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    private void OnRankingButtonClick()
    {
        StartPanel.SetActive(false);
        RankingPanel.SetActive(true);
    }

    private void OnExitButtonClick()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    private void OnAboutButtonClick()
    {
        StartPanel.SetActive(false);
        AboutPanel.SetActive(true);
    }
}
