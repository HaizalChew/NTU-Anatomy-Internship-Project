using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManger : MonoBehaviour
{
    [SerializeField] GameObject uiPanel, searchPanel, animationPanel;
    [SerializeField] Button button;
    [SerializeField] Image searchImage, controlImage, animImage;
    [SerializeField] BasicInteractions variable;
    [SerializeField] Animator hotbarAnimator;

    private Image image;
    private Color originColor;
    private bool controlCheck, searchCheck, animCheck,animState;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        SwitchSprite(controlCheck, controlImage);
        SwitchSprite(searchCheck, searchImage);
        SwitchSprite(animCheck, animImage);
    }

    public void SwitchSprite(bool check, Image image = null)
    {   
        if (image != null)
        {
            if (check == true)
            {
                originColor = image.color;
                image.color = new Color(0.4056604f, 0.4056604f, 0.4056604f, 1);
            }
            else
            {
                image.color = Color.white;

            }
        }
    }

    public void ActivateControls()
    {
        controlCheck = !controlCheck;
        searchCheck = false;
        animCheck = false;

        uiPanel.SetActive(controlCheck);
        searchPanel.SetActive(false);
        animationPanel.SetActive(false);
    }
    public void ActivateSearch()
    {
        controlCheck = false;
        searchCheck = !searchCheck;
        animCheck = false;

        uiPanel.SetActive(false);
        searchPanel.SetActive(searchCheck);
        animationPanel.SetActive(false);
    }

    public void ActivateAnim()
    {
        controlCheck = false;
        searchCheck = false;
        animCheck = !animCheck;

        uiPanel.SetActive(false);
        searchPanel.SetActive(false);
        animationPanel.SetActive(animCheck);
    }

    public void ActivateAnimState()
    {
        animState = !animState;
        hotbarAnimator.SetBool("Close",animState);
        controlCheck = false;
        searchCheck = false;
        animCheck = false;

        uiPanel.SetActive(false);
        searchPanel.SetActive(false);
        animationPanel.SetActive(false);
    }

}
