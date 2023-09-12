using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class RayCast : MonoBehaviour
{
    public Material highlightMaterial;
    public Material selectionMaterial;
    public GameObject model;
    public Button isolateBtn;

    private bool clickStatus;

    private GameObject selectedPart;
    private Material orignialMaterial;
    private Transform highlight;
    private Transform selection;
    private RaycastHit raycastHit;

    // Start is called before the first frame update
    void Start()
    {
        //selection = null;
    }

    public void IsolatePart()
    {
        foreach (Transform child in model.transform)
        {
            if (child.name == selectedPart.name)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
                      
        }
    }

    private void RestorePart()
    {
        if (selection != null)
        {
            foreach (Transform child in model.transform)
            {
                child.gameObject.SetActive(true);
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
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            if (highlight.CompareTag("Selectable") && highlight != selection)
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


    private void SelectPart()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //HighlightPart();
        if (selection != null && Input.GetMouseButtonUp(0))
        {
            selection.GetComponent<MeshRenderer>().material = orignialMaterial;
            Debug.Log("NULLED");
        }

        if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out raycastHit))
            {
                selection = raycastHit.transform;
                selectedPart = raycastHit.transform.gameObject;
                orignialMaterial = selection.GetComponent<MeshRenderer>().material;
                selection.GetComponent<MeshRenderer>().material = selectionMaterial;
            }
            else
            {
                selection = null;
            }
        }
        for (int i = 0; i >= 1 ; i++)
        {
            if(i == 0)
            {
                isolateBtn.onClick.AddListener(IsolatePart);
            }
            if(i == 1)
            {
                isolateBtn.onClick.AddListener(RestorePart);
            }
        }




        // Transforms Object when click
        //if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        //{
        //    if(selection != null)
        //    {
        //        Debug.Log(orignialMaterial);
        //        selection.GetComponent<MeshRenderer>().material = orignialMaterial;
        //        selection = null;
        //    }

        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray,out raycastHit))
        //    {
        //        selection = raycastHit.transform;
        //        //selectedPart = raycastHit.transform.gameObject;
        //        Debug.Log(selection);
        //        Debug.Log(selection.GetComponent<MeshRenderer>().material);
        //        if (selection.CompareTag("Selectable"))
        //        {
        //            Debug.Log(selectionMaterial);            
        //            selection.GetComponent<MeshRenderer>().material = selectionMaterial;
        //            Debug.Log("Changed to Select Mate");    

        //        }
        //        else
        //        {
        //            selection = null;

        //        }
        //    }
        //}
    }
}
