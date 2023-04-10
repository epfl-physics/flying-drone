using UnityEngine;

public class CubeSlideController : Slides.SimulationSlideController
{
    public bool cubeIsRotating;

    public override void InitializeSlide()
    {
        CubeSimulation sim = (CubeSimulation)simulation;

        sim.isRotating = cubeIsRotating;
    }
}
