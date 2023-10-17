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

    public GameObject coronarySideModel;
    [SerializeField] GameObject coronaryModelAngio;
    [SerializeField] PartList partList;
    [SerializeField] Slider renderingSlider;
    public Animator dropdownPanel;
    
    public Material transMat;
    private Material newMat;

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

    public GameObject cloneContainer;

    [SerializeField] Image image;

    //[SerializeField] GameObject selectedViewModel;
    void Start()
    {
        if (model == null)
        {
            model = GameObject.FindGameObjectWithTag("Model");
        }

        if (camControl == null)
        {
            camControl = Camera.main.GetComponent<CameraControls>();
        }

        mousePos = new Vector2(0, 0);
        showCheck = false;
        objRect = new Rect(0, 0, 300, 100);
        if(transMat != null )
        {
            newMat = Instantiate(transMat);

        }
    }

    // Update is called once per frame
    void Update()
    {

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

                //selectedInstantiatedObj.transform.parent = model.transform;

                Material[] matArray = new Material[selectedInstantiatedObj.GetComponent<MeshRenderer>().materials.Length];

                for (int i = 0; i < matArray.Length; i++)
                {
                    matArray[i] = new Material(selectedMat);
                    if (!ColorDifferenceTreshold(selectedObj.GetComponent<MeshRenderer>().materials[i].color, selectedMat.GetColor("_Color2")))
                    {
                        matArray[i].color = selectedObj.GetComponent<MeshRenderer>().materials[i].color;
                    }
                }
                selectedInstantiatedObj.GetComponent<MeshRenderer>().materials = matArray;

                selectedInstantiatedObj.layer = 0;
                //selectedObj.gameObject.SetActive(false);
                selectedObj.gameObject.layer = LayerMask.NameToLayer("Seeable");

                camControl.ActivateRecentering(selectedObj);
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySoundEffect(0);
                }
                    

            }
        }
        else if (selectedObj != null && selected == null)
        {
            if (selectedInstantiatedObj != null)
            {
                Destroy(selectedInstantiatedObj);
                selectedInstantiatedObj = null;
            }

            selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
            selectedObj = null;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, selectableLayerMask))
            {
                selectedObj = hitInfo.transform;

                selectedInstantiatedObj = Instantiate(selectedObj.gameObject, selectedObj.position, selectedObj.rotation);

                //selectedInstantiatedObj.transform.parent = model.transform;

                Material[] matArray = new Material[selectedInstantiatedObj.GetComponent<MeshRenderer>().materials.Length];

                for (int i = 0; i < matArray.Length; i++)
                {
                    matArray[i] = new Material(selectedMat);
                    if (!ColorDifferenceTreshold(selectedObj.GetComponent<MeshRenderer>().materials[i].color, selectedMat.GetColor("_Color2")))
                    {
                        matArray[i].color = selectedObj.GetComponent<MeshRenderer>().materials[i].color;
                    }
                }
                selectedInstantiatedObj.GetComponent<MeshRenderer>().materials = matArray;

                selectedInstantiatedObj.layer = 0;
                selectedObj.gameObject.layer = LayerMask.NameToLayer("Seeable");

                camControl.ActivateRecentering(selectedObj);
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlaySoundEffect(0);
                }
            }
        }
        else if (selectedObj == null && selected != null)
        {
            selectedObj = selected;

            selectedInstantiatedObj = Instantiate(selectedObj.gameObject, selectedObj.position, selectedObj.rotation);

            Material[] matArray = new Material[selectedInstantiatedObj.GetComponent<MeshRenderer>().materials.Length];

            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i] = new Material(selectedMat);
                if (!ColorDifferenceTreshold(selectedObj.GetComponent<MeshRenderer>().materials[i].color, selectedMat.GetColor("_Color2")))
                {
                    matArray[i].color = selectedObj.GetComponent<MeshRenderer>().materials[i].color;
                }
            }
            selectedInstantiatedObj.GetComponent<MeshRenderer>().materials = matArray;

            selectedInstantiatedObj.layer = 0;
            selectedObj.gameObject.layer = LayerMask.NameToLayer("Seeable");

            camControl.ActivateRecentering(selectedObj);
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySoundEffect(0);
            }
        }
        else if (selectedObj != null && selected != null)
        {
            if (selectedInstantiatedObj != null)
            {
                Destroy(selectedInstantiatedObj);
                selectedInstantiatedObj = null;
            }

            selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
            selectedObj = null;

            selectedObj = selected;

            selectedInstantiatedObj = Instantiate(selectedObj.gameObject, selectedObj.position, selectedObj.rotation);

           //selectedInstantiatedObj.transform.parent = model.transform;

            Material[] matArray = new Material[selectedInstantiatedObj.GetComponent<MeshRenderer>().materials.Length];

            for (int i = 0; i < matArray.Length; i++)
            {
                matArray[i] = new Material(selectedMat);
                if (!ColorDifferenceTreshold(selectedObj.GetComponent<MeshRenderer>().materials[i].color, selectedMat.GetColor("_Color2")))
                {
                    matArray[i].color = selectedObj.GetComponent<MeshRenderer>().materials[i].color;
                }
            }
            selectedInstantiatedObj.GetComponent<MeshRenderer>().materials = matArray;

            selectedInstantiatedObj.layer = 0;
            selectedObj.gameObject.layer = LayerMask.NameToLayer("Seeable");

            camControl.ActivateRecentering(selectedObj);
            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySoundEffect(0);
            }
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
                    highlightMaterials[i] = new Material(highlightMaterial);
                    highlightMaterials[i].color = orignialMaterial[i].color;
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

            uiManagerScript.SwitchSprite(isolateCheck, image);

            foreach (Transform child in selectedObj.transform.parent)
            {
                if (child.name == selectedObj.name)
                {
                    //child.gameObject.SetActive(isolateCheck);
                    selectedInstantiatedObj.SetActive(isolateCheck);
                }
                else
                {
                    child.gameObject.SetActive(!isolateCheck);
                    selectedInstantiatedObj.SetActive(!isolateCheck);
                }

            }

            if (isolateCheck)
            {
                selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
            }
            else
            {
                selectedObj.gameObject.layer = LayerMask.NameToLayer("Seeable");
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


    GameObject ChangeMaterial(GameObject bodyPart)
    {
        GameObject changedMat = Instantiate(bodyPart, bodyPart.transform.position, bodyPart.transform.rotation, cloneContainer.transform);
        Destroy(changedMat.GetComponent<Rigidbody>());
        Destroy(changedMat.GetComponent<MeshCollider>());
        Renderer targetRend = changedMat.GetComponent<Renderer>();
        Renderer srcRend = bodyPart.GetComponent<Renderer>();
        Material newMat = Instantiate(transMat);
        newMat.mainTexture = srcRend.material.mainTexture;
        targetRend.material = newMat;
        return (changedMat);
    }

    public void ToggleVeinTransparent()
    {
        if (veinCheck)
        {
            foreach (Transform child in coronarySideModel.transform)
            {
                if (child.tag != "Vein")
                {

                    //ChangeMaterial(child.gameObject);
                    child.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            //foreach (Transform child in cloneContainer.transform)
            //{
            //    Destroy(child.gameObject);
            //}
            foreach (Transform child in coronarySideModel.transform)
            {
                if (child.tag != "Vein")
                {
                    child.gameObject.SetActive(true);
                }
            }
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
        partList.useExclude = viewMode;
        partList.ResetNameListOnButton();

        if (viewMode)
        {
            uiManagerScript.gameObject.GetComponent<QuizManager>().topicIndex = 1;
            UpdateSliderValue();
            ToggleCollider(coronarySideModel,viewMode);
            ResetDict();
            dropdownPanel.SetBool("IsOpen", true);

            if (selectedObj != null)
            {
                selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
                selectedObj = null;
                Destroy(selectedInstantiatedObj);
            }
            
        }
        else
        {
            uiManagerScript.gameObject.GetComponent<QuizManager>().topicIndex = 0;
            veinCheck = false;
     
            ToggleCollider(coronarySideModel, viewMode);
            ResetDict();
            dropdownPanel.SetBool("IsOpen", false);

            if (selectedObj != null)
            {
                selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
                selectedObj = null;
                Destroy(selectedInstantiatedObj);
            }
        }

    }

    private bool ColorDifferenceTreshold(Color color1, Color color2)
    {
        float treshold = .5f;
        float distance = Mathf.Sqrt(Mathf.Pow(color2.r - color1.r, 2) + Mathf.Pow(color2.g - color1.g, 2) + Mathf.Pow(color2.b - color1.b, 2));

        if (distance < treshold)
        {
            return true;
        }
        else
        {
            return false;
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
