using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Michsky.UI.ModernUIPack.ProgressBar progressBar;
    public TextMeshProUGUI progressText;

    public void LoadLevel (int sceneIndex)
    {
        progressBar.currentPercent = 0;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            progressBar.currentPercent = progress;
            progressText.text = (progress*100).ToString("F1");
            yield return null;
        }
    }
}
