using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (HoverText.Instance)
        {
            HoverText.Instance.SetToolTipAtPosWithMsg(transform.position, gameObject.name);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (HoverText.Instance)
        {
            HoverText.Instance.DeactivateToolTip();
        }
    }

    private void OnMouseEnter()
    {
        if (HoverText.Instance)
        {
            HoverText.Instance.SetToolTipAtPosWithMsg(transform.position, gameObject.name, false);
        }
    }

    private void OnMouseExit()
    {
        if (HoverText.Instance)
        {
            HoverText.Instance.DeactivateToolTip();
        }
    }
}
