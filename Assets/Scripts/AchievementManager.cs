using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SaveStateManager;
using UnityEngine.UI;
using Unity.VisualScripting;

public class AchievementManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI titleUi;
    public TextMeshProUGUI subTextUi;
    public Image achievementUi;
    public GameObject achievementPanel;
    public Sprite[] achievementPng;

    public static AchievementManager instance;

    public bool[] loadStillChecking;
    public bool[] loadStillCheckingMile;
    public bool[] loadStillCheckingYear;

    List<UniqueAchievement> achievementPanels = new List<UniqueAchievement>();
    List<MilestoneAchievement> milestonePanels = new List<MilestoneAchievement>();
    List<YearAchievement> yearPanels = new List<YearAchievement>();


    public class UniqueAchievement
    {
        public int uniqueNum { get; set; }
        public int pngNum { get; set; }
        public bool stillChecking { get; set; }
        public string title { get; set; }
        public string subMessage { get; set; }
    }

    public class MilestoneAchievement
    {
        public int numComplete { get; set; }
        public int pngNum { get; set; }
        public bool stillChecking { get; set; }
        public string title { get; set; }
        public string subMessage { get; set; }
    }

    public class YearAchievement
    {
        public int[] numNeeded { get; set; }
        public int pngNum { get; set; }
        public bool stillChecking { get; set; }
        public string title { get; set; }
        public string subMessage { get; set; }
    }

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
    void Start()
    {
        achievementPanels.Add(new UniqueAchievement() { uniqueNum = 0, pngNum = 0, stillChecking = loadStillChecking[0] , title = "Getting Started", subMessage = "Complete Chapter1" });
        achievementPanels.Add(new UniqueAchievement() { uniqueNum = 1, pngNum = 1, stillChecking = loadStillChecking[1], title = "Heart Expert", subMessage = "Complete Chapter2" });
        achievementPanels.Add(new UniqueAchievement() { uniqueNum = 5, pngNum = 2, stillChecking = loadStillChecking[2], title = "Spine Expert", subMessage = "Complete Chapter6" });
        achievementPanels.Add(new UniqueAchievement() { uniqueNum = 8, pngNum = 3, stillChecking = loadStillChecking[3], title = "Mouth Expert", subMessage = "Complete Chapter8" });

        milestonePanels.Add(new MilestoneAchievement() { numComplete = 4, pngNum = 4, stillChecking = loadStillCheckingMile[0], title = "Nurse", subMessage = "Complete 4 Chapters" });
        milestonePanels.Add(new MilestoneAchievement() { numComplete = 8, pngNum = 5, stillChecking = loadStillCheckingMile[1], title = "Assistant", subMessage = "Complete 8 Chapters" });
        milestonePanels.Add(new MilestoneAchievement() { numComplete = 12, pngNum = 6, stillChecking = loadStillCheckingMile[2], title = "Doctor", subMessage = "Complete All Chapters" });

        yearPanels.Add(new YearAchievement() { numNeeded = new int[]{0,1,2,3,4,5}, pngNum = 7, stillChecking = loadStillCheckingYear[0], title = "Finish Year1", subMessage = "Complete All Year 1 Chapters" });
        yearPanels.Add(new YearAchievement() { numNeeded = new int[]{6,7,8,9,10,11}, pngNum = 8, stillChecking = loadStillCheckingYear[1], title = "Finish Year2", subMessage = "Complete All Year 2 Chapters" });


    }

    public IEnumerator StartDisplayAchievement()
    {
        //yield return new WaitWhile(() => achievementPanel.activeSelf);
        CheckUniqueAchievement();
        //Debug.Log("wait1");
        yield return new WaitWhile(() => achievementPanel.activeSelf);
        CheckMilestoneAchievement();
        //Debug.Log("wait2");
        yield return new WaitWhile(() => achievementPanel.activeSelf);
        CheckYearAchievement();
        //Debug.Log("wait3");
        yield return new WaitWhile(() => achievementPanel.activeSelf);
    }
    private void CheckUniqueAchievement()
    {
        int i = 0;
        foreach (UniqueAchievement uq in achievementPanels)
        {
            if (SaveStateManager.instance.topicChecklistCompleted[uq.uniqueNum] == true && uq.stillChecking == false)
            {
                //yield return StartCoroutine(CheckMilestoneAchievement());
                achievementUi.sprite = achievementPng[uq.pngNum];
                titleUi.text = uq.title;
                subTextUi.text = uq.subMessage;
                achievementPanel.SetActive(true);
                uq.stillChecking = true;
                loadStillChecking[i] = uq.stillChecking;
                PlayerPrefs.SetInt("loadStillChecking" + i, loadStillChecking[i] ? 1 : 0);
                Debug.Log("changed1");
                i++;
                //yield return new WaitWhile(() => achievementPanel.activeSelf);

            }
            Debug.Log("escape1");
            i++;
        }
        Debug.Log("end1");
    }

    private void CheckMilestoneAchievement()
    {
        int i = 0;
        foreach (MilestoneAchievement uq in milestonePanels)
        {
            if(SaveStateManager.instance.NumComplete() == uq.numComplete && uq.stillChecking == false)
            {
                //yield return StartCoroutine(CheckYearAchievement());
                achievementUi.sprite = achievementPng[uq.pngNum];
                titleUi.text = uq.title;
                subTextUi.text = uq.subMessage;
                achievementPanel.SetActive(true);
                uq.stillChecking = true;
                loadStillCheckingMile[i] = uq.stillChecking;
                PlayerPrefs.SetInt("loadStillChecking" + i, loadStillCheckingMile[i] ? 1 : 0);
                Debug.Log("changed2");
                i++;
                //yield return new WaitWhile(() => achievementPanel.activeSelf);
            }
            i++;
            Debug.Log("escape2");
        }
        Debug.Log("end2");
    }

    public void CheckYearAchievement()
    {
        int i = 0;
        foreach(YearAchievement uq in yearPanels)
        {
            foreach(int num in uq.numNeeded)
            {
                Debug.Log(num);
                Debug.Log(SaveStateManager.instance.topicChecklistCompleted[num] + "NO.");
                if (SaveStateManager.instance.topicChecklistCompleted[num] == true && uq.stillChecking == false)
                {
                    continue;
                }
                else
                {
                    Debug.Log("escape3");
                    i++;
                    goto end;
                }
            }                                

            //yield return StartCoroutine(CheckMilestoneAchievement());
            achievementUi.sprite = achievementPng[uq.pngNum];
            titleUi.text = uq.title;
            subTextUi.text = uq.subMessage;
            achievementPanel.SetActive(true);
            uq.stillChecking = true;
            loadStillCheckingYear[i] = uq.stillChecking;
            PlayerPrefs.SetInt("loadStillChecking" + i, loadStillCheckingYear[i] ? 1 : 0);
            Debug.Log("changed3");
            i++;
            continue;
        end: { 

            }
            //yield return new WaitWhile(() => achievementPanel.activeSelf);
        }
        Debug.Log("end3");
    }

    public void LoadAchievementData()
    {
        Debug.Log("Load");
        //foreach (UniqueAchievement uq in achievementPanels)
        //{
        //    Debug.Log("loop");
        //    int i = 0;
        //    Debug.Log("loop");
        //    if(PlayerPrefs.GetInt("loadStillChecking" + i) == uq )
        //    {
        //        loadStillChecking[i] = true;
        //        Debug.Log("CheckUnique" + loadStillChecking[i]);
        //    }
        //}
        for (int i = 0; i < loadStillChecking.Length; i++)
        {
            if (PlayerPrefs.GetInt("loadStillChecking" + i) == 1)
            {
                loadStillChecking[i] = true;
            }
        }

        for (int i = 0; i < loadStillCheckingMile.Length; i++)
        {
            if (PlayerPrefs.GetInt("loadStillCheckingMile" + i) == 1)
            {
                loadStillCheckingMile[i] = true;
            }
        }
        for (int i = 0; i < loadStillCheckingYear.Length; i++)
        {
            if (PlayerPrefs.GetInt("loadStillCheckingYear" + i) == 1)
            {
                loadStillCheckingYear[i] = true;
            }
        }
    }


}
