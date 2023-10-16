using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private RectTransform progressBar;
    [SerializeField] private GameObject[] completedCheckIndex;

    public GameObject[] achivements;
    public Transform rotationCenter;
    public float radius;

    private void Start()
    {
        SpawnAround();
    }

    private void Update()
    {
        for (int i = 0; i < completedCheckIndex.Length; i++)
        {
            if (completedCheckIndex[i] != null)
            {
                completedCheckIndex[i].SetActive(SaveStateManager.instance.topicChecklistCompleted[i]);
            }
            
        }

 

    }

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

    void SpawnAround()
    {
        for (int i = 0; i < achivements.Length; i++)
        {
            float segment = 2 * Mathf.PI * i / achivements.Length;
            float horiValue = Mathf.Cos(segment);
            float vertValue = Mathf.Sin(segment);
            Vector2 dirValue = new Vector2(horiValue, vertValue);
            Vector3 worldPos = (Vector2)rotationCenter.transform.position + dirValue * radius;

            achivements[i].transform.position = worldPos;

            GameObject c = Instantiate(achivements[i], worldPos, Quaternion.identity);
            c.transform.SetParent(rotationCenter, true);
            c.transform.localScale = new Vector3(1,1,1);
        }
    }

}
