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
    [SerializeField] GameObject selectedInstantiatedObj;
    [SerializeField] GameObject model;
    [SerializeField] CameraControls camControl;
    [SerializeField] Button isolateBtn;

    [SerializeField] GameObject coronaryModel;
    [SerializeField] Slider renderingSlider;

    private Vector3 toolTipPos;
    private Vector2 mousePos;
    private Vector3 offset = new Vector3(16, 16, 0);
    private Rect objRect;
    private bool showCheck;

    private float sliderValue;

    public Transform highlight;
    private RaycastHit raycastHit;
    public bool isolateCheck;
    public bool veinCheck;
    public bool viewMode;


    [SerializeField] UiManger uiMangerScript;
    [SerializeField] Image image;
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
            if (highlight != null)
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

    public void ShowVein()
    {
        //veinCheck = !veinCheck;
        if (veinCheck)
        {
            foreach (Transform child in coronaryModel.transform)
            {
                if (child.tag != "Vein")
                {
                    ////child.gameObject.SetActive(false);
                    ////Destroy(selectedInstantiatedObj);
                    ////selectedInstantiatedObj = null;
                    ////selectedObj = null;
                    Debug.Log(child);
                    child.GetComponent<MeshRenderer>().material.SetFloat("_Mode", 3);
                    //Turn on Alpha Blending
                    child.GetComponent<MeshRenderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    child.GetComponent<MeshRenderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    child.GetComponent<MeshRenderer>().material.EnableKeyword("_ALPHABLEND_ON");
                    child.GetComponent<MeshRenderer>().material.renderQueue = 3000;
                    Color color = child.GetComponent<MeshRenderer>().material.color;
                    color.a = sliderValue;
                    child.GetComponent<MeshRenderer>().material.color = color;
                    Debug.Log("vein");
                }
            }
        }
        else
        {
            //foreach (Transform child in coronaryModel.transform)
            //{
            //    if (child.tag != "Vein")
            //    {
            //        //child.gameObject.SetActive(true);
            //        Color color = child.GetComponent<Renderer>().material.color;
            //        color.a = 0;
            //        child.GetComponent<Renderer>().material.color = color;
            //        Debug.Log("yes");
            //    }
            //}
        }
    }

    public void ActivateViewMode()
    {
        viewMode = !viewMode;
        if (viewMode)
        {
            foreach (Transform child in coronaryModel.transform)
            {
                if (child.tag != "Vein")
                {
                    child.GetComponent<Collider>().enabled = false;
                }
            }
        }
        else
        {
            foreach (Transform child in coronaryModel.transform)
            {
                if (child.tag != "Vein")
                {
                    child.GetComponent<Collider>().enabled = true;
                }
            }
        }

    }
    public void OnMouseEnter() { showCheck = true; }
    public void OnMouseExit() { showCheck = false; }
}
