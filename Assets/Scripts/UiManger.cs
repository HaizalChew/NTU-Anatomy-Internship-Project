using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManger : MonoBehaviour
{
    [SerializeField] GameObject uiPanel, searchPanel, animationPanel;

    [SerializeField] Button button;

    [SerializeField] Image searchImage;
    [SerializeField] Image controlImage;

    [SerializeField] BasicInteractions variable;


    private Image image;
    private Color originColor;


    private bool controlCheck, searchCheck, uiCheck, uiSeleceted, animCheck;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SwitchSprite(bool check, Image image = null)
    {   
        if (image != null)
        {
            if (check == true)
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

    public void ChangePanel()
    {

    }

    public void ActivateControls()
    {
        controlCheck = !controlCheck;
        if (uiPanel != null)
        {
            uiPanel.SetActive(controlCheck);
            SwitchSprite(controlCheck, controlImage);

        }
    }
    public void ActivateSearch()
    {
        searchCheck = !searchCheck;
        if (searchPanel != null)
        {
            searchPanel.SetActive(searchCheck);
            SwitchSprite(searchCheck, searchImage);

        }
    }
    public void ShowPanel()
    {
        
    }

    public void ShowControls()
    {
        if(searchCheck == false)
        {
            ActivateControls();
        }
        else
        {
            ActivateControls();
            ActivateSearch();
        }
    }

    public void ShowSearch()
    {
        if (controlCheck == false)
        {
            ActivateSearch();
        }
        else
        {
            ActivateSearch();
            ActivateControls();
        }
    }
    public void ShowAnimation()
    {
        animCheck = !animCheck;

        animationPanel.SetActive(animCheck);
    }

}
