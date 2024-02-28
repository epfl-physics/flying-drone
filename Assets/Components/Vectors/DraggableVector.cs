// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using System.Collections.Generic;
using UnityEngine;

public class DraggableVector : Vector
{
    public bool interactable = true;
    [SerializeField] private float dragPlaneDistance = 10f;

    [Header("Interaction Zones")]
    [SerializeField] private VectorClickZone tailClickZone;
    [SerializeField] private VectorClickZone headClickZone;

    [Header("Sticky Point")]
    public bool useStickyPoint;
    public Vector3 stickyPoint;
    public float stickyPointRadius = 0.5f;

    [Header("Sticky Directions")]
    public bool useStickyDirections;
    public List<Vector3> stickyDirections;

    [Header("Magnitude")]
    public bool clampMagnitude;
    public float minMagnitude = 0.2f;
    public float maxMagnitude = 3f;

    // For interactions
    private Vector3 initialPosition;
    private Vector3 dragStartPosition;
    private Camera mainCamera;
    private Plane dragPlane;

    private bool draggingTail;
    private bool draggingHead;

    // For resetting
    private Vector3 resetPosition;
    private Vector3 resetComponents;

    private void Awake()
    {
        resetPosition = transform.position;
        resetComponents = components;
    }

    public override void OnEnable()
    {
        base.OnEnable();

        Redraw();

        VectorClickZone.OnZoneMouseDown += HandleZoneMouseDown;
        VectorClickZone.OnZoneMouseUp += HandleZoneMouseUp;
    }

    private void OnDisable()
    {
        VectorClickZone.OnZoneMouseDown -= HandleZoneMouseDown;
        VectorClickZone.OnZoneMouseUp -= HandleZoneMouseUp;
    }

    public void HandleZoneMouseDown(VectorClickZone clickZone)
    {
        if (!interactable) return;

        if (clickZone == tailClickZone)
        {
            draggingTail = true;
        }
        else if (clickZone == headClickZone)
        {
            draggingHead = true;
        }
    }

    public void HandleZoneMouseUp(VectorClickZone clickZone)
    {
        if (clickZone == tailClickZone)
        {
            draggingTail = false;
        }
        else if (clickZone == headClickZone)
        {
            draggingHead = false;
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;
        // Create a plane at a fixed distance from the camera, perpendicular to the camera's forward direction
        Vector3 planeNormal = -mainCamera.transform.forward;
        Vector3 planePosition = mainCamera.transform.position + dragPlaneDistance * mainCamera.transform.forward;
        dragPlane = new Plane(planeNormal, planePosition);
    }

    public override void Redraw()
    {
        base.Redraw();

        if (headClickZone) headClickZone.transform.position = transform.position + components;
    }

    private void Update()
    {
        if (draggingTail || draggingHead)
        {
            // Create a ray from the mouse click position
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            // Initialise the enter variable
            float enter = 0.0f;

            if (dragPlane.Raycast(ray, out enter))
            {
                // Get the point that is clicked
                Vector3 hitPoint = ray.GetPoint(enter);

                if (draggingTail)
                {
                    // Move the vector to the clicked point
                    transform.position = hitPoint;
                    if (useStickyPoint)
                    {
                        if (Vector3.Distance(hitPoint, stickyPoint) <= stickyPointRadius)
                        {
                            transform.position = stickyPoint;
                        }
                    }
                }
                else
                {
                    // Update components
                    Vector3 newComponents = hitPoint - transform.position;

                    // Snap the direction
                    if (useStickyDirections && transform.position == stickyPoint)
                    {
                        foreach (var direction in stickyDirections)
                        {
                            float cosAngle = Vector3.Dot(newComponents, direction);
                            cosAngle /= (newComponents.magnitude * direction.magnitude);
                            if (cosAngle > 0.98f)
                            {
                                newComponents = newComponents.magnitude * direction.normalized;
                                break;
                            }
                        }
                    }

                    if (clampMagnitude)
                    {
                        newComponents = Vector3.ClampMagnitude(newComponents, maxMagnitude);
                        if (newComponents.magnitude < minMagnitude)
                        {
                            newComponents = minMagnitude * newComponents.normalized;
                        }
                    }

                    components = newComponents;
                    Redraw();
                }
            }
        }
    }

    public void SetClickZoneColors()
    {
        Color clickZoneColor = color - new Color(0, 0, 0, 0.8f);
        if (tailClickZone) tailClickZone.SetColor(clickZoneColor);
        if (headClickZone) headClickZone.SetColor(clickZoneColor);
    }

    public void MakeInteractable()
    {
        SetInteractable(true);
    }

    public void SetInteractable(bool value)
    {
        interactable = value;

        if (headClickZone)
        {
            headClickZone.gameObject.SetActive(interactable);
            headClickZone.interactable = interactable;
        }

        if (tailClickZone)
        {
            tailClickZone.gameObject.SetActive(interactable);
            tailClickZone.interactable = interactable;
        }
    }

    public void Reset()
    {
        transform.position = resetPosition;
        components = resetComponents;
        Redraw();

        HideHeadClickZone();
        HideTailClickZone();
    }

    public void ShowTailClickZone(bool interactable)
    {
        if (tailClickZone)
        {
            tailClickZone.gameObject.SetActive(true);
            tailClickZone.interactable = interactable;
            tailClickZone.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void HideTailClickZone()
    {
        if (tailClickZone)
        {
            tailClickZone.gameObject.SetActive(false);
            tailClickZone.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    public void ShowHeadClickZone(bool interactable)
    {
        if (headClickZone)
        {
            headClickZone.gameObject.SetActive(true);
            headClickZone.interactable = interactable;
            headClickZone.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    public void HideHeadClickZone()
    {
        if (headClickZone)
        {
            headClickZone.gameObject.SetActive(false);
            headClickZone.GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
