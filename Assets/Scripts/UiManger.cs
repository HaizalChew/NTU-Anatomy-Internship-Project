using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManger : MonoBehaviour
{
    [SerializeField] GameObject uiPanel;
    [SerializeField] GameObject searchPanel;

    [SerializeField] Button button;

    private Image image;
    private Color originColor;


    private bool controlCheck, searchCheck, uiCheck, selectCheck;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchSprite(Image image = null)
    {
        if (image != null)
        {
            uiCheck = !uiCheck;
            if(uiCheck == true)
            {
                originColor = image.color;
                image.color = new Color(0.4056604f, 0.4056604f, 0.4056604f, 1);
                Debug.Log("Yes");
            }
            else
            {
                image.color = originColor;
                Debug.Log("No");
            }
        }
    }

    public void ShowControls()
    {
        if(searchCheck == false)
        {
            controlCheck = !controlCheck;

            if (uiPanel != null)
            {
                uiPanel.SetActive(controlCheck);
                

            }
            else
            {
                uiPanel.SetActive(controlCheck);

            }
        }
    }

    public void ShowSearch()
    {
        if (controlCheck == false)
        {
            searchCheck = !searchCheck;

            if (searchPanel != null)
            {
                Debug.Log("1");
                searchPanel.SetActive(searchCheck);

            }
            else
            {
                Debug.Log("working");
                searchPanel.SetActive(searchCheck);

            }
        }
    }
}
