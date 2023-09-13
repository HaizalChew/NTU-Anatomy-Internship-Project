using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HoverText : MonoBehaviour
{
    [SerializeField] LayerMask selectableLayerMask;
    [SerializeField] private GameObject toolTipUi = null;
    [SerializeField] private TextMeshProUGUI partText = null;

    public static HoverText instance = null;
    public static HoverText Instance => instance;

    private RaycastHit raycastHit;
    private Transform textName;
    private bool toolTipCheck;
    [SerializeField] private RectTransform transformToolTip = null;


    private Vector3 GetPos_2D(Vector3 pos) => pos + PosTowardCenter(pos);
    private Vector3 GetPos_3D(Vector3 pos)
    { 
        Vector3 screenPos = Camera.main.WorldToScreenPoint(pos);
        return screenPos + PosTowardCenter(screenPos);
    }

    private Vector3 centerScreenPos = new Vector3(Screen.width / 2, Screen.height / 2);

    private Vector3 PosTowardCenter(Vector3 pos) => (centerScreenPos - pos).normalized * transformToolTip.rect.width;


    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            DestroyImmediate(this);
    }

    public void SetToolTipAtPosWithMsg(Vector3 pos, string message, bool isTwoDimension = true)
    {
        toolTipUi.gameObject.SetActive(true);

        partText.text = message;

        transformToolTip.position = isTwoDimension ? GetPos_2D(pos) : GetPos_3D(pos);
    }

    public void DeactivateToolTip() => toolTipUi.SetActive(false);


    // Update is called once per frame
    void Update()
    {
        //toolTipUi.transform.position = Input.mousePosition;
        //OnMouseOver();
        //if (toolTipCheck == true)
        //{
        //    toolTipUi.gameObject.SetActive(toolTipCheck);
        //}
        //else
        //{
        //    toolTipUi.gameObject.SetActive(toolTipCheck);
        //}
    }

    //private void OnMouseEnter()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, selectableLayerMask) && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        textName = raycastHit.transform;
    //        partText.GetComponent<TextMeshProUGUI>().text = textName.name;
    //        toolTipCheck = true;
    //    }
    //}

    //private void OnMouseExit()
    //{
    //    toolTipCheck = false;
    //}

    //void ShowPartName()
    //{
    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    toolTipUi.gameObject.SetActive(toolTipCheck);
    //    if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, selectableLayerMask) && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        toolTipUi.transform.position = Input.mousePosition;
    //        toolTipCheck = true;
    //        textName = raycastHit.transform;
    //        partText.GetComponent<TextMeshProUGUI>().text = textName.name;
    //    }
    //    else
    //    {
    //        toolTipCheck = false;
    //    }
    //}

}
