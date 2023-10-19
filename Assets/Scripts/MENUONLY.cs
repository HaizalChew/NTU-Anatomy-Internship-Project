using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MENUONLY : MonoBehaviour
{
    // Start is called before the first frame update

    public Image[] achivements;
    private bool achievementStatus;

    public Transform rotationCenter;
    public float radius;

    public GameObject panel;
    void Start()
    {
        SpawnAround();
    }

    // Update is called once per frame
    void Update()
    {
        OnAchievement();
    }

    public void OnAchievement()
    {
        for (int j = 0; j < AchievementManager.instance.loadStillChecking.Length; j++)
        {
            if (AchievementManager.instance.loadStillChecking[j] == true)
            {
                achivements[j].color = new Color(1f, 1f, 1f, 1f);
            }
        }

        for (int y = 0; y < AchievementManager.instance.loadStillCheckingMile.Length; y++)
        {
            if (AchievementManager.instance.loadStillCheckingMile[y] == true)
            {
                achivements[y + 4].color = new Color(1f, 1f, 1f, 1f);
            }
        }

        for (int x = 0; x < AchievementManager.instance.loadStillCheckingYear.Length; x++)
        {
            if (AchievementManager.instance.loadStillCheckingYear[x] == true)
            {
                achivements[x + 7].color = new Color(1f, 1f, 1f, 1f);
            }
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
                panel.SetActive(true);
            }
        }
    }
}
