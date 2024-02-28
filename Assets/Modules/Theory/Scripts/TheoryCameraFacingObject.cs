// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
﻿using UnityEngine;

[ExecuteInEditMode]
public class TheoryCameraFacingObject : MonoBehaviour
{
    public bool matchRotation;

    private Camera mainCamera;

    private void OnEnable()
    {
        CameraTagger.OnMainCameraChanged += ReleaseCameraReference;
    }

    private void OnDisable()
    {
        CameraTagger.OnMainCameraChanged -= ReleaseCameraReference;
    }

    private void Update()
    {
        if (!mainCamera) mainCamera = Camera.main;

        transform.forward = mainCamera.transform.forward;

        if (matchRotation) transform.rotation = mainCamera.transform.rotation;
    }

    public void ReleaseCameraReference()
    {
        mainCamera = null;
    }
}
