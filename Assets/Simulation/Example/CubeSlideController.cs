using UnityEngine;

public class CubeSlideController : Slides.SimulationSlideController
{
    public bool cubeIsRotating;

    public override void InitializeSlide()
    {
        Debug.Log("Initializing " + transform.name);

        CubeSimulation sim = (CubeSimulation)simulation;

        sim.isRotating = cubeIsRotating;
    }
}
