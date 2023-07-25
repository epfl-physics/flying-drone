using UnityEngine;
using UnityEngine.EventSystems;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;

    [Header("Parameters")]
    public float panSpeed = 1.0f;
    public float minPolarAngle = 0.0f;
    public float maxPolarAngle = 90.0f;
    public float minAzimuthalAngle = -180.0f;
    public float maxAzimuthalAngle = 180.0f;
    public bool clampAzimuthalAngle = false;

    [Header("Options")]
    public bool canOrbit = true;

    private bool _canOrbit;
    private bool waitForCameraController;

    private float polarAngle;
    private float azimuthalAngle;
    private float currentDistance;
    private bool isDragging;

    public static event System.Action OnStartOrbit;
    public static event System.Action OnEndOrbit;

    private void OnEnable()
    {
        CameraTagger.OnMainCameraChanged += HandleMainCameraChanged;
        if (TryGetComponent<CameraController>(out var controller))
        {
            // Debug.Log(transform.name + " will respond to CameraController");
            waitForCameraController = true;
            CameraController.OnCameraMovementComplete += HandleCameraMovementComplete;
        }
    }

    private void OnDisable()
    {
        CameraTagger.OnMainCameraChanged -= HandleMainCameraChanged;
        if (TryGetComponent<CameraController>(out var controller))
        {
            CameraController.OnCameraMovementComplete -= HandleCameraMovementComplete;
        }
    }

    public void Initialize()
    {
        // Debug.Log("CameraOrbit > initializing " + transform.name);

        if (target) transform.LookAt(target);

        Vector3 angles = transform.localEulerAngles;
        azimuthalAngle = angles.y;
        polarAngle = angles.x;

        // Calculate the initial camera position relative to the target object
        Vector3 direction = Quaternion.Euler(polarAngle, azimuthalAngle, 0) * Vector3.back;
        Vector3 targetPosition = target ? target.localPosition : Vector3.zero;
        currentDistance = Vector3.Distance(transform.localPosition, targetPosition);
    }

    private void Start()
    {
        if (waitForCameraController)
        {
            _canOrbit = canOrbit;
            canOrbit = false;
        }
        else
        {
            Initialize();
        }
    }

    private void Update()
    {
        if (!canOrbit) return;

        // Do not use camera orbit if the user has clicked on a UI element
        if (EventSystem.current.IsPointerOverGameObject() && !isDragging) return;

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDragging = true;
            Cursor.visible = false;
            OnStartOrbit?.Invoke();
        }

        if (isDragging)
        {
            // Update azimuthal angle
            azimuthalAngle += Input.GetAxis("Mouse X") * panSpeed;

            // Update polar angle
            polarAngle -= Input.GetAxis("Mouse Y") * panSpeed;

            if (clampAzimuthalAngle)
            {
                azimuthalAngle = Mathf.Clamp(azimuthalAngle, minAzimuthalAngle, maxAzimuthalAngle);
            }
            polarAngle = Mathf.Clamp(polarAngle, minPolarAngle, maxPolarAngle);

            Quaternion rotation = Quaternion.Euler(polarAngle, azimuthalAngle, 0);
            Vector3 direction = rotation * Vector3.back;
            Vector3 targetPosition = target ? target.localPosition : Vector3.zero;
            Vector3 position = targetPosition + direction * currentDistance;

            transform.localPosition = position;
            transform.localRotation = rotation;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Cursor.visible = true;
            OnEndOrbit?.Invoke();
        }
    }

    public void HandleMainCameraChanged()
    {
        // Debug.Log(transform.name + " CameraTagger.OnMainCameraChanged");
        Initialize();
    }

    public void HandleCameraMovementComplete(Vector3 cameraPosition, Quaternion cameraRotation)
    {
        // Debug.Log("Camera movement complete");
        Initialize();
        canOrbit = _canOrbit;
    }
}
