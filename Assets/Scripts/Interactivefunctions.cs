using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class Interactivefunctions : MonoBehaviour
{
    public Material hoverMaterial;
    public Material clickedMaterial;

    private bool hoverStatus;
    private Transform highlight;
    private Material originalMaterial;
    private RaycastHit raycastHit;


    // Start is called before the first frame update
    void Start()
    {
        hoverStatus = false;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out raycastHit))
        {
            highlight = raycastHit.transform;
            highlight.GetComponent<MeshRenderer>().material = originalMaterial;
            hoverStatus = true;
            if (highlight.CompareTag("Selectable") && (hoverStatus == true))
            {
                highlight.GetComponent<MeshRenderer>().material = hoverMaterial;

            }


        }
    }
}
