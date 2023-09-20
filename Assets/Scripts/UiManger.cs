using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManger : MonoBehaviour
{
    [SerializeField] GameObject uiPanel;
    [SerializeField] GameObject searchPanel;
    [SerializeField] GameObject animationPanel;

    [SerializeField] Button button;

    [SerializeField] Image searchImage;
    [SerializeField] Image controlImage;

    [SerializeField] BasicInteractions variable;


    private Image image;
    private Color originColor;


    private bool controlCheck, searchCheck, uiCheck, uiSeleceted;
    private bool animCheck;


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
            if (uiCheck == true)
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
        if(searchCheck == false && variable.isolateCheck == false)
        {
            controlCheck = !controlCheck;

            if (uiPanel != null)
            {
                uiPanel.SetActive(controlCheck);
                SwitchSprite(controlImage);


            }
            else
            {
                uiPanel.SetActive(controlCheck);

            }
        }
    }

    public void ShowSearch()
    {
        if (controlCheck == false && variable.isolateCheck == false)
        {
            searchCheck = !searchCheck;

            if (searchPanel != null)
            {
                Debug.Log("1");
                searchPanel.SetActive(searchCheck);
                SwitchSprite(searchImage);

            }
            else
            {
                Debug.Log("working");
                searchPanel.SetActive(searchCheck);

            }
        }
    }

    public void ShowAnimation()
    {
        animCheck = !animCheck;

        animationPanel.SetActive(animCheck);
    }
}
