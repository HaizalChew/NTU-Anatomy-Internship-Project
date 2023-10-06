using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ContentFilter : MonoBehaviour
{

    public GameObject containerPanel;
    public TMP_Dropdown dropDownPanel;

    // Start is called before the first frame update
    void Start()
    {
        FilterContent("Year1");
        dropDownPanel = GetComponent<TMP_Dropdown>();
        dropDownPanel.onValueChanged.AddListener(delegate { DropDownPanelValueChanged(dropDownPanel); });
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
