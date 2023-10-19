using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderIsolateCamera : MonoBehaviour
{
    [SerializeField] Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }
}
