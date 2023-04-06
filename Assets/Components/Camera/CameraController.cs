using System.Collections;
using UnityEngine;

/// <summary>
/// This component can be attached to a camera itself or to another game object.
/// In the latter case, the script gets reference to Camera.main.
/// 
/// Slides in the introduction, for example, will each have a CameraController
/// but will not be cameras themselves.
/// </summary>
public class CameraController : MonoBehaviour
{
    public CameraState cameraState;

    [Header("Position / Orientation")]
    public Vector3 targetPosition;
    public Vector3 targetRotation;
    public float moveTime = 1;

    [Header("Zoom")]
    public bool canZoom = false;
    public float minZoom = 1f;
    public float maxZoom = 10f;
    public float zoomSpeed = 1f;
    public Transform zoomTarget;
    private float initialDistance;

    [Header("Background Color")]
    public Color targetColor = Color.white;
    public float colorTransitionTime = 1;

    // [Header("Orbit Options")]
    // public bool canPanX;
    // public bool canPanY;

    // The actual camera object to move
    [HideInInspector] public Transform cameraTransform;

    private new Camera camera;
    private bool componentIsCamera;

    private void AssignCameraReferences()
    {
        // Check if this object is the camera
        if (TryGetComponent(out camera))
        {
            // Debug.Log(transform.name + " IS a camera");
            componentIsCamera = true;
        }
        else
        {
            // Debug.Log(transform.name + " IS NOT a camera");
            camera = Camera.main;
            componentIsCamera = false;
        }

        cameraTransform = camera.transform;
    }

    private void Awake()
    {
        AssignCameraReferences();
    }

    private void Start()
    {
        // Slides in the introduction will not be cameras themselves
        // The SlideManager will handle triggering camera movement in that case
        if (componentIsCamera)
        {
            // Place the camera based on state parameters
            if (cameraState)
            {
                cameraTransform.position = cameraState.position;
                cameraTransform.rotation = cameraState.rotation;
                cameraTransform.localScale = cameraState.scale;
            }

            // Move the camera to this controller's target position and rotation
            StartCoroutine(MoveTo(targetPosition, targetRotation, moveTime));
            StartCoroutine(ChangeBackgroundColor(targetColor, colorTransitionTime));
        }

        if (zoomTarget)
        {
            initialDistance = Vector3.Distance(cameraTransform.position, zoomTarget.position);
        }
    }

    private void LateUpdate()
    {
        if (!zoomTarget || !canZoom) return;

        // Get the input from the mouse wheel
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");

        // Calculate the new distance between camera and target
        float newDistance = Mathf.Clamp(initialDistance - zoomInput * zoomSpeed, minZoom, maxZoom);

        // Calculate the new camera position
        Vector3 direction = (cameraTransform.position - zoomTarget.position).normalized;
        cameraTransform.position = zoomTarget.position + direction * newDistance;
    }

    private void UpdateCameraState()
    {
        if (cameraState) cameraState.SetState(camera);
    }

    private IEnumerator MoveTo(Vector3 targetPosition, Vector3 targetRotation, float moveTime)
    {
        Vector3 startPosition = cameraTransform.position;
        Quaternion startRotation = cameraTransform.rotation;
        Quaternion finalRotation = Quaternion.Euler(targetRotation);
        float time = 0;

        while (time < moveTime)
        {
            time += Time.deltaTime;
            float t = time / moveTime;
            t = t * t * (3f - 2f * t);
            cameraTransform.position = Vector3.Slerp(startPosition, targetPosition, t);
            cameraTransform.rotation = Quaternion.Slerp(startRotation, finalRotation, t);
            UpdateCameraState();
            yield return null;
        }

        cameraTransform.position = targetPosition;
        cameraTransform.rotation = finalRotation;
    }

    private IEnumerator ChangeBackgroundColor(Color targetColor, float transitionTime)
    {
        Color startColor = camera.backgroundColor;
        float time = 0;

        while (time < transitionTime)
        {
            time += Time.deltaTime;
            camera.backgroundColor = Color.Lerp(startColor, targetColor, time / transitionTime);
            yield return null;
        }

        camera.backgroundColor = targetColor;
    }

    // Called by SlideManager
    public void TriggerCameraMovement(bool useCameraState = false)
    {
        // Cancel any coroutines initiated by this instance
        StopAllCoroutines();

        // Synchronize the camera with the CameraState parameters
        if (useCameraState)
        {
            cameraTransform.position = cameraState.position;
            cameraTransform.rotation = cameraState.rotation;
            cameraTransform.localScale = cameraState.scale;
            if (camera) camera.backgroundColor = cameraState.backgroundColor;
        }

        // Move the camera to this controller's target position and rotation
        StartCoroutine(MoveTo(targetPosition, targetRotation, moveTime));
        StartCoroutine(ChangeBackgroundColor(targetColor, colorTransitionTime));
    }

    // Can be called from a button in the inspector in edit mode
    public void SetCameraImmediately()
    {
        // Make sure we have the camera references
        AssignCameraReferences();

        // Set the actual camera's parameters
        cameraTransform.position = targetPosition;
        cameraTransform.rotation = Quaternion.Euler(targetRotation);
        if (camera) camera.backgroundColor = targetColor;

        // Synchronize the CameraState with the updated parameters
        UpdateCameraState();
    }
}
