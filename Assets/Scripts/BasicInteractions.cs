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

    public bool objectSelected;

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

    }

    public void SelectPart(Transform selected = null)
    {
        if(selected == null){
            if (Input.GetMouseButtonDown(1))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                bool hitSelectable = Physics.Raycast(ray, out var hit) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable");
                if (hitSelectable) {
                    selectPost.enabled = true;

                    if (hit.transform.childCount > 0)
                    {
                        selectPost.SelectedObject = hit.transform.GetComponentsInChildren<Renderer>();
                    }
                    else
                    {
                        Renderer[] renderers = { hit.transform.GetComponent<Renderer>() };
                        selectPost.SelectedObject = renderers;
                    }
                    
                    camControl.ActivateRecentering(hit.transform);
                    AudioManager.instance?.PlaySoundEffect(0);
                    objectSelected = true;
                    highlight = hit.transform;

                    selectedObj = hit.transform;
                } else {
                    selectPost.enabled = false;
                    selectPost.SelectedObject = null;
                    objectSelected = false;
                }
            }
        }
        else{
             selectPost.GetComponent<SelectPost>().enabled = true;

            if (selected.transform.childCount > 0)
            {
                selectPost.SelectedObject = selected.transform.GetComponentsInChildren<Renderer>();
            }
            else
            {
                Renderer[] renderers = { selected.transform.GetComponent<Renderer>() };
                selectPost.SelectedObject = renderers;
            }
            camControl.ActivateRecentering(selected);
            AudioManager.instance?.PlaySoundEffect(0);
            objectSelected = true;
            highlight = selected;
            selectedObj = selected;
        }
    }

    public void HighlightPart()
    {

        if (objectSelected == false)
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable")))
            {
                postScript.enabled = true;

                if (hit.transform.childCount > 0)
                {        
                    postScript.OutlinedObject = hit.transform.GetComponentsInChildren<Renderer>();
                    highlight = hit.transform;
                }
                else
                {
                    Renderer[] renderers = { hit.transform.GetComponent<Renderer>() };
                    postScript.OutlinedObject = renderers;
                    highlight = hit.transform;
                }

            }
            else
            {
                postScript.enabled = false;
                postScript.OutlinedObject = null;
                highlight = null;
            }
        }
        else
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!EventSystem.current.IsPointerOverGameObject() && (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable")) && hit.transform.name != postScript.transform.name)
            {
                if (hit.transform.childCount > 0)
                {
                    postScript.OutlinedObject = hit.transform.GetComponentsInChildren<Renderer>();
                    highlight = hit.transform;
                }
                else
                {
                    Renderer[] renderers = { hit.transform.GetComponent<Renderer>() };
                    postScript.OutlinedObject = renderers;
                    highlight = hit.transform;
                }

            }
            else
            {
                postScript.OutlinedObject = null;
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

            foreach (Transform child in model.transform)
            {
                child.gameObject.SetActive(!isolateCheck);
            }

            selectedObj.gameObject.SetActive(true);
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


    //GameObject ChangeMaterial(GameObject bodyPart)
    //{
    //    GameObject changedMat = Instantiate(bodyPart, bodyPart.transform.position, bodyPart.transform.rotation, cloneContainer.transform);
    //    Destroy(changedMat.GetComponent<Rigidbody>());
    //    Destroy(changedMat.GetComponent<MeshCollider>());
    //    Renderer targetRend = changedMat.GetComponent<Renderer>();
    //    Renderer srcRend = bodyPart.GetComponent<Renderer>();
    //    Material newMat = Instantiate(transMat);
    //    newMat.mainTexture = srcRend.material.mainTexture;
    //    targetRend.material = newMat;
    //    return (changedMat);
    //}

    public void ToggleVeinTransparent()
    {   
        veinCheck = !veinCheck;
        partList.useExclude = veinCheck;
        partList.ResetNameListOnButton();

        if (veinCheck)
        {
            //ToggleCollider(coronarySideModel, veinCheck);
            dropdownPanel.SetBool("IsOpen", true);

            ToggleAngioView(false);
        }
        else
        {
            //ToggleCollider(coronarySideModel, veinCheck);
            dropdownPanel.SetBool("IsOpen", false);

            ToggleAngioView(true);
        }

        void ToggleAngioView(bool toggle)
        {
            foreach (Transform child in coronarySideModel.transform)
            {
                if (child.childCount > 0)
                {
                    for (int i = 0; i < child.childCount; i++)
                    {
                        Transform childChild = child.GetChild(i);

                        if (childChild.tag != "Vein")
                        {
                            childChild.GetComponent<Renderer>().enabled = toggle;
                            childChild.GetComponent<Collider>().enabled = toggle;
                        }

                        if (childChild.tag == "Hidden")
                        {
                            childChild.GetComponent<Renderer>().enabled = !toggle;
                            childChild.GetComponent<Collider>().enabled = !toggle;
                        }
                    }
                }

                if (child.tag != "Vein")
                {
                    child.GetComponent<Renderer>().enabled = toggle;
                    child.GetComponent<Collider>().enabled = toggle;
                }

                if (child.tag == "Hidden")
                {
                    child.GetComponent<Renderer>().enabled = !toggle;
                    child.GetComponent<Collider>().enabled = !toggle;
                }
            }

            if (selectedObj != null)
            {
                selectedObj = null;
                selectPost.enabled = false;
                selectPost.SelectedObject = null;
                objectSelected = false;
            }

        }
    }

    //public void ShowSlider()
    //{
    //    if (veinCheck)
    //    {
    //        renderingSlider.gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        renderingSlider.gameObject.SetActive(false);
    //    }
    //}

    //public void ToggleCollider(GameObject model, bool viewMode)
    //{
    //    foreach (Transform child in model.transform)
    //    {
    //        if (child.tag != "Vein")
    //        {
    //            child.GetComponent<Collider>().enabled = !viewMode;
    //        }

    //        if (child.tag == "Hidden")
    //        {
    //            child.GetComponent<Collider>().enabled = viewMode;
    //        }
    //    }
    //}
    //public void ActivateViewMode()
    //{
    //    viewMode = !viewMode;
        

    //    if (viewMode)
    //    {
    //        //UpdateSliderValue();
            

    //        if (selectedObj != null)
    //        {
    //            selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
    //            selectedObj = null;
    //            Destroy(selectedInstantiatedObj);
    //        }
            
    //    }
    //    else
    //    {
    //        //veinCheck = false;
     
            

    //        if (selectedObj != null)
    //        {
    //            selectedObj.gameObject.layer = LayerMask.NameToLayer("Selectable");
    //            selectedObj = null;
    //            Destroy(selectedInstantiatedObj);
    //        }
    //    }

    //}

    //private bool ColorDifferenceTreshold(Color color1, Color color2)
    //{
    //    float treshold = .5f;
    //    float distance = Mathf.Sqrt(Mathf.Pow(color2.r - color1.r, 2) + Mathf.Pow(color2.g - color1.g, 2) + Mathf.Pow(color2.b - color1.b, 2));

    //    if (distance < treshold)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //private Renderer[] GetSpecificComponentsInChildren(Transform parent)
    //{
    //    List<Renderer> highlightObjectChildren = new List<Renderer>();
    //    foreach (Renderer child in parent.GetComponentsInChildren<Renderer>())
    //    {
    //        if (!child.gameObject.name.Contains(parent.name))
    //        {
    //            highlightObjectChildren.Add(child);
    //        }
    //    }

    //    return highlightObjectChildren.ToArray();
    //}

    public void OnMouseEnter() { showCheck = true; }
    public void OnMouseExit() { showCheck = false; }
}
