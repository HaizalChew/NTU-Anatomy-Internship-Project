using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManger : MonoBehaviour
{
    [SerializeField] GameObject uiPanel, searchPanel, animationPanel;
    [SerializeField] Button button;
    [SerializeField] Image searchImage, controlImage, animImage, angioImage;
    [SerializeField] RawImage controlAnimImage;
    [SerializeField] Animator hotbarAnimator;
    [SerializeField] GameObject originalModel;
    [SerializeField] Texture pauseBtn, playBtn;
    [SerializeField] Slider transSlider;
    [SerializeField] TMP_Text displayName;
    [SerializeField] Slider zoomSlider;
    [SerializeField] SelectableHandler orbitControl;
    [SerializeField] Button plus, minus;
    [SerializeField] CameraControls camControls;

    [SerializeField] BasicInteractions basicInteractions;

    private bool controlCheck, searchCheck, animCheck, animState, modelAnimState;
    public bool sliderCheck;
    private GameObject loadedAnimModel;

    // Start is called before the first frame update
    void Start()
    {
        if (basicInteractions == null)
        {
            basicInteractions = GameObject.FindGameObjectWithTag("GameController").GetComponent<BasicInteractions>();
        }

        if (camControls == null)
        {
            camControls = Camera.main.GetComponent<CameraControls>();
        }
        
        if (originalModel == null)
        {
            originalModel = GameObject.FindGameObjectWithTag("Model");
        }

        if (camControls != null)
        {
            camControls.zoomSlider = zoomSlider;
            camControls.selectableHandler = orbitControl;
            plus.onClick.AddListener(delegate { camControls.SetZoomCamera(2); });
            minus.onClick.AddListener(delegate { camControls.SetZoomCamera(-2); });
        }   
    }

    // Update is called once per frame
    void Update()
    {
        SwitchSprite(controlCheck, controlImage);
        SwitchSprite(searchCheck, searchImage);
        SwitchSprite(animCheck, animImage);

        DisplayName();

        if (angioImage != null)
        {
            SwitchSpritePink(basicInteractions.viewMode, angioImage);
        }
        

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

    public void SwitchSpritePink(bool check, Image image = null)
    {
        if (image != null)
        {
            if (check)
            {
                image.color = new Color(0.8301887f, 0.206764f, 0.5564725f, 1);
            }
            else
            {
                image.color = Color.white;

            }
        }
    }

    public void DisplayName()
    {
        if (basicInteractions.selectedObj != null)
        {
            displayName.text = basicInteractions.selectedObj.name;
        }
        else
        {
            displayName.text = null;
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

    public void ActivateTransSlider()
    {
        sliderCheck = !sliderCheck;
        if (sliderCheck)
        {
            transSlider.gameObject.SetActive(true);
        }
        else
        {
            transSlider.gameObject.SetActive(false);
        }
    }

}
