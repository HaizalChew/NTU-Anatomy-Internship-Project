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
    [SerializeField] Material orignialMaterial;
    [SerializeField] Transform selectedObj;
    [SerializeField] LayerMask selectableLayerMask;
    [SerializeField] GameObject selectedInstantiatedObj;
    [SerializeField] GameObject model;
    [SerializeField] CameraControls camControl;
    [SerializeField] Button isolateBtn;

    private Transform highlight;
    private RaycastHit raycastHit;
    public bool isolateCheck;

    // Update is called once per frame
    void Update()
    {
        if(isolateCheck == false)
        {
            if (Input.GetMouseButtonDown(1))
            {
                SelectPart();
            }
            
            HighlightPart();
            isolateBtn.GetComponentInChildren<TextMeshProUGUI>().text = "Isolate";
        }
        else
        {
            isolateBtn.GetComponentInChildren<TextMeshProUGUI>().text = "DeIsolate";
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
            highlight.GetComponent<MeshRenderer>().material = orignialMaterial;
            highlight = null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Hightlight Object when hovered
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit, Mathf.Infinity, selectableLayerMask))
        {
            highlight = raycastHit.transform;
            if (highlight != selectedObj)
            {
                if (highlight.GetComponent<MeshRenderer>().material != highlightMaterial)
                {
                    orignialMaterial = highlight.GetComponent<MeshRenderer>().material;
                    highlight.GetComponent<MeshRenderer>().material = highlightMaterial;
                }
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
        
}
