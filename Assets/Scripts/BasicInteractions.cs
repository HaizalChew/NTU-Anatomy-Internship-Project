using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class BasicInteractions : MonoBehaviour
{
    [SerializeField] Material selectedMat;
    [SerializeField] Material highlightMaterial;
    [SerializeField] Material[] orignialMaterial;
    [SerializeField] public Transform selectedObj;
    [SerializeField] LayerMask selectableLayerMask;
    public GameObject selectedInstantiatedObj;
    public GameObject model;
    [SerializeField] CameraControls camControl;
    [SerializeField] Button isolateBtn;

    public GameObject coronaryModel;
    public GameObject coronarySideModel;
    [SerializeField] GameObject coronaryModelAngio;
    [SerializeField] Slider renderingSlider;
    public Animator dropdownPanel;

    private Transform coronaryTransform;
    private Transform coronarySideTransform;

    [SerializeField] PartList partListScript;
    [SerializeField] UiManger uiManagerScript;

    private Vector3 toolTipPos;
    private Vector2 mousePos;
    private Vector3 offset = new Vector3(16, 16, 0);
    private Rect objRect;
    private bool showCheck;

    public float sliderValue;

    public Transform highlight;
    private RaycastHit raycastHit;
    public bool isolateCheck;
    public bool veinCheck;
    public bool viewMode;
    public bool changeView;


    [SerializeField] UiManger uiMangerScript;
    [SerializeField] Image image;

    //[SerializeField] GameObject selectedViewModel;
    void Start()
    {
        mousePos = new Vector2(0, 0);
        showCheck = false;
        objRect = new Rect(0, 0, 300, 100);
    }

    // Update is called once per frame
    void Update()
    {   
        ShowVein();

        if (isolateCheck == false)
        {
            if (Input.GetMouseButtonDown(1))
            {

                SelectPart();
            }

            HighlightPart();
            OnMouseEnter();

        }
        else
        {

        }
    }

    public void SelectPart(Transform selected = null)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (selectedObj == null && selected == null)
        {
            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, selectableLayerMask))
            {
                selectedObj = hitInfo.transform;

                selectedInstantiatedObj = Instantiate(selectedObj.gameObject, selectedObj.position, selectedObj.rotation);

                selectedInstantiatedObj.transform.parent = model.transform;

                Material[] matArray = new Material[selectedInstantiatedObj.GetComponent<MeshRenderer>().materials.Length];

                for (int i = 0; i < matArray.Length; i++)
                {
                    matArray[i] = selectedMat;
                }
                selectedInstantiatedObj.GetComponent<MeshRenderer>().materials = matArray;

                selectedInstantiatedObj.layer = 0;
                selectedObj.gameObject.SetActive(false);

                camControl.ActivateRecentering(selectedObj);

            }
        }
        else if (selectedObj != null && selected == null)
        {
            if (selectedInstantiatedObj != null)
            {
                Destroy(selectedInstantiatedObj);
                selectedInstantiatedObj = null;
            }

            selectedObj.gameObject.SetActive(true);
            selectedObj = null;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, selectableLayerMask))
            {
                selectedObj = hitInfo.transform;

                selectedInstantiatedObj = Instantiate(selectedObj.gameObject, selectedObj.position, selectedObj.rotation);

                selectedInstantiatedObj.transform.parent = model.transform;

                Material[] matArray = new Material[selectedInstantiatedObj.GetComponent<MeshRenderer>().materials.Length];

                for (int i = 0; i < matArray.Length; i++)
                {
                    matArray[i] = selectedMat;
                }
                selectedInstantiatedObj.GetComponent<MeshRenderer>().materials = matArray;

                selectedInstantiatedObj.layer = 0;
                selectedObj.gameObject.SetActive(false);

                camControl.ActivateRecentering(selectedObj);
            }
        }
        else if (selectedObj == null && selected != null)
        {
            selectedObj = selected;

            selectedInstantiatedObj = Instantiate(selectedObj.gameObject, selectedObj.position, selectedObj.rotation);

            Material[] matArray = new Material[selectedInstantiatedObj.GetComponent<MeshRenderer>().materials.Length];

            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i] = selectedMat;
            }
            selectedInstantiatedObj.GetComponent<MeshRenderer>().materials = matArray;

            selectedInstantiatedObj.layer = 0;
            selectedObj.gameObject.SetActive(false);

            camControl.ActivateRecentering(selectedObj);
        }
        else if (selectedObj != null && selected != null)
        {
            if (selectedInstantiatedObj != null)
            {
                Destroy(selectedInstantiatedObj);
                selectedInstantiatedObj = null;
            }

            selectedObj.gameObject.SetActive(true);
            selectedObj = null;

            selectedObj = selected;

            selectedInstantiatedObj = Instantiate(selectedObj.gameObject, selectedObj.position, selectedObj.rotation);

            selectedInstantiatedObj.transform.parent = model.transform;

            Material[] matArray = new Material[selectedInstantiatedObj.GetComponent<MeshRenderer>().materials.Length];

            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i] = selectedMat;
            }
            selectedInstantiatedObj.GetComponent<MeshRenderer>().materials = matArray;

            selectedInstantiatedObj.layer = 0;
            selectedObj.gameObject.SetActive(false);

            camControl.ActivateRecentering(selectedObj);
        }
    }

    private void HighlightPart()
    {
        if (highlight != null)
        {
            highlight.GetComponent<MeshRenderer>().materials = orignialMaterial;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Hightlight Object when hovered
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, Mathf.Infinity, selectableLayerMask))
        {
            highlight = raycastHit.transform;
            if (highlight != selectedObj)
            {
                orignialMaterial = highlight.GetComponent<MeshRenderer>().materials;
                Material[] highlightMaterials = new Material[orignialMaterial.Length];

                for (int i = 0; i < highlightMaterials.Length; i++)
                {
                    highlightMaterials.SetValue(highlightMaterial, i);
                }

                highlight.GetComponent<MeshRenderer>().materials = highlightMaterials;

            }
            else
            {
                highlight = null;
            }
        }
    }
    public void IsolatePart()
    {
        if (selectedObj)
        {
            isolateCheck = !isolateCheck;

            uiMangerScript.SwitchSprite(isolateCheck, image);

            foreach (Transform child in model.transform)
            {
                if (child.name == selectedObj.name)
                {
                    child.gameObject.SetActive(isolateCheck);
                    selectedInstantiatedObj.SetActive(isolateCheck);
                }
                else
                {
                    child.gameObject.SetActive(!isolateCheck);
                    selectedInstantiatedObj.SetActive(!isolateCheck);
                }

            }
        }
    }

    private void OnGUI()
    {
        if (showCheck)
        {
            if (highlight != null && !QuizManager.quizModeActive)
            {
                mousePos = Input.mousePosition + offset;
                objRect.x = mousePos.x;

                objRect.y = Mathf.Abs(mousePos.y - Camera.main.pixelHeight);
                GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 40;
                GUI.Label(objRect, highlight.name);
            }
        }
    }

    public void UpdateSliderValue()
    {
        sliderValue = renderingSlider.value;
        veinCheck = true;
    }

    public void ToggleVeinTransparent(GameObject model, bool check)
    {
        if (check)
        {
            foreach (Transform child in model.transform)
            {
                if (child.tag != "Vein")
                {
                    child.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 3);
                    //Turn on Alpha Blending
                    child.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    child.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    child.GetComponent<MeshRenderer>().material.EnableKeyword("_ALPHABLEND_ON");
                    child.GetComponent<MeshRenderer>().material.renderQueue = 3000;
                    Color color = child.GetComponent<MeshRenderer>().material.color;
                    color.a = sliderValue;
                    child.GetComponent<MeshRenderer>().material.color = color;
                }
            }
        }
        else
        {
            foreach (Transform child in model.transform)
            {
                if (child.tag != "Vein")
                {
                    child.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 1);
                    //Turn on Alpha Blending
                    child.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
                    child.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
                    child.GetComponent<MeshRenderer>().material.DisableKeyword("_ALPHABLEND_ON");
                    child.GetComponent<MeshRenderer>().material.renderQueue = 2000;
                    Color color = child.GetComponent<MeshRenderer>().material.color;
                    color.a = 1;
                    child.GetComponent<MeshRenderer>().material.color = color;
                }
            }
        }
    }

    public void ShowVein()
    {
        if (coronaryModel != null)
        {
            ToggleVeinTransparent(coronaryModel, veinCheck);
            ToggleVeinTransparent(coronarySideModel, veinCheck);
        }
        
    }

    public void ShowSlider()
    {
        if (viewMode)
        {
            renderingSlider.gameObject.SetActive(true);
        }
        else
        {
            renderingSlider.gameObject.SetActive(false);
        }
    }

    public void ToggleCollider(GameObject model, bool viewMode)
    {
        foreach (Transform child in model.transform)
        {
            if (child.tag != "Vein")
            {
                if(viewMode)
                {
                    child.GetComponent<Collider>().enabled = !viewMode;
                }
                else
                {
                    child.GetComponent<Collider>().enabled = !viewMode;
                }
            }
        }
    }
    public void ActivateViewMode()
    {
        viewMode = !viewMode;
        if (viewMode)
        {
            uiManagerScript.gameObject.GetComponent<QuizManager>().topicIndex = 1;
            UpdateSliderValue();
            ToggleCollider(coronaryModel,viewMode);
            ToggleCollider(coronarySideModel,viewMode);
            ResetDict();
            dropdownPanel.SetBool("IsOpen", true);
        }
        else
        {
            uiManagerScript.gameObject.GetComponent<QuizManager>().topicIndex = 0;
            veinCheck = false;
            ToggleCollider(coronaryModel, viewMode);
            ToggleCollider(coronarySideModel, viewMode);
            ResetDict();
            dropdownPanel.SetBool("IsOpen", false);
        }

    }

    public void ResetDict()
    {
        //partListScript.ResetNameList(coronaryModel.transform);
        //partListScript.ResetNameList(coronarySideModel.transform);

    }

    ////public void ChangeViewMode()
    ////{
    ////    changeView = !changeView;
    ////    if (viewMode)
    ////    {
    ////        ActivateViewMode();
    ////    }
    ////    veinCheck = false;

    ////    if (changeView)
    ////    {
    ////        model = coronaryModelAngio;
    ////        partListScript.parentModel = coronaryModelAngio.transform;
    ////        if (uiManagerScript.sliderCheck)
    ////        {
    ////            uiManagerScript.ActivateTransSlider();
    ////        }
    ////        if (selectedObj != null)
    ////        {
    ////            coronaryModel.gameObject.SetActive(!changeView);
    ////            coronaryModelAngio.gameObject.SetActive(changeView);
    ////            partListScript.ResetNameList();
    ////            partListScript.InitializeDictionary();
    ////            Destroy(selectedInstantiatedObj);
    ////            selectedObj.gameObject.SetActive(true);
    ////            selectedObj = null;
    ////        }
    ////        else
    ////        {
    ////            coronaryModel.gameObject.SetActive(!changeView);
    ////            coronaryModelAngio.gameObject.SetActive(changeView);
    ////            partListScript.ResetNameList();
    ////            partListScript.InitializeDictionary();
    ////        }
    ////    }
    ////    else
    ////    {
    ////        model = coronaryModel;
    ////        uiManagerScript.gameObject.GetComponent<QuizManager>().topicIndex = 0;
    ////        partListScript.parentModel = coronaryModel.transform;
    ////        if (uiManagerScript.sliderCheck)
    ////        {
    ////            uiManagerScript.ActivateTransSlider();
    ////        }
    ////        if (selectedObj != null)
    ////        {
    ////            coronaryModel.gameObject.SetActive(!changeView);
    ////            coronaryModelAngio.gameObject.SetActive(changeView);
    ////            partListScript.ResetNameList();
    ////            partListScript.InitializeDictionary();
    ////            Destroy(selectedInstantiatedObj);
    ////            selectedObj.gameObject.SetActive(true);
    ////            selectedObj = null;
    ////        }
    ////        else
    ////        {
    ////            coronaryModel.gameObject.SetActive(!changeView);
    ////            coronaryModelAngio.gameObject.SetActive(changeView);
    ////            partListScript.ResetNameList();
    ////            partListScript.InitializeDictionary();
    ////        }
    ////    }

    public void OnMouseEnter() { showCheck = true; }
    public void OnMouseExit() { showCheck = false; }
}
