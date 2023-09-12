using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasicInteractions : MonoBehaviour
{
    [SerializeField] Material selectedMat;
    [SerializeField] Material highlightMaterial;
    [SerializeField] Material orignialMaterial;
    [SerializeField] Transform selectedObj;
    [SerializeField] LayerMask selectableLayerMask;
    [SerializeField] GameObject selectedInstantiatedObj;

    private Transform highlight;
    private RaycastHit raycastHit;

    // Update is called once per frame
    void Update()
    {
        HighlightPart();
        SelectPart();   
    }

    void SelectPart()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (selectedObj == null)
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
                }
            }
            else
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
                }
            }

            
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
}
