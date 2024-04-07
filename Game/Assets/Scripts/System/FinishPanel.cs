using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FinishPanel : MonoBehaviour
{
    public Button MainPageButton;
    public Button RestartButton;

    private void Start()
    {
        MainPageButton.onClick.AddListener(OnMainPageButtonClick);
        RestartButton.onClick.AddListener(OnRestartButtonClick);
    }

    private void OnDestroy()
    {
        MainPageButton.onClick.RemoveListener(OnMainPageButtonClick);
        RestartButton.onClick.RemoveListener(OnRestartButtonClick);
    }

    private void OnMainPageButtonClick()
    {
        SceneLoader.Instance.LoadScene("Start");
    }

    private void OnRestartButtonClick()
    {
        SceneLoader.Instance.LoadScene("Level1");
    }

}
