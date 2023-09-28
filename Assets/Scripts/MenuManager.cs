using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadSceneAsync("Menu");
    }

    public void ToOtherScenes(int buildIndex)
    {
        SceneManager.LoadSceneAsync(buildIndex);
    }
}
