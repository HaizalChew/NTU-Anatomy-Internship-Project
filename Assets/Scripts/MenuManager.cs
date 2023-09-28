using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private RectTransform progressBar;

    public void ToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void ToOtherScenes(int buildIndex)
    {
        StartCoroutine(LoadingScreen(buildIndex));
    }

    IEnumerator LoadingScreen(int buildIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(buildIndex);

        fadeCanvas.SetActive(true);
        float initWidth = progressBar.sizeDelta.x;

        while (!operation.isDone)
        {
            progressBar.sizeDelta = new Vector3(operation.progress * initWidth, progressBar.sizeDelta.y);

            yield return null;
        }
    }

}
