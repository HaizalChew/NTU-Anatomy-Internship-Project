using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveStateManager : MonoBehaviour
{

    public static SaveStateManager instance;
    public Image bar;
    public bool[] topicChecklistCompleted = new bool[5];

    public int maximum;
    public int current;

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

        LoadData();
        Debug.Log(topicChecklistCompleted[1]);

        //for (int i = 0; i < topicChecklistCompleted.Length; i++)
        //{
        //    topicChecklistCompleted[i] = true;
        //}
        GetCurrentFill();

    }

    public bool allComplete()
    {
        foreach ( bool topicCheck in topicChecklistCompleted )
        {
            if ( topicCheck == false )
            {
                return false;
            }
        }
        return true;
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
        for ( int i = 0; i < topicChecklistCompleted.Length; i++ )
        {
            if(PlayerPrefs.GetInt("topicCheckListCompleted" + i) == 1)
            {
                topicChecklistCompleted[i] = true;
                Debug.Log("Check");
            }
        }
    }

    private void GetCurrentFill()
    {
        current = NumComplete();
        maximum = 6;
        float fillAmount = (float)current / (float)maximum;
        bar.fillAmount = fillAmount;
    }
}
