using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEditor.IMGUI.Controls;
using Unity.VisualScripting;

public class MainRotate : MonoBehaviour
{

    public Transform rotationCenter;
    public float angularSpeed = 2f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateAchievement();
    }

    public void RotateAchievement()
    {
        gameObject.transform.RotateAround(rotationCenter.position, new Vector3(0, 0, 1), angularSpeed * Time.deltaTime);
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
