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

    private bool objectSelected;

    public PostProcess postScript;
    public SelectPost selectPost;

    public GameObject cloneContainer;

    public bool selectHighlight;

    [SerializeField] Image image;

    private GameObject originObject;

    //[SerializeField] GameObject selectedViewModel;
    void Start()
    {
        //postScript.currentOutline = postScript.ApplyOutline;
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

            SelectPart();
            HighlightPart();
            OnMouseEnter();

        }
        else
        {

        }
        if(selectPost.SelectedObject != null){
            selectedObj = selectPost.SelectedObject.transform;
        }
        else{
            selectedObj = null;
        }
    }

    public void SelectPart(Transform selected = null)
    {
        if(selected == null){
            if (Input.GetMouseButtonDown(1))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool hitSelectable = Physics.Raycast(ray, out var hit) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable");
                if (hitSelectable) {
                    selectPost.GetComponent<SelectPost>().enabled = true;
                    selectPost.SelectedObject = hit.transform.GetComponent<Renderer>();
                    camControl.ActivateRecentering(selectPost.SelectedObject.transform);
                    AudioManager.instance?.PlaySoundEffect(0);
                    objectSelected = true;
                    highlight = hit.transform;
                } else {
                    selectPost.GetComponent<SelectPost>().enabled = false;
                    selectPost.SelectedObject = null;
                    objectSelected = false;
                }
            }
        }
        else{
                selectPost.GetComponent<SelectPost>().enabled = true;
                selectPost.SelectedObject = selected.transform.GetComponent<Renderer>();
                camControl.ActivateRecentering(selected);
                AudioManager.instance?.PlaySoundEffect(0);
                objectSelected = true;
                highlight = selected;
        }
    }

    private void HighlightPart()
    {
        if(objectSelected == false){
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable")))
            {
                postScript.GetComponent<PostProcess>().enabled = true;
                postScript.OutlinedObject = hit.transform.GetComponent<Renderer>();
                highlight = hit.transform;
            } else {
                postScript.GetComponent<PostProcess>().enabled = false;
                postScript.OutlinedObject = null;
                highlight = null;
            }
        }
        else{
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable")) && hit.transform.name != postScript.transform.name)
            {
                postScript.OutlinedObject = hit.transform.GetComponent<Renderer>();
                highlight = hit.transform;
            } else {
                postScript.OutlinedObject = null;
                highlight = null;
            }
        }

    }
    
    public void IsolatePart()
    {
        if (selectPost.SelectedObject)
        {
            isolateCheck = !isolateCheck;

            uiManagerScript.SwitchSprite(isolateCheck, image);

            foreach (Transform child in selectPost.SelectedObject.transform.root)
            {
                Debug.Log(child);
                if (child.name == selectPost.SelectedObject.transform.name)
                {
                    //child.gameObject.SetActive(isolateCheck);
                    originObject = child.gameObject;
                    child.gameObject.SetActive(isolateCheck);
                }
                else
                {
                    child.gameObject.SetActive(!isolateCheck);
                }

            }
            originObject.SetActive(true);

            // if (isolateCheck)
            // {
            //     selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
            // }
            // else
            // {
            //     selectedObj.gameObject.layer = LayerMask.NameToLayer("Seeable");
            // }
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
        veinCheck = !veinCheck;
        partList.useExclude = veinCheck;
        partList.ResetNameListOnButton();

        if (veinCheck)
        {
            ToggleCollider(coronarySideModel, veinCheck);
            ResetDict();
            dropdownPanel.SetBool("IsOpen", true);

            foreach (Transform child in coronarySideModel.transform)
            {
                if (child.tag != "Vein")
                {

                    //ChangeMaterial(child.gameObject);
                    child.gameObject.SetActive(false);
                }

                if (child.tag == "Hidden")
                {
                    child.gameObject.SetActive(true);
                }
            }

            if (selectedObj != null)
            {
                selectedObj = null;
            }
        }
        else
        {
            ToggleCollider(coronarySideModel, veinCheck);
            ResetDict();
            dropdownPanel.SetBool("IsOpen", false);

            foreach (Transform child in coronarySideModel.transform)
            {
                if (child.tag != "Vein")
                {
                    child.gameObject.SetActive(true);
                }

                if (child.tag == "Hidden")
                {
                    child.gameObject.SetActive(false);
                }
            }

            if (selectedObj != null)
            {
                selectedObj = null;
            }
        }
    }

    public void ShowSlider()
    {
        if (veinCheck)
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
            //UpdateSliderValue();
            

            if (selectedObj != null)
            {
                selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
                selectedObj = null;
                Destroy(selectedInstantiatedObj);
            }
            
        }
        else
        {
            //veinCheck = false;
     
            

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
