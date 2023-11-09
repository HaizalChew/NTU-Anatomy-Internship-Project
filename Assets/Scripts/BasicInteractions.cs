using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem.HID;

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
    [SerializeField] GameObject toggleAngioViewButton;

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
                RaycastHit[] hits;
                bool hitSelectable = Physics.Raycast(ray, out var hit) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable");
                if (hitSelectable) {
                    hits = Physics.RaycastAll(ray, Mathf.Infinity);
                    RaycastHit[] shortLongHit = GetShortLongHit(hits);
                    if (shortLongHit[0].transform.GetComponent<MeshRenderer>().material.GetFloat("_Mode") != 3)
                    {
                        selectPost.enabled = true;
                        selectPost.SelectedObject = shortLongHit[0].transform.GetComponentsInChildren<Renderer>();
                        highlight = shortLongHit[0].transform;
                        selectedObj = shortLongHit[0].transform;
                    }
                    else
                    {
                        selectPost.enabled = true;
                        selectPost.SelectedObject = shortLongHit[1].transform.GetComponentsInChildren<Renderer>();
                        highlight = shortLongHit[1].transform;
                        selectedObj = shortLongHit[1].transform;
                    }

                    camControl.ActivateRecentering(hit.transform);
                    AudioManager.instance?.PlaySoundEffect(0);
                    objectSelected = true;

                } else {
                    selectPost.enabled = false;
                    selectPost.SelectedObject = null;
                    selectedObj = null;
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
            RaycastHit[] hits;
            if (!EventSystem.current.IsPointerOverGameObject() && (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable")))
            {
                hits = Physics.RaycastAll(ray, Mathf.Infinity);
                RaycastHit[] shortLongHit = GetShortLongHit(hits);
                if (shortLongHit[0].transform.GetComponent<MeshRenderer>().material.GetFloat("_Mode") != 3)
                {
                    postScript.enabled = true;
                    postScript.OutlinedObject = shortLongHit[0].transform.GetComponentsInChildren<Renderer>();
                    highlight = shortLongHit[0].transform;
                }
                else
                {
                    postScript.enabled = true;
                    postScript.OutlinedObject = shortLongHit[1].transform.GetComponentsInChildren<Renderer>();
                    highlight = shortLongHit[1].transform;
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
            RaycastHit[] hits;
            if (!EventSystem.current.IsPointerOverGameObject() && (Physics.Raycast(ray, out var hit, Mathf.Infinity) && hit.transform.gameObject.layer == LayerMask.NameToLayer("Selectable")) && hit.transform.name != postScript.transform.name)
            {
                hits = Physics.RaycastAll(ray, Mathf.Infinity);
                RaycastHit[] shortLongHit = GetShortLongHit(hits);
                if (shortLongHit[0].transform.GetComponent<MeshRenderer>().material.GetFloat("_Mode") != 3)
                {
                    postScript.enabled = true;
                    postScript.OutlinedObject = shortLongHit[0].transform.GetComponentsInChildren<Renderer>();
                    highlight = shortLongHit[0].transform;
                }
                else
                {
                    postScript.enabled = true;
                    postScript.OutlinedObject = shortLongHit[1].transform.GetComponentsInChildren<Renderer>();
                    highlight = shortLongHit[1].transform;
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

        toggleAngioViewButton?.SetActive(!isolateCheck);
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

    private RaycastHit[] GetShortLongHit(RaycastHit[] hits)
    {
        float highestDistance = 0f;
        float shortestDistance = 0f;
        RaycastHit[] bothInfo = new RaycastHit[2];

        for (int i = 0; i < hits.Length; i++)
        {
            RaycastHit hit = hits[i];
            float distance = hit.distance;
            if (i == 0)
            {
                shortestDistance = hit.distance;
            }

            if (distance <= shortestDistance)
            {
                shortestDistance = distance;
                bothInfo[0] = hit;
            }

            if (distance > highestDistance)
            {
                highestDistance = distance;
                bothInfo[1] = hit;
            }
        }
        return bothInfo;
    }

    public void OnMouseEnter() { showCheck = true; }
    public void OnMouseExit() { showCheck = false; }
}
