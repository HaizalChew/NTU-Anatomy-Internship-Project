using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContentFilter : MonoBehaviour
{

    public GameObject containerPanel;
    public TMP_Dropdown dropDownPanel;
    public Dictionary<GameObject, GameObject> TopicContent = new Dictionary<GameObject, GameObject>();

    public GameObject[] contentArray;
    public Button[] topicArray;
    public static int saveYearInt;

    public Image bar;
    public Image twoBar;

    public int maximum;
    public int current;

    public int him;

    // Start is called before the first frame update
    void Start()
    {
        dropDownPanel.value = saveYearInt;
        DropDownPanelValueChanged(dropDownPanel);
        GetCurrentFill(saveYearInt);
        SwitchProgressBar();
        dropDownPanel.onValueChanged.AddListener(delegate { DropDownPanelValueChanged(dropDownPanel); });
        dropDownPanel.onValueChanged.AddListener(delegate { SwitchProgressBar(); });
        for (int i = 0; i < contentArray.Length; i++)
        {
            int count = i;
            topicArray[count].onClick.AddListener(delegate { him = count; });
            topicArray[count].onClick.AddListener(delegate { ShowContent(); });
        }


        }

    private int GetNumber(int i)
    {
        him = i;
        return i;
    }

    public void DropDownPanelValueChanged(TMP_Dropdown change)
    {
        if (change.value == 0)
        {
            FilterContent("Year1");
            saveYearInt = 0;
        }
        else
        {
            FilterContent("Year2");
            saveYearInt = 1;
        }
    }

    public void SwitchProgressBar()
    {

        if (ContentFilter.saveYearInt == 0)
        {
            bar.gameObject.SetActive(true);
            twoBar.gameObject.SetActive(false);
            GetCurrentFill(saveYearInt);

        }
        else
        {
            bar.gameObject.SetActive(false);
            twoBar.gameObject.SetActive(true);
            GetCurrentFill(saveYearInt);
        }
    }

    public void GetCurrentFill(int year)
    {
        if(year == 0)
        {
            current = CheckYearOneProgress();
            maximum = 6;
            float fillAmount = (float)current / (float)maximum;
            bar.fillAmount = fillAmount;
        }
        else
        {
            current = CheckYearTwoProgress();
            maximum = 6;
            float fillAmount = (float)current / (float)maximum;
            twoBar.fillAmount = fillAmount;
        }
    }

    public int CheckYearOneProgress()
    {
        int i = 0;
        int min = 0;
        foreach( bool check in SaveStateManager.instance.topicChecklistCompleted)
        {
           if(check && i < 6)
            {
                min++;
            }
           i++;
        }
        return min;
    }

    public int CheckYearTwoProgress()
    {
        int i = 0;
        int min = 0;
        foreach (bool check in SaveStateManager.instance.topicChecklistCompleted)
        {
            if (check && i > 5)
            {
                min++;
            }
            i++;
        }
        return min;
    }
    public int NumComplete()
    {
        int i = 0;
        foreach (bool topicCheck in SaveStateManager.instance.topicChecklistCompleted)
        {
            if (topicCheck)
            {
                i++;
            }
        }
        return i;
    }

    private void FilterContent(string year)
    {
        foreach (Transform child in containerPanel.transform)
        {
            if (child.tag == year)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ShowContentOnButton()
    {

        //ShowContent(contentArray[0]);

    }


    public void ShowContent()
    {
        AudioManager.instance?.PlaySoundEffect(1);

        if (contentArray[him].activeSelf == true)
        {
            contentArray[him].SetActive(false);
        }
        else
        {
            foreach (GameObject content in contentArray)
            {
                content.SetActive(false);
            }
            contentArray[him].SetActive(true);
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(him);
    }
}
