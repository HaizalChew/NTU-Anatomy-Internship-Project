using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static SaveStateManager;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI titleUi;
    public TextMeshProUGUI subTextUi;
    public Image achievementUi;
    public Sprite[] achievementPng;

    List<UniqueAchievement> achievementPanels = new List<UniqueAchievement>();
    List<MilestoneAchievement> milestonePanels = new List<MilestoneAchievement>();


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
    void Start()
    {
        achievementPanels.Add(new UniqueAchievement() { uniqueNum = 0, pngNum = 0, stillChecking = false, title = "Getting Started", subMessage = "Complete Chapter1" });
        achievementPanels.Add(new UniqueAchievement() { uniqueNum = 1, pngNum = 1, stillChecking = false, title = "Heart Expert", subMessage = "Complete Chapter2" });
        achievementPanels.Add(new UniqueAchievement() { uniqueNum = 5, pngNum = 2, stillChecking = false, title = "Spine Expert", subMessage = "Complete Chapter6" });
        achievementPanels.Add(new UniqueAchievement() { uniqueNum = 8, pngNum = 3, stillChecking = false, title = "Mouth Expert", subMessage = "Complete Chapter8" });

        milestonePanels.Add(new MilestoneAchievement() { numComplete = 4, pngNum = 4, stillChecking = false, title = "Nurse", subMessage = "Complete 4 Chapters" });
        milestonePanels.Add(new MilestoneAchievement() { numComplete = 8, pngNum = 5, stillChecking = false, title = "Assistant", subMessage = "Complete 8 Chapters" });
        milestonePanels.Add(new MilestoneAchievement() { numComplete = 12, pngNum = 6, stillChecking = false, title = "Doctor", subMessage = "Complete All Chapters" });
    }

    // Update is called once per frame
    void Update()
    {
        CheckUniqueAchievement();
        CheckMilestoneAchievement();
    }

    private void CheckUniqueAchievement()
    {
        foreach (UniqueAchievement uq in achievementPanels)
        {
            if (SaveStateManager.instance.topicChecklistCompleted[uq.uniqueNum] == true && uq.stillChecking == false)
            {
                achievementUi.sprite = achievementPng[uq.pngNum];
                titleUi.text = uq.title;
                subTextUi.text = uq.subMessage;
                uq.stillChecking = true;
            }
        }
    }

    private void CheckMilestoneAchievement()
    {
        foreach (MilestoneAchievement uq in milestonePanels)
        {
            if(SaveStateManager.instance.NumComplete() == uq.numComplete && uq.stillChecking == false)
            {
                achievementUi.sprite = achievementPng[uq.pngNum];
                titleUi.text = uq.title;
                subTextUi.text = uq.subMessage;
                uq.stillChecking = true;
            }
        }
    }
}
