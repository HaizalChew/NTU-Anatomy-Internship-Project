using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(Camera))]
public class CameraControls : MonoBehaviour
{
    // Control camera focus
    [SerializeField] Transform focus;
    [SerializeField] public Transform target;
    [SerializeField] Transform orientation;
    [SerializeField, Range(0.1f, 20f)] float distance = 5f;
    [SerializeField, Min(0f)] float focusRadius = 1f;
    [SerializeField, Range(0f, 1f)] float focusCentering = 0.5f;
    [SerializeField] public bool stopRecentering;

    // Control orbit rotation expressed in degrees
    [SerializeField, Range(1f, 360f)] float rotationSpeed = 90f;
    [SerializeField, Range(-89f, 89f)] float minVerticalAngle = -30f, maxVerticalAngle = 60f;
    [SerializeField] InputActionReference orbitLookInput, orbitUnlockInput, panUnlockInput, focusInput;

    // Control camera zoom
    [SerializeField] InputActionReference zoomScrollInput;
    [SerializeField] LayerMask ignoreMask;
    [SerializeField] float zoomSpeed = 1f;
    [SerializeField] Slider zoomSlider;
    [SerializeField] SelectableHandler selectableHandler;

    // Set orbit angles
    Vector2 orbitAngles = new Vector2(45f, 0f);

    //Vector3 focusPoint;
    Vector3 newMousePoint;

    private void Awake()
    {
        orbitAngles = transform.localRotation.eulerAngles;
        //focusPoint = focus.position;
        transform.localRotation = Quaternion.Euler(orbitAngles);

        zoomSlider.minValue = 0.1f;
        zoomSlider.maxValue = 20f;
    }

    private void OnEnable()
    {
        focusInput.action.performed += ctx => ActivateRecenteringOnButton();
    }

    private void OnDisable()
    {
        focusInput.action.performed -= ctx => ActivateRecenteringOnButton();
    }

    void OnValidate()
    {
        // Validate the min does not go beyond the max, this will run in the inspector
        if (maxVerticalAngle < minVerticalAngle)
        {
            maxVerticalAngle = minVerticalAngle;
        }
    }

    private void LateUpdate()
    {
        // Updates camera position and rotation
        if (!stopRecentering)
        {
            UpdateFocusPoint();
        }
        

        Quaternion lookRotation;
        if (OrbitCamera())
        {
            ConstrainAnglesInOrbit();
            lookRotation = Quaternion.Euler(orbitAngles);
        }
        else
        {
            lookRotation = transform.localRotation;
        }

        // Zoom the camera
        Ray raycast = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(raycast, out RaycastHit hitInfo, Mathf.Infinity, ignoreMask))
        {
            ZoomCamera();
        }


        // Enable camera panning
        Pan();

        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focus.transform.position - lookDirection * distance;
        transform.SetPositionAndRotation(lookPosition, lookRotation);

        

    }


    // This will update the focus pont everytime the model moves/changes
    // Applies easing in whenever it moves to prevent sharp snapping
    void UpdateFocusPoint()
    {
        Vector3 targetPoint = target.position;
        float distance = Vector3.Distance(targetPoint, focus.transform.position);
        float t = 1f;
        if (distance > 0.01f && focusCentering > 0f)
        {
            t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
        }
        if (distance > focusRadius)
        {
            t = Mathf.Min(t, focusRadius / distance);
        }
        focus.transform.position = Vector3.Lerp(targetPoint, focus.transform.position, t);
    }

    public void ActivateRecentering(Transform targetPos)
    {
        target = targetPos;
        stopRecentering = false;

        if (distance >= 2f)
        {
            distance = 2f;
        }
        
    }

    public void ActivateRecenteringOnButton()
    {
        stopRecentering = false;
        if (distance >= 2f)
        {
            distance = 2f;
        }
        
    }
    
    // This will orbit the camera around the focus
    bool OrbitCamera()
    {
        Vector2 input = new Vector2(-orbitLookInput.action.ReadValue<Vector2>().y, orbitLookInput.action.ReadValue<Vector2>().x);

        const float e = 0.001f;
        if ((input.x < -e || input.x > e || input.y < -e || input.y > e) && ((orbitUnlockInput.action.IsPressed() && !panUnlockInput.action.IsPressed()) || selectableHandler.buttonPressed) && Input.GetMouseButton(0))
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
            return true;
        }

        return false;
    }

    // This will constrain the camera in the assigned boundaries
    void ConstrainAnglesInOrbit()
    {
        orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

        if (orbitAngles.y < 0f)
        {
            orbitAngles.y += 360f;
        }
        else if (orbitAngles.y >= 360f)
        {
            orbitAngles.y -= 360f;
        }
    }

    // This will control the zoom function of the camera
    void ZoomCamera()
    {
        float input = zoomScrollInput.action.ReadValue<Vector2>().y;

        const float e = 0.001f;
        if (input > -e)
        {
            distance -= zoomSpeed * Mathf.Abs(input / 120f);
        }
        else if (input < e)
        {
            distance += zoomSpeed * Mathf.Abs(input / 120f);
        }

        distance = Mathf.Clamp(distance, .1f, 20f);
        zoomSlider.value = distance;
    }

    public void SetZoomCamera(float value = 0)
    {
        if (value < 0 || value > 0)
        {
            distance += value;
            distance = Mathf.Clamp(distance, .1f, 20f);
        }
        else
        {
            distance = zoomSlider.value;
        }
    }

    // This will control camera panning
    void Pan()
    {
        if (panUnlockInput.action.IsPressed() && orbitUnlockInput.action.IsPressed())
        {
            if (Input.GetMouseButtonDown(0))
            {
                newMousePoint = Input.mousePosition;
                stopRecentering = true;
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 direction = (Input.mousePosition - newMousePoint) * 0.001f;

                Vector3 viewDir = focus.position - transform.position;
                orientation.forward = viewDir.normalized;

                Vector3 movement = orientation.up * direction.y + orientation.right * direction.x;

                focus.transform.Translate(movement);

                newMousePoint = Input.mousePosition;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(2))
            {
                newMousePoint = Input.mousePosition;
                stopRecentering = true;
            }

            if (Input.GetMouseButton(2))
            {
                Vector3 direction = (Input.mousePosition - newMousePoint) * 0.001f;

                Vector3 viewDir = focus.position - transform.position;
                orientation.forward = viewDir.normalized;

                Vector3 movement = orientation.up * direction.y + orientation.right * direction.x;

                focus.transform.Translate(movement);

                newMousePoint = Input.mousePosition;
            }
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(focus.transform.position, .5f);
    }
}
