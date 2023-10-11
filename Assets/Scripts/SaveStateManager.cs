using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveStateManager : MonoBehaviour
{

    public static SaveStateManager instance;

    public bool[] topicChecklistCompleted = new bool[5];

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
}
