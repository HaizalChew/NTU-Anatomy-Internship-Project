using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveStateManager : MonoBehaviour
{

    public static SaveStateManager instance;
    public bool[] topicChecklistCompleted;
    public GameObject[] achievementPanel;

    public int loadYearInt;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (loadYearInt != 0)
        {
            ContentFilter.saveYearInt = loadYearInt;
        }

        LoadData();
        AchievementManager.instance.LoadAchievementData();
        Debug.Log(topicChecklistCompleted[1]);

        //for (int i = 0; i < topicChecklistCompleted.Length; i++)
        //{
        //    topicChecklistCompleted[i] = true;
        //}
    }

    private void Update()
    {

        loadYearInt = ContentFilter.saveYearInt;
    }
    

    //Check if All Quiz is Compeleted
    public bool allComplete()
    {
        foreach (bool topicCheck in topicChecklistCompleted)
        {
            if (topicCheck == false)
            {
                return false;
            }
        }
        return true;
    }

    public bool CheckCondition(int index)
    {
        if (topicChecklistCompleted[index] == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int NumComplete()
    {
        int i = 0;
        foreach (bool topicCheck in topicChecklistCompleted)
        {
            if (topicCheck)
            {
                i++;
            }
        }
        return i;
    }

    public void LoadData()
    {
        for (int i = 0; i < topicChecklistCompleted.Length; i++)
        {
            if (PlayerPrefs.GetInt("topicCheckListCompleted" + i) == 1)
            {
                topicChecklistCompleted[i] = true;
                Debug.Log("Check");
            }
        }
    }
}
