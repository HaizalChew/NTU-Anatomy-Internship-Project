using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.WSA;
using UnityEngine.Windows.Speech;

public class ContentFilter : MonoBehaviour
{

    public GameObject containerPanel;
    public TMP_Dropdown dropDownPanel;
    public Dictionary<GameObject, GameObject> TopicContent = new Dictionary<GameObject, GameObject>();

    public GameObject[] contentArray; 
    public Button[] topicArray;

    public int him;

    // Start is called before the first frame update
    void Start()
    {
        FilterContent("Year1");
        dropDownPanel.onValueChanged.AddListener(delegate { DropDownPanelValueChanged(dropDownPanel); });
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
       if ( change.value == 0)
        {
            FilterContent("Year1");
        }
        else
        {
            FilterContent("Year2");
        }
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
