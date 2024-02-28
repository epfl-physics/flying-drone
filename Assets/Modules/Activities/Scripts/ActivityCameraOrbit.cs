// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class ActivityCameraOrbit : CameraOrbit
{
    protected override void OnEnable()
    {
        base.OnEnable();

        CameraTagger.OnMainCameraChanged += HandleMainCameraChanged;
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        CameraTagger.OnMainCameraChanged -= HandleMainCameraChanged;
    }

    public void HandleMainCameraChanged()
    {
        // Debug.Log(transform.name + " CameraTagger.OnMainCameraChanged");
        Initialize();
    }
}
