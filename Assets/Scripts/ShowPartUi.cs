using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class ShowPartUi : MonoBehaviour
{
    //SerializeField] GameObject toolTipCanvas;
    //[SerializeField] GameObject toolTipPrefab;
    [SerializeField] GameObject toolTipUI;

    private Vector3 toolTipPos;
    private Vector2 mousePos;
    private Vector3 offset = new Vector3 (16, 16, 0);
    private Rect objRect;
    private bool showCheck;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(toolTipPrefab, toolTipCanvas.transform);
        //toolTipCanvas.transform.position = new Vector3(1,1,1);
        mousePos = new Vector2 (0,0);
        showCheck = false;
        objRect = new Rect(0, 0, 300, 100);
        Debug.Log("START");
        
      
    }

    public void OnMouseEnter() { showCheck = true; Debug.Log("Enter"); }
    public void OnMouseExit() { showCheck = false; }
    private void OnGUI()
    {
        if (showCheck)
        {
            if (BasicInteractions.highlight != null)
            {
                Debug.Log("yes");
                //mousePos = Input.mousePosition + offset;
                //objRect.x = mousePos.x;

                //objRect.y = Mathf.Abs(mousePos.y - Camera.main.pixelHeight);
                //GUI.skin.label.fontSize = GUI.skin.box.fontSize = GUI.skin.button.fontSize = 40;
                //GUI.Label(objRect, gameObject.name);
                toolTipUI.gameObject.SetActive(true);
                Debug.Log("active");

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
