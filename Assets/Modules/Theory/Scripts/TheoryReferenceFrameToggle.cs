// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class TheoryReferenceFrameToggle : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.interactable = false;
    }

    private void OnEnable()
    {
        CameraController.OnCameraMovementComplete += HandleCameraMovementComplete;
    }

    private void OnDisable()
    {
        CameraController.OnCameraMovementComplete -= HandleCameraMovementComplete;
    }

    public void HandleCameraMovementComplete(Vector3 position, Quaternion rotation)
    {
        canvasGroup.interactable = true;
    }
}
