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

    [Header("Background Color")]
    public Color targetColor = Color.white;
    public float colorTransitionTime = 1;

    // The actual camera object to move
    [HideInInspector] public Transform cameraTransform;

    private new Camera camera;
    private bool componentIsCamera;

    public static event System.Action<Vector3, Quaternion> OnCameraMovementComplete;

    private void AssignCameraReferences()
    {
        // Check if this object is the camera
        if (TryGetComponent(out camera))
        {
            componentIsCamera = true;
        }
        else
        {
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
        // Slides in the Introduction will not be cameras themselves
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
        UpdateCameraState();

        // Alert other scripts (e.g. SlideControllers) that the camera has finished moving
        OnCameraMovementComplete?.Invoke(targetPosition, finalRotation);
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
