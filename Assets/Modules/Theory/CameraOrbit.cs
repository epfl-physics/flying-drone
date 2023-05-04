using UnityEngine;
using UnityEngine.EventSystems;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;
    public float panSpeed = 1.0f;
    public float minPolarAngle = 20.0f;
    public float maxPolarAngle = 80.0f;
    public float minAzimuthalAngle = -180.0f;
    public float maxAzimuthalAngle = 180.0f;
    public bool canOrbit;

    private float polarAngle;
    private float azimuthalAngle;
    private float currentDistance;
    private bool isDragging;

    private void OnEnable()
    {
        CameraController.OnCameraMovementComplete += HandleCameraMovementComplete;
    }

    private void OnDisable()
    {
        CameraController.OnCameraMovementComplete -= HandleCameraMovementComplete;
    }

    private void Initialize()
    {
        Vector3 angles = transform.eulerAngles;
        azimuthalAngle = angles.y;
        polarAngle = angles.x;

        // Calculate the initial camera position relative to the target object
        Vector3 direction = Quaternion.Euler(polarAngle, azimuthalAngle, 0) * Vector3.back;
        currentDistance = Vector3.Distance(transform.position, target.position);
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        // Do not use camera orbit if the user has clicked on a UI element
        if (EventSystem.current.IsPointerOverGameObject() && !isDragging)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            isDragging = true;
            Cursor.visible = false;
        }

        if (isDragging)
        {
            azimuthalAngle += Input.GetAxis("Mouse X") * panSpeed;
            polarAngle -= Input.GetAxis("Mouse Y") * panSpeed;

            azimuthalAngle = Mathf.Clamp(azimuthalAngle, minAzimuthalAngle, maxAzimuthalAngle);
            polarAngle = Mathf.Clamp(polarAngle, minPolarAngle, maxPolarAngle);

            Quaternion rotation = Quaternion.Euler(polarAngle, azimuthalAngle, 0);
            Vector3 direction = rotation * Vector3.back;
            Vector3 position = target.position + direction * currentDistance;

            transform.rotation = rotation;
            transform.position = position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            Cursor.visible = true;
        }
    }

    public void HandleCameraMovementComplete(Vector3 cameraPosition, Quaternion cameraRotation)
    {
        // Debug.Log("CameraOrbit > HandleCameraMovementComplete()");
        Initialize();
        canOrbit = true;
    }
}
