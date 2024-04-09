using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : PersistentSingleton<SceneLoader>
{
    [SerializeField] Image LoadImage;
    public float fadetime = 2.5f;
    public int score;
    Color color;

    public void LoadScene(string sceneName)
    {
        StopAllCoroutines();
        StartCoroutine(LoadCoroutine(sceneName));
    }

    IEnumerator LoadCoroutine(string sceneName)
    {
        var loadingCompleted = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        loadingCompleted.allowSceneActivation = false;
        LoadImage.gameObject.SetActive(true);
        while (color.a < 1)
        {
            color.a = Mathf.Clamp01(color.a + Time.deltaTime / fadetime);
            LoadImage.color = color;
            yield return null;
        }
        loadingCompleted.allowSceneActivation = true;

        while (color.a > 0)
        {
            color.a = Mathf.Clamp01(color.a - Time.deltaTime / fadetime);
            LoadImage.color = color;
            yield return null;
        }
        LoadImage.gameObject.SetActive(false);
    }

}