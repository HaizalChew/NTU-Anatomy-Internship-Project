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
        
    }
}
