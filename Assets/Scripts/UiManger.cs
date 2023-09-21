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
    [SerializeField] RawImage controlAnimImage;
    [SerializeField] BasicInteractions variable;
    [SerializeField] Animator hotbarAnimator;
    [SerializeField] GameObject originalModel;
    [SerializeField] Texture pauseBtn, playBtn;

    private bool controlCheck, searchCheck, animCheck, animState, modelAnimState;
    private GameObject loadedAnimModel;

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
            if (check)
            {
                image.color = new Color(0.4056604f, 0.4056604f, 0.4056604f, 1);
            }
            else
            {
                image.color = Color.white;

            }
        }
    }

    public void SwitchBetweenDiffSprites(bool check, RawImage image)
    {
        if (check)
        {
            image.texture = pauseBtn;
        }
        else
        {
            image.texture = playBtn;
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

    public void LoadAnimationModel(GameObject model)
    {
        loadedAnimModel = Instantiate(model, Vector3.zero, Quaternion.identity);
        originalModel.SetActive(false);
    }

    public void TogglePlaying()
    {
        modelAnimState = !modelAnimState;

        if (modelAnimState)
        {
            loadedAnimModel.GetComponent<Animator>().speed = 0;
        }
        else
        {
            loadedAnimModel.GetComponent<Animator>().speed = 1;
        }

        SwitchBetweenDiffSprites(!modelAnimState, controlAnimImage);
    }

    public void ReturnToModel()
    {
        Destroy(loadedAnimModel);
        loadedAnimModel = null;
        originalModel.SetActive(true);
    }

}
