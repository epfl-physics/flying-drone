// ----------------------------------------------------------------------------------------------------------
// Author: Austin Peel
//
// © All rights reserved. ECOLE POLYTECHNIQUE FEDERALE DE LAUSANNE, Switzerland, Section de Physique, 2024.
// See the LICENSE.md file for more details.
// ----------------------------------------------------------------------------------------------------------
using UnityEngine;

public class CubeSlideController : Slides.SimulationSlideController
{
    public bool cubeIsRotating;
    public bool pauseWhileCameraMoves;

    private CubeSimulation sim;

    private void Awake()
    {
        sim = (CubeSimulation)simulation;
    }

    private void OnEnable()
    {
        CameraController.OnCameraMovementComplete += HandleCameraMotionComplete;
    }

    private void OnDisable()
    {
        CameraController.OnCameraMovementComplete -= HandleCameraMotionComplete;
    }

    public override void InitializeSlide()
    {
        sim.isRotating = cubeIsRotating;

        if (pauseWhileCameraMoves) sim.Pause();
    }

    public void HandleCameraMotionComplete(Vector3 position, Quaternion rotation)
    {
        sim.Resume();
    }
}
