using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject fadeCanvas;
    [SerializeField] private RectTransform progressBar;
    [SerializeField] private GameObject[] completedCheckIndex;

    public Image[] achivements;
    public Transform rotationCenter;
    public float radius;

    private bool achievementStatus;
    public GameObject panel;

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
        AudioManager.instance?.PlaySoundEffect(2);

        SceneManager.LoadSceneAsync("Menu");
    }

    public void ToOtherScenes(int buildIndex)
    {
        AudioManager.instance?.PlaySoundEffect(2);
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

            //GameObject c = Instantiate(achivements[i], worldPos, Quaternion.identity);
            //c.transform.SetParent(rotationCenter, true);
            //c.transform.localScale = new Vector3(1,1,1);
        }
    }

    public void ToggleAchievement()
    {
        achievementStatus = !achievementStatus;
        AudioManager.instance?.PlaySoundEffect(1);
        if (achievementStatus)
        {
            foreach (Image achievement in achivements)
            {
                achievement.enabled = true;
                panel.SetActive(false);
            }
        }
        else
        {
            foreach (Image achievement in achivements)
            {
                achievement.enabled = false;
                panel.SetActive(true) ;
            }
        }
    }


    public void OnAchievement()
    {
        int i = 0;
        foreach(bool check in AchievementManager.instance.loadStillChecking)
        {
            if (check)
            {
                achivements[i].color = new Color(1f, 1f, 1f, 1f);
                i++;
            }
        }
        int y = 0;
        foreach (bool check in AchievementManager.instance.loadStillCheckingMile)
        {
            if (check)
            {
                achivements[y+4].color = new Color(1f, 1f, 1f, 1f);
                y++;
            }
        }
        int x = 0;
        foreach (bool check in AchievementManager.instance.loadStillCheckingYear)
        {
            if (check)
            {
                achivements[x+10].color = new Color(1f, 1f, 1f, 1f);
                x++;
            }
        }
    }
}   
