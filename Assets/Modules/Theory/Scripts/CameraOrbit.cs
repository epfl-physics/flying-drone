using UnityEngine;
using UnityEngine.EventSystems;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;

    [Header("Parameters")]
    public float panSpeed = 1.0f;
    public float minPolarAngle = 20.0f;
    public float maxPolarAngle = 80.0f;
    public float minAzimuthalAngle = -180.0f;
    public float maxAzimuthalAngle = 180.0f;
    public bool clampAzimuthalAngle = true;

    [Header("Options")]
    public bool canOrbit = true;
    public bool ignoreCameraController;

    [Header("Simulation State")]
    public DroneSimulationState simState;

    private bool _canOrbit;

    private float polarAngle;
    private float azimuthalAngle;
    private float currentDistance;
    private bool isDragging;

    private void OnEnable()
    {
        if (!ignoreCameraController) CameraController.OnCameraMovementComplete += HandleCameraMovementComplete;
        CameraTagger.OnMainCameraChanged += HandleMainCameraChanged;
    }

    private void OnDisable()
    {
        if (!ignoreCameraController) CameraController.OnCameraMovementComplete -= HandleCameraMovementComplete;
        CameraTagger.OnMainCameraChanged -= HandleMainCameraChanged;
    }

    private void Initialize()
    {
        // Debug.Log("CameraOrbit > initializing " + transform.name);
        if (simState)
        {
            transform.position = simState.cameraPosition;
            transform.rotation = simState.cameraRotation;
        }

        Vector3 angles = transform.localEulerAngles;
        azimuthalAngle = angles.y;
        polarAngle = angles.x;

        // Calculate the initial camera position relative to the target object
        Vector3 direction = Quaternion.Euler(polarAngle, azimuthalAngle, 0) * Vector3.back;
        Vector3 targetPosition = target ? target.position : Vector3.zero;
        currentDistance = Vector3.Distance(transform.position, targetPosition);
    }

    private void Start()
    {
        if (!ignoreCameraController)
        {
            _canOrbit = canOrbit;
            canOrbit = false;
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
        }

        if (isDragging)
        {
            // Change azimuthal angle
            if (simState)
            {
                if (simState.frameIsInertial || simState.rotationIsZero)
                {
                    azimuthalAngle += Input.GetAxis("Mouse X") * panSpeed;
                }
            }
            else
            {
                azimuthalAngle += Input.GetAxis("Mouse X") * panSpeed;
            }

            // Change polar angle
            polarAngle -= Input.GetAxis("Mouse Y") * panSpeed;

            if (clampAzimuthalAngle)
            {
                azimuthalAngle = Mathf.Clamp(azimuthalAngle, minAzimuthalAngle, maxAzimuthalAngle);
            }
            polarAngle = Mathf.Clamp(polarAngle, minPolarAngle, maxPolarAngle);

            Quaternion rotation = Quaternion.Euler(polarAngle, azimuthalAngle, 0);
            Vector3 direction = rotation * Vector3.back;
            Vector3 targetPosition = target ? target.position : Vector3.zero;
            Vector3 position = targetPosition + direction * currentDistance;

            transform.position = position;
            transform.localRotation = rotation;

            if (simState)
            {
                simState.cameraPosition = position;
                simState.cameraRotation = rotation;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Cursor.visible = true;
        }
    }

    public void HandleCameraMovementComplete(Vector3 cameraPosition, Quaternion cameraRotation)
    {
        // Camera controller transition from home screen

        // Debug.Log("CameraOrbit > HandleCameraMovementComplete()");
        if (simState)
        {
            simState.frameIsInertial = true;
            simState.cameraPosition = cameraPosition;
            simState.cameraRotation = cameraRotation;
        }

        Initialize();
        canOrbit = _canOrbit;
    }

    public void HandleMainCameraChanged()
    {
        Initialize();
    }
}
